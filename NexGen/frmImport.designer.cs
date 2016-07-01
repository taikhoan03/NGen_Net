namespace NexGen
{
    partial class frmImport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtImportURL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnImport = new System.Windows.Forms.Button();
            this.dgFiletoImport = new System.Windows.Forms.DataGridView();
            this.ColFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStateCode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColCountyID = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColFilmTrackerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBatchNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMonthOfCounty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColYearOfCounty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.dgIndex = new System.Windows.Forms.DataGridView();
            this.ClmBeforeIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClmAfterIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRunIndex = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnLoadIndex = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboxDocType = new NexGen.Controls.Combobox_Doctype();
            this.txtFilmTrackerID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCSVFile = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgFiletoImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgIndex)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtImportURL
            // 
            this.txtImportURL.Location = new System.Drawing.Point(95, 23);
            this.txtImportURL.Margin = new System.Windows.Forms.Padding(4);
            this.txtImportURL.Name = "txtImportURL";
            this.txtImportURL.Size = new System.Drawing.Size(275, 22);
            this.txtImportURL.TabIndex = 0;
            this.txtImportURL.Text = "D:\\Temp\\Import";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Import from";
            // 
            // BtnImport
            // 
            this.BtnImport.Location = new System.Drawing.Point(752, 399);
            this.BtnImport.Margin = new System.Windows.Forms.Padding(4);
            this.BtnImport.Name = "BtnImport";
            this.BtnImport.Size = new System.Drawing.Size(100, 28);
            this.BtnImport.TabIndex = 3;
            this.BtnImport.Text = "Import";
            this.BtnImport.UseVisualStyleBackColor = true;
            this.BtnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // dgFiletoImport
            // 
            this.dgFiletoImport.AllowUserToAddRows = false;
            this.dgFiletoImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgFiletoImport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColFileName,
            this.ColStateCode,
            this.ColCountyID,
            this.ColFilmTrackerID,
            this.ColBatchNum,
            this.ColMonthOfCounty,
            this.ColYearOfCounty});
            this.dgFiletoImport.Location = new System.Drawing.Point(8, 68);
            this.dgFiletoImport.Margin = new System.Windows.Forms.Padding(4);
            this.dgFiletoImport.Name = "dgFiletoImport";
            this.dgFiletoImport.Size = new System.Drawing.Size(845, 180);
            this.dgFiletoImport.TabIndex = 2;
            // 
            // ColFileName
            // 
            this.ColFileName.FillWeight = 120F;
            this.ColFileName.HeaderText = "File Name";
            this.ColFileName.Name = "ColFileName";
            this.ColFileName.Width = 140;
            // 
            // ColStateCode
            // 
            this.ColStateCode.HeaderText = "State";
            this.ColStateCode.Name = "ColStateCode";
            this.ColStateCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColStateCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColStateCode.Width = 70;
            // 
            // ColCountyID
            // 
            this.ColCountyID.HeaderText = "County";
            this.ColCountyID.Name = "ColCountyID";
            this.ColCountyID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColCountyID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColCountyID.Width = 130;
            // 
            // ColFilmTrackerID
            // 
            this.ColFilmTrackerID.HeaderText = "Film";
            this.ColFilmTrackerID.Name = "ColFilmTrackerID";
            this.ColFilmTrackerID.Width = 70;
            // 
            // ColBatchNum
            // 
            this.ColBatchNum.HeaderText = "Batch";
            this.ColBatchNum.Name = "ColBatchNum";
            this.ColBatchNum.Width = 60;
            // 
            // ColMonthOfCounty
            // 
            this.ColMonthOfCounty.HeaderText = "Month";
            this.ColMonthOfCounty.Name = "ColMonthOfCounty";
            this.ColMonthOfCounty.Width = 60;
            // 
            // ColYearOfCounty
            // 
            this.ColYearOfCounty.HeaderText = "Year";
            this.ColYearOfCounty.Name = "ColYearOfCounty";
            this.ColYearOfCounty.Width = 60;
            // 
            // btnLoadData
            // 
            this.btnLoadData.Location = new System.Drawing.Point(753, 21);
            this.btnLoadData.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(100, 28);
            this.btnLoadData.TabIndex = 4;
            this.btnLoadData.Text = "Load Data";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // dgIndex
            // 
            this.dgIndex.AllowUserToAddRows = false;
            this.dgIndex.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgIndex.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ClmBeforeIndex,
            this.ClmAfterIndex});
            this.dgIndex.Location = new System.Drawing.Point(8, 23);
            this.dgIndex.Margin = new System.Windows.Forms.Padding(4);
            this.dgIndex.Name = "dgIndex";
            this.dgIndex.Size = new System.Drawing.Size(736, 439);
            this.dgIndex.TabIndex = 5;
            // 
            // ClmBeforeIndex
            // 
            this.ClmBeforeIndex.HeaderText = "Before Index";
            this.ClmBeforeIndex.Name = "ClmBeforeIndex";
            this.ClmBeforeIndex.Width = 250;
            // 
            // ClmAfterIndex
            // 
            this.ClmAfterIndex.HeaderText = "After Index";
            this.ClmAfterIndex.Name = "ClmAfterIndex";
            this.ClmAfterIndex.Width = 250;
            // 
            // btnRunIndex
            // 
            this.btnRunIndex.Location = new System.Drawing.Point(752, 59);
            this.btnRunIndex.Margin = new System.Windows.Forms.Padding(4);
            this.btnRunIndex.Name = "btnRunIndex";
            this.btnRunIndex.Size = new System.Drawing.Size(100, 28);
            this.btnRunIndex.TabIndex = 6;
            this.btnRunIndex.Text = "Run Index";
            this.btnRunIndex.UseVisualStyleBackColor = true;
            this.btnRunIndex.Click += new System.EventHandler(this.btnRunIndex_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.dgFiletoImport);
            this.groupBox1.Controls.Add(this.txtImportURL);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnLoadData);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(860, 255);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Directory";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(379, 21);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 28);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgIndex);
            this.groupBox2.Controls.Add(this.btnLoadIndex);
            this.groupBox2.Controls.Add(this.BtnDelete);
            this.groupBox2.Controls.Add(this.BtnImport);
            this.groupBox2.Controls.Add(this.btnRunIndex);
            this.groupBox2.Location = new System.Drawing.Point(16, 305);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(860, 470);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Index";
            // 
            // btnLoadIndex
            // 
            this.btnLoadIndex.Location = new System.Drawing.Point(753, 23);
            this.btnLoadIndex.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadIndex.Name = "btnLoadIndex";
            this.btnLoadIndex.Size = new System.Drawing.Size(100, 28);
            this.btnLoadIndex.TabIndex = 6;
            this.btnLoadIndex.Text = "Load Index";
            this.btnLoadIndex.UseVisualStyleBackColor = true;
            this.btnLoadIndex.Click += new System.EventHandler(this.btnLoadIndex_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.Location = new System.Drawing.Point(752, 434);
            this.BtnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(100, 28);
            this.BtnDelete.TabIndex = 3;
            this.BtnDelete.Text = "Delete";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboxDocType);
            this.panel1.Controls.Add(this.txtFilmTrackerID);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(885, 36);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(555, 254);
            this.panel1.TabIndex = 9;
            // 
            // cboxDocType
            // 
            this.cboxDocType.FormattingEnabled = true;
            this.cboxDocType.Location = new System.Drawing.Point(167, 24);
            this.cboxDocType.Name = "cboxDocType";
            this.cboxDocType.Size = new System.Drawing.Size(121, 24);
            this.cboxDocType.TabIndex = 1;
            // 
            // txtFilmTrackerID
            // 
            this.txtFilmTrackerID.Location = new System.Drawing.Point(167, 69);
            this.txtFilmTrackerID.Name = "txtFilmTrackerID";
            this.txtFilmTrackerID.Size = new System.Drawing.Size(121, 22);
            this.txtFilmTrackerID.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Film Tracker ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Doc type:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 278);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "VSC file:";
            // 
            // txtCSVFile
            // 
            this.txtCSVFile.Location = new System.Drawing.Point(95, 278);
            this.txtCSVFile.Name = "txtCSVFile";
            this.txtCSVFile.Size = new System.Drawing.Size(442, 22);
            this.txtCSVFile.TabIndex = 11;
            // 
            // frmImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1669, 790);
            this.Controls.Add(this.txtCSVFile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmImport";
            this.Text = "frmImport";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgFiletoImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgIndex)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtImportURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnImport;
        private System.Windows.Forms.DataGridView dgFiletoImport;
        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFileName;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColStateCode;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColCountyID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFilmTrackerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBatchNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMonthOfCounty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColYearOfCounty;
        private System.Windows.Forms.DataGridView dgIndex;
        private System.Windows.Forms.Button btnRunIndex;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClmBeforeIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClmAfterIndex;
        private System.Windows.Forms.Button btnLoadIndex;
        private System.Windows.Forms.Button BtnDelete;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFilmTrackerID;
        private Controls.Combobox_Doctype cboxDocType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCSVFile;
    }
}