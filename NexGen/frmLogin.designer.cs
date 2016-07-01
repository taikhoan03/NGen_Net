namespace NexGen
{
    partial class frmLogin
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.lblLoginMessageResult = new System.Windows.Forms.Label();
            this.lblValiddateUsername = new System.Windows.Forms.Label();
            this.lblValidatePassword = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            //((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.txtPass.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(169, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Login System";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(37, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(45, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password:";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(118, 174);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(118, 82);
            this.txtUser.Name = "txtUser";
            //this.txtUser.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.txtUser.Size = new System.Drawing.Size(200, 20);
            this.txtUser.TabIndex = 5;
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(117, 116);
            this.txtPass.Name = "txtPass";
            //this.txtPass.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(200, 20);
            this.txtPass.TabIndex = 6;
            // 
            // lblLoginMessageResult
            // 
            this.lblLoginMessageResult.AutoSize = true;
            this.lblLoginMessageResult.ForeColor = System.Drawing.Color.Red;
            this.lblLoginMessageResult.Location = new System.Drawing.Point(115, 148);
            this.lblLoginMessageResult.Name = "lblLoginMessageResult";
            this.lblLoginMessageResult.Size = new System.Drawing.Size(116, 13);
            this.lblLoginMessageResult.TabIndex = 8;
            this.lblLoginMessageResult.Text = "lblLoginMessageResult";
            this.lblLoginMessageResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValiddateUsername
            // 
            this.lblValiddateUsername.AutoSize = true;
            this.lblValiddateUsername.ForeColor = System.Drawing.Color.Red;
            this.lblValiddateUsername.Location = new System.Drawing.Point(325, 87);
            this.lblValiddateUsername.Name = "lblValiddateUsername";
            this.lblValiddateUsername.Size = new System.Drawing.Size(109, 13);
            this.lblValiddateUsername.TabIndex = 9;
            this.lblValiddateUsername.Text = "lblValiddateUsername";
            // 
            // lblValidatePassword
            // 
            this.lblValidatePassword.AutoSize = true;
            this.lblValidatePassword.ForeColor = System.Drawing.Color.Red;
            this.lblValidatePassword.Location = new System.Drawing.Point(324, 119);
            this.lblValidatePassword.Name = "lblValidatePassword";
            this.lblValidatePassword.Size = new System.Drawing.Size(101, 13);
            this.lblValidatePassword.TabIndex = 10;
            this.lblValidatePassword.Text = "lblValidatePassword";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(206, 174);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 253);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblValidatePassword);
            this.Controls.Add(this.lblValiddateUsername);
            this.Controls.Add(this.lblLoginMessageResult);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLogin_FormClosed);
            //((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).EndInit();
            //((System.ComponentModel.ISupportInitialize)(this.txtPass.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Label lblLoginMessageResult;
        private System.Windows.Forms.Label lblValiddateUsername;
        private System.Windows.Forms.Label lblValidatePassword;
        private System.Windows.Forms.Button btnCancel;
    }
}