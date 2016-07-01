using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Libs;

namespace NexGen
{
    public partial class RecordList : Form
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static BindingList<DataObject.Record_base> _list_record;
        private DataObject.Record_base _current_record;
        public string[] DisplayColumns,Columns_rin,Columns_upc,Columns_rsd, Columns_rin_rsd, Columns_upc_rsd;
        private static RecordList instance;
        private string CONST_sname = "RecordList";
        public ToolStripMenuItem mnuUpdate = new ToolStripMenuItem("Update");

        public Form caller { get; set; }

        public void bind_grid()
        {
            if (_list_record == null || _list_record.Count < 0)
            {
                return;
            }
            if (_list_record.Count > 0)
            {
                var itemFactor = _list_record[0];
                if (itemFactor.GetType().Equals(typeof(DataObject.Record_RIN)))
                {
                    dgListRecord.DataSource = new BindingList<DataObject.Record_RIN>((_list_record.Cast<DataObject.Record_RIN>()).ToList());//
                    if (Columns_rin != null)
                        DisplayColumns = Columns_rin;
                }
                else if (itemFactor.GetType().Equals(typeof(DataObject.Record_RSD)))
                {
                    dgListRecord.DataSource = new BindingList<DataObject.Record_RSD>((_list_record.Cast<DataObject.Record_RSD>()).ToList());//
                    if (Columns_rsd != null)
                        DisplayColumns = Columns_rsd;
                }
                else if (itemFactor.GetType().Equals(typeof(DataObject.Record_UPC)))
                {
                    dgListRecord.DataSource = new BindingList<DataObject.Record_UPC>((_list_record.Cast<DataObject.Record_UPC>()).ToList());//
                    if (Columns_upc != null)
                        DisplayColumns = Columns_upc;
                }
                else if (itemFactor.GetType().Equals(typeof(DataObject.Record_RIN_RSD)))
                {
                    dgListRecord.DataSource = new BindingList<DataObject.Record_RIN_RSD>((_list_record.Cast<DataObject.Record_RIN_RSD>()).ToList());//
                    if (Columns_upc != null)
                        DisplayColumns = Columns_rin_rsd;
                }
                else if (itemFactor.GetType().Equals(typeof(DataObject.Record_UPC_RSD)))
                {
                    dgListRecord.DataSource = new BindingList<DataObject.Record_UPC_RSD>((_list_record.Cast<DataObject.Record_UPC_RSD>()).ToList());//
                    if (Columns_upc != null)
                        DisplayColumns = Columns_upc_rsd;
                }
            }

            dgListRecord.Columns.Clear();
            DataGridViewColumn column;
            foreach (string item in DisplayColumns)
            {
                if (item == "Item_type")
                {
                    column = new DataGridViewComboBoxColumn();
                    ((DataGridViewComboBoxColumn)column).DataSource = new List<string> { "Item", "Void", "Coupon" };
                }
                else
                    column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = item;
                column.HeaderText = item;
                if (item == "Per_Item")
                    column.ReadOnly = true;
                //column.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgListRecord.Columns.Add(column);
            }
            
            //if (_list_record.Count > 0)
            //{
            //    var itemFactor = _list_record[0];
            //    if (itemFactor.GetType().Equals(typeof(DataObject.Record_RIN)))
            //    {
            //        dgListRecord.DataSource = new BindingList<DataObject.Record_RIN>((_list_record.Cast<DataObject.Record_RIN>()).ToList());//
            //    }
            //    if (itemFactor.GetType().Equals(typeof(DataObject.Record_RSD)))
            //    {
            //        dgListRecord.DataSource = new BindingList<DataObject.Record_RSD>((_list_record.Cast<DataObject.Record_RSD>()).ToList());//
            //    }
            //    if (itemFactor.GetType().Equals(typeof(DataObject.Record_UPC)))
            //    {
            //        dgListRecord.DataSource = new BindingList<DataObject.Record_UPC>((_list_record.Cast<DataObject.Record_UPC>()).ToList());//
            //    }
            //}
            //dgListRecord.DataSource = _list_record;
            if (caller != null)
            {
                var _caller = ((From_Keyer)caller);
                if(_caller.Counter!=null)
                {
                    //_caller.Counter.Num_of_records += _list_record.Count;
                    _caller.Update_counter_label(_caller.Counter.Num_of_records + _list_record.Count);
                }
                
            }
                            
        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check the value of the e.ColumnIndex property if you want to apply this formatting only so some rcolumns.
            if (e.Value != null)
            {
                e.Value = e.Value.ToString().ToUpper();
                e.FormattingApplied = true;
            }
        }
        public void set_data(List<DataObject.Record_base> base_list)
        {
            _list_record = new BindingList<DataObject.Record_base>(base_list);//base_list;// new BindingList<DataObject.Record_base>(base_list);//
            bind_grid();
        }

        public List<T> get_data<T>()
        {
            if (_list_record == null)
                _list_record = new BindingList<DataObject.Record_base>();
            //sap xep lai thu tu
            int new_sr_no = 0;
            foreach (var item in _list_record)
            {
                new_sr_no++;
                item.Sr_No = new_sr_no;
                //re-calculate list
                if (item.Item_type == DataObject.EVRData.Item_type_Coupon ||
                    item.Item_type == DataObject.EVRData.Item_type_Void)
                {
                    if (item.GetType() == typeof(DataObject.Record_RIN))
                    {
                        var p_obj = (DataObject.Record_RIN)item;
                        p_obj.Price = -Math.Abs(p_obj.Price);
                        p_obj.Per_Item = p_obj.Price / p_obj.Qty;
                    }
                    else if (item.GetType() == typeof(DataObject.Record_RSD))
                    {
                        var p_obj = (DataObject.Record_RSD)item;
                        p_obj.Price = -Math.Abs(p_obj.Price);
                        p_obj.Per_Item = p_obj.Price / p_obj.Qty;
                    }
                    else if (item.GetType() == typeof(DataObject.Record_UPC))
                    {
                        var p_obj = (DataObject.Record_UPC)item;
                        p_obj.Price = -Math.Abs(p_obj.Price);
                        p_obj.Per_Item = p_obj.Price / p_obj.Qty;
                    }
                }
            }
            //_list_record.ForEach(item =>
            //{
            //    new_sr_no++;
            //    item.Sr_No = new_sr_no;
            //});
            return _list_record.Cast<T>().ToList();
        }

        public void add_record(DataObject.Record_base record)
        {
            //added by cuong.ha 2014-11-08
            //[AT] Copy record khi F63 = Y (1900); F120 = Y (ADC) thi ko cho copy nua
            if (record.GetType().Equals(typeof(DataObject.Record_RIN)))
                _list_record.Add((DataObject.Record_RIN)record.Clone());
            else if (record.GetType().Equals(typeof(DataObject.Record_RSD)))
                _list_record.Add((DataObject.Record_RSD)record.Clone());
            else if (record.GetType().Equals(typeof(DataObject.Record_UPC)))
                _list_record.Add((DataObject.Record_UPC)record.Clone());
            else if (record.GetType().Equals(typeof(DataObject.Record_RIN_RSD)))
                _list_record.Add((DataObject.Record_RIN_RSD)record.Clone());
            else if (record.GetType().Equals(typeof(DataObject.Record_UPC_RSD)))
                _list_record.Add((DataObject.Record_UPC_RSD)record.Clone());
            bind_grid();
            //added by cuong.ha 2014-11-20
            //68: [AT] Request: Khi copy record, tự động chuyển sang record vừa copy
            try
            {
                this._current_record = _list_record.LastOrDefault();
                //keydocEdit = _list_record().Last();
                dgListRecord.FirstDisplayedScrollingRowIndex = dgListRecord.RowCount-1;
            }
            catch (Exception)
            {
            }
        }

        public RecordList()
        {
            InitializeComponent();
            
            _list_record = new BindingList<DataObject.Record_base>();
            panel1.Dock = DockStyle.Fill;
            dgListRecord.Dock = DockStyle.Fill;
            dgListRecord.AutoGenerateColumns = false;
            if (instance != null)
            {
                if (instance.DisplayColumns != null)
                    this.DisplayColumns = instance.DisplayColumns;
                if (instance.Columns_rin != null)
                    this.Columns_rin = instance.Columns_rin;
                if (instance.Columns_rsd != null)
                    this.Columns_rsd = instance.Columns_rsd;
                if (instance.Columns_upc != null)
                    this.Columns_upc = instance.Columns_upc;
                if (instance.Columns_rin_rsd != null)
                    this.Columns_rin_rsd = instance.Columns_rin_rsd;
                if (instance.Columns_upc_rsd != null)
                    this.Columns_upc_rsd = instance.Columns_upc_rsd;
            }
            //Update and delete cmd
            ContextMenuStrip mnu = new ContextMenuStrip();
            ToolStripMenuItem mnuInsert = new ToolStripMenuItem("Insert here");
            ToolStripMenuItem mnuDelete = new ToolStripMenuItem("Delete");
            ToolStripMenuItem mnuDeleteAll = new ToolStripMenuItem("DeleteAll");
            
            //Assign event handlers
            mnuDelete.Click += new EventHandler(grid_delete_menu_click);
            mnuDeleteAll.Click += new EventHandler(grid_delete_all_menu_click);
            mnuInsert.Click += new EventHandler(grid_Insert_menu_click);
            //Add to main context menu
            mnu.Items.AddRange(new ToolStripItem[] { mnuInsert, mnuUpdate, mnuDelete, mnuDeleteAll });
            //Assign to datagridview
            dgListRecord.ContextMenuStrip = mnu;
            //dgListRecord.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //dgListRecord.CurrentCellDirtyStateChanged += (sender,e) =>
            //{
            //    if (dgListRecord.IsCurrentCellDirty)
            //    {
            //        dgListRecord.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //    }
            //};
            dgListRecord.CellEndEdit += (sender, e) =>
            {
                try
                {
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        try
                        {
                            var record = _list_record[e.RowIndex];
                            //if (dgListRecord.Columns[e.ColumnIndex].HeaderText == "Qty" || dgListRecord.Columns[e.ColumnIndex].HeaderText == "Price")
                            //{
                            
                            //    record.Per_Item = record.Price / record.Qty;
                            //}
                            if (record.Item_type == "Item")
                                    record.Price = Math.Abs(record.Price);
                                else
                                {
                                    record.Price = Math.Abs(record.Price) * -1;
                                }
                            //if (dgListRecord.Columns[e.ColumnIndex].HeaderText == "Type")
                            //{
                            //    if (record.Type == "Item")
                            //        record.Price = Math.Abs(record.Price);
                            //    else
                            //    {
                            //        record.Price = Math.Abs(record.Price) * -1;
                            //    }
                            //}
                            record.Per_Item = record.Price / record.Qty;
                        }
                        catch (Exception ex_calculation)
                        {
                            logger.Error(ex_calculation.Message);
                            logger.Error(ex_calculation.StackTrace);
                        }
                        if (caller != null)
                            ((From_Keyer)caller)._update_label_info();
                        //bind_grid();
                    }));
                }
                catch (Exception)
                {
                }
            };
            dgListRecord.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridView1_CellFormatting);
        }

        private void grid_Insert_menu_click(object obj, EventArgs e)
        {
            //var source = _list_record;//(List<DataObject.Record_base>)dgListRecord.DataSource;
            try
            {
                int idx = dgListRecord.SelectedCells[0].RowIndex;
                var value = (DataObject.Record_base)_list_record[idx].Clone();
                value.Sr_No = 0;
                value.Qty = 1;
                value.Price = 0;
                value.Per_Item = 0;
                value.Item_type = DataObject.EVRData.Item_type_Item;
                if (value.GetType() == typeof(DataObject.Record_RIN))
                {
                    var p_obj = (DataObject.Record_RIN)value;
                    p_obj.Item_number = string.Empty;
                }
                else if (value.GetType() == typeof(DataObject.Record_RSD))
                {
                    var p_obj = (DataObject.Record_RSD)value;
                    p_obj.Item_description = string.Empty;
                }
                else if (value.GetType() == typeof(DataObject.Record_UPC))
                {
                    var p_obj = (DataObject.Record_UPC)value;
                    p_obj.Upc_code = string.Empty;
                }
                else if (value.GetType() == typeof(DataObject.Record_RIN_RSD))
                {
                    var p_obj = (DataObject.Record_RIN_RSD)value;
                    p_obj.Item_number = string.Empty;
                    p_obj.Description = string.Empty;
                }
                else if (value.GetType() == typeof(DataObject.Record_UPC_RSD))
                {
                    var p_obj = (DataObject.Record_UPC_RSD)value;
                    p_obj.Upc_code = string.Empty;
                    p_obj.Description = string.Empty;
                }
                //source.Remove(value);
                _list_record.Insert(idx, value);
                bind_grid();
            }
            catch (Exception)
            {
            }
        }

        private void grid_delete_all_menu_click(object obj, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure delete all records?", "Confirm", MessageBoxButtons.OKCancel);
            if (confirm == System.Windows.Forms.DialogResult.OK)
            {
                _list_record = new BindingList<DataObject.Record_base>();
                bind_grid();
                if (caller != null)
                {
                    if (caller.GetType() == typeof(From_Keyer))
                    {
                        ((From_Keyer)caller)._update_label_info();
                    }
                }
            }
        }

        private void grid_delete_menu_click(object obj, EventArgs e)
        {
            if (dgListRecord.SelectedRows.Count <= 0)
                return;

            var firstSelectedRow = dgListRecord.SelectedRows[0];
            var record = (DataObject.Record_base)firstSelectedRow.DataBoundItem;
            _list_record.Remove(record);
            bind_grid();
            if (caller != null)
            {
                if (caller.GetType() == typeof(From_Keyer))
                {
                    ((From_Keyer)caller)._update_label_info();
                }
            }
        }
        
        public DataObject.Record_base Get_selected_record()
        {
            if (dgListRecord.SelectedRows.Count <= 0)
                return null;

            var firstSelectedRow = dgListRecord.SelectedRows[0];
            return (DataObject.Record_base)firstSelectedRow.DataBoundItem;
        }

        public void UpdateRecord(DataObject.Record_base record)
        {
            var record_in_list = _list_record.Where(p => p.Sr_No == record.Sr_No).FirstOrDefault();
            if (record_in_list != null)
            {
                record_in_list.Per_Item = record.Per_Item;
                record_in_list.Price = record.Price;
                record_in_list.Qty = record.Qty;
                record_in_list.Remarks = record.Remarks;
                record_in_list.Type = record.Type;
                bind_grid();
            }
        }

        public void populate()
        {
            if (instance == null)
            {
                instance = (RecordList)ApplicationContextFactory.GetContext().GetObject(CONST_sname);
                
                if (instance.DisplayColumns != null)
                    this.DisplayColumns = instance.DisplayColumns;
                if (instance.Columns_rin != null)
                    this.Columns_rin = instance.Columns_rin;
                if (instance.Columns_rsd != null)
                    this.Columns_rsd = instance.Columns_rsd;
                if (instance.Columns_upc != null)
                    this.Columns_upc = instance.Columns_upc;
                if (instance.Columns_rin_rsd != null)
                    this.Columns_rin_rsd = instance.Columns_rin_rsd;
                if (instance.Columns_upc_rsd != null)
                    this.Columns_upc_rsd = instance.Columns_upc_rsd;
            }
        }

        public void test()
        {
            var a = 1;
        }

        private void RecordList_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}