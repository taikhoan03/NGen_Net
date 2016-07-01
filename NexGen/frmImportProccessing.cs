using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
namespace NexGen
{
    public partial class frmImportProccessing : Form
    {
        public static string name { get; set; }
        public static int index { get; set; }
        public static int total { get; set; }
        public static int fails { get; set; }
        public static string OCRText { get; set; }
        //List<string> fileName { get; set; }
        public frmImportProccessing()
        {
            InitializeComponent();
            total = 100;
            btnClose.Visible = false;
            lblOCR.Text = "";
        }
        System.Threading.Thread tUpdate;
        

        public static Task task;

        public void updateProccess()
        {

                lblName.Invoke((Action)delegate { 
                    lblName.Text = frmImportProccessing.name;
                    progressBar1.Maximum = frmImportProccessing.total;
                    progressBar1.Value = frmImportProccessing.index;
                    progressBar1.Increment(1);
                    lblNoteCurrentIndex.Text = string.Format("{0}/{1} Doc(s)", index + 1, total + 1);
                    lblMessageFail.Text = string.Format("{0} fails", fails);
                    lblOCR.Text = frmImportProccessing.OCRText;
                });
                if (frmImportProccessing.index == frmImportProccessing.total)
                {
                    
                    return;
                }
        }

        private void frmImportProccessing_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}
