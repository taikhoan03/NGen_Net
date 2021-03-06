﻿namespace NexGen
{
    partial class FormApp
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.manageDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importPackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packagePriorityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.managePackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packageAssignmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageDataToolStripMenuItem,
            this.workingToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(869, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // manageDataToolStripMenuItem
            // 
            this.manageDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importPackageToolStripMenuItem,
            this.packagePriorityToolStripMenuItem,
            this.managePackageToolStripMenuItem,
            this.packageAssignmentToolStripMenuItem});
            this.manageDataToolStripMenuItem.Name = "manageDataToolStripMenuItem";
            this.manageDataToolStripMenuItem.Size = new System.Drawing.Size(111, 24);
            this.manageDataToolStripMenuItem.Text = "Manage Data";
            // 
            // importPackageToolStripMenuItem
            // 
            this.importPackageToolStripMenuItem.Name = "importPackageToolStripMenuItem";
            this.importPackageToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.importPackageToolStripMenuItem.Text = "Import package";
            this.importPackageToolStripMenuItem.Click += new System.EventHandler(this.importPackageToolStripMenuItem_Click);
            // 
            // packagePriorityToolStripMenuItem
            // 
            this.packagePriorityToolStripMenuItem.Name = "packagePriorityToolStripMenuItem";
            this.packagePriorityToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.packagePriorityToolStripMenuItem.Text = "Package priority";
            this.packagePriorityToolStripMenuItem.Click += new System.EventHandler(this.packagePriorityToolStripMenuItem_Click);
            // 
            // managePackageToolStripMenuItem
            // 
            this.managePackageToolStripMenuItem.Name = "managePackageToolStripMenuItem";
            this.managePackageToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.managePackageToolStripMenuItem.Text = "Manage package";
            this.managePackageToolStripMenuItem.Click += new System.EventHandler(this.managePackageToolStripMenuItem_Click);
            // 
            // workingToolStripMenuItem
            // 
            this.workingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keyToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.workingToolStripMenuItem.Name = "workingToolStripMenuItem";
            this.workingToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.workingToolStripMenuItem.Text = "Working";
            // 
            // keyToolStripMenuItem
            // 
            this.keyToolStripMenuItem.Name = "keyToolStripMenuItem";
            this.keyToolStripMenuItem.Size = new System.Drawing.Size(127, 26);
            this.keyToolStripMenuItem.Text = "Key";
            this.keyToolStripMenuItem.Click += new System.EventHandler(this.keyToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(127, 26);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // packageAssignmentToolStripMenuItem
            // 
            this.packageAssignmentToolStripMenuItem.Name = "packageAssignmentToolStripMenuItem";
            this.packageAssignmentToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.packageAssignmentToolStripMenuItem.Text = "Package Assignment";
            this.packageAssignmentToolStripMenuItem.Click += new System.EventHandler(this.packageAssignmentToolStripMenuItem_Click);
            // 
            // FormApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 546);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormApp";
            this.Text = "FormApp";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormApp_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormApp_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem manageDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importPackageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packagePriorityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem managePackageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packageAssignmentToolStripMenuItem;
    }
}