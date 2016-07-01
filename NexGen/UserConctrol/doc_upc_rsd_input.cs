using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NexGen.UserConctrol
{
    public partial class doc_upc_rsd_input : UserControl
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region properties

        public string _TransactionType { get; set; }

        public string _TransactionDate { get; set; }

        private int _sr_no = 0;
        private Control _focused_control;

        public Control Get_focused_control()
        {
            return _focused_control;
        }
        
        #endregion properties
        
        public doc_upc_rsd_input()
        {
            InitializeComponent();
            cboxItemType.Populate();
            txtUPCCode.Focus();
            //added events for OCR
            txt_rsd_description.Enter += new EventHandler(evt_control_focused);
            txt_rsd_description.CharacterCasing = CharacterCasing.Upper;
            txtUPCCode.Enter += new EventHandler(evt_control_focused);
            txtPrice.Enter += new EventHandler(evt_control_focused);
            txtQty.Enter += new EventHandler(evt_control_focused);
        }

        public DataObject.Record_UPC_RSD GetRecord()
        {
            if (string.IsNullOrEmpty(_TransactionType))
            {
                return null;   
            }
            //if(string.IsNullOrEmpty(_TransactionDate))
            //{
            //    return null;   
            //}
            
            //collect data
            try
            {
                int qty = 0;
                double price = 0;
                try
                {
                    qty = Convert.ToInt32(txtQty.Text);
                }
                catch (Exception)
                {
                    txtQty.Focus();
                    throw;
                }
                try
                {
                    price = Convert.ToDouble(txtPrice.Text);
                    if(cboxItemType.SelectedItem+string.Empty==DataObject.EVRData.Item_type_Coupon || 
                        cboxItemType.SelectedItem+string.Empty==DataObject.EVRData.Item_type_Void)
                        price=Convert.ToDouble("-"+txtPrice.Text);
                }
                catch (Exception)
                {
                    txtPrice.Focus();
                    throw;
                }
                //double.TryParse(txtPrice.Text, out price);
                //rule Qty
                //if (cboxItemType.SelectedItem + string.Empty == DataObject.EVRData.Item_type_Coupon)
                //    qty = 0;
                //if (cboxItemType.SelectedItem + string.Empty == DataObject.EVRData.Item_type_Void)
                //    qty = qty*-1;
                double per_item = price / qty;
                //if(per_item==double.NegativeInfinity || per_item==double.PositiveInfinity)
                //{
                //    per_item = 0;
                //}
                _sr_no++;
                var rs = new DataObject.Record_UPC_RSD
                {
                    //Image_2 = null,
                    Price = price,
                    Qty = qty,
                    Per_Item = per_item,
                    //Receipt_Images_1 = null,
                    Remarks = null,
                    Sr_No = _sr_no,
                    Upc_code = txtUPCCode.Text.Trim(),
                    //Transaction_Date = DateTime.Parse(_TransactionDate),
                    Description=txt_rsd_description.Text,
                    Transaction_Type = _TransactionType,
                    Type = cboxItemType.SelectedItem + string.Empty,
                    Role_author = DataObject.EVRData.Role_author_keyer,
                    Item_type = cboxItemType.Text,
                };
                //clear
                txtUPCCode.Text = string.Empty;
                txtPrice.Text = string.Empty;
                txtQty.Text = "1";
                txt_rsd_description.Text = string.Empty;
                //txtItemNumber.Focus();
                txtUPCCode.Focus();
                cboxItemType.SelectedItem = DataObject.EVRData.Item_type_Item;
                return rs;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
            return null;
        }

        public void Load_record_to_editting_form(DataObject.Record_UPC_RSD doc_rin_input)
        {
            txtUPCCode.Text = doc_rin_input.Upc_code;
            txtPrice.Text = (doc_rin_input.Price + string.Empty).Replace("-",string.Empty);
            txtQty.Text = doc_rin_input.Qty + string.Empty;
            cboxItemType.SelectedItem = doc_rin_input.Item_type;
        }

        public void resetForm()
        {
            txtUPCCode.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtQty.Text = "1";
            //txtItemNumber.Focus();
            cboxItemType.SelectedItem = DataObject.EVRData.Item_type_Item;
        }

        private void evt_control_focused(object sender, EventArgs e)
        {
            try
            {
                var control = (Control)sender;
                _focused_control = control;
                if (control.GetType() == typeof(TextBox))
                    ((TextBox)sender).SelectAll();
            }
            catch (Exception ex)
            {
                _focused_control = null;
                logger.Error(ex.Message);
            }
        }

        private void cboxItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(cboxItemType.SelectedItem+string.Empty==DataObject.EVRData.Item_type_Coupon)
            //    txtQty.Text = "0";
        }
    }
}