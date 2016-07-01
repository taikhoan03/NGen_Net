using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NexGen.Controls
{
    public class Combobox_ItemType:System.Windows.Forms.ComboBox
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<string> _arraySource;
        private const string CONST_sname = "Combobox_ItemType";
        private static Combobox_ItemType instancte;

        public Combobox_ItemType()
        {
        }

        public void Populate(bool addFirstBlankItem=false)
        {
            try
            {
                if (instancte == null)
                    instancte = (Combobox_ItemType)ApplicationContextFactory.GetContext().GetObject(CONST_sname);
                if(addFirstBlankItem)
                {
                    instancte._arraySource.Insert(0, string.Empty);
                }
                this.DataSource = instancte._arraySource;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        public bool choseOneValid()
        {
            if (string.IsNullOrEmpty(this.SelectedItem + string.Empty))
                return false;
            else if (string.IsNullOrEmpty(this.Text))
                return false;
            else
                return true;
        }
    }
}
