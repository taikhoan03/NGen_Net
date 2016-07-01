namespace NexGen
{
    partial class frmImportProccessing
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblNoteCurrentIndex = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblMessageFail = new System.Windows.Forms.Label();
            this.lblOCR = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Proccessing";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(112, 13);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "label2";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(64, 48);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(559, 21);
            this.progressBar1.TabIndex = 2;
            // 
            // lblNoteCurrentIndex
            // 
            this.lblNoteCurrentIndex.AutoSize = true;
            this.lblNoteCurrentIndex.Location = new System.Drawing.Point(264, 81);
            this.lblNoteCurrentIndex.Name = "lblNoteCurrentIndex";
            this.lblNoteCurrentIndex.Size = new System.Drawing.Size(35, 13);
            this.lblNoteCurrentIndex.TabIndex = 3;
            this.lblNoteCurrentIndex.Text = "label2";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(279, 97);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblMessageFail
            // 
            this.lblMessageFail.AutoSize = true;
            this.lblMessageFail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessageFail.ForeColor = System.Drawing.Color.Red;
            this.lblMessageFail.Location = new System.Drawing.Point(363, 81);
            this.lblMessageFail.Name = "lblMessageFail";
            this.lblMessageFail.Size = new System.Drawing.Size(35, 13);
            this.lblMessageFail.TabIndex = 5;
            this.lblMessageFail.Text = "label2";
            // 
            // lblOCR
            // 
            this.lblOCR.AutoSize = true;
            this.lblOCR.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOCR.ForeColor = System.Drawing.Color.Red;
            this.lblOCR.Location = new System.Drawing.Point(482, 11);
            this.lblOCR.Name = "lblOCR";
            this.lblOCR.Size = new System.Drawing.Size(52, 17);
            this.lblOCR.TabIndex = 6;
            this.lblOCR.Text = "label2";
            // 
            // frmImportProccessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 138);
            this.Controls.Add(this.lblOCR);
            this.Controls.Add(this.lblMessageFail);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblNoteCurrentIndex);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.label1);
            this.Location = new System.Drawing.Point(200, 400);
            this.Name = "frmImportProccessing";
            this.Text = "frmImportProccessing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImportProccessing_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblNoteCurrentIndex;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblMessageFail;
        private System.Windows.Forms.Label lblOCR;
    }
}