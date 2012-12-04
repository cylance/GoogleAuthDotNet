namespace Cylance.Research.Tools.GAuthClient
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanelIdentities = new System.Windows.Forms.TableLayoutPanel();
            this.panelScrollable = new System.Windows.Forms.Panel();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.labelNewName = new System.Windows.Forms.Label();
            this.textBoxNewName = new System.Windows.Forms.TextBox();
            this.labelNewSecret = new System.Windows.Forms.Label();
            this.textBoxNewSecret = new System.Windows.Forms.TextBox();
            this.buttonNewAdd = new System.Windows.Forms.Button();
            this.panelScrollable.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelIdentities
            // 
            this.tableLayoutPanelIdentities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelIdentities.AutoSize = true;
            this.tableLayoutPanelIdentities.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelIdentities.ColumnCount = 1;
            this.tableLayoutPanelIdentities.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelIdentities.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelIdentities.Name = "tableLayoutPanelIdentities";
            this.tableLayoutPanelIdentities.RowCount = 1;
            this.tableLayoutPanelIdentities.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelIdentities.Size = new System.Drawing.Size(0, 0);
            this.tableLayoutPanelIdentities.TabIndex = 1;
            // 
            // panelScrollable
            // 
            this.panelScrollable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelScrollable.AutoScroll = true;
            this.panelScrollable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelScrollable.Controls.Add(this.tableLayoutPanelIdentities);
            this.panelScrollable.Location = new System.Drawing.Point(12, 12);
            this.panelScrollable.Name = "panelScrollable";
            this.panelScrollable.Size = new System.Drawing.Size(327, 286);
            this.panelScrollable.TabIndex = 0;
            // 
            // timerUpdate
            // 
            this.timerUpdate.Enabled = true;
            this.timerUpdate.Interval = 500;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // labelNewName
            // 
            this.labelNewName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelNewName.AutoSize = true;
            this.labelNewName.Location = new System.Drawing.Point(12, 314);
            this.labelNewName.Name = "labelNewName";
            this.labelNewName.Size = new System.Drawing.Size(38, 13);
            this.labelNewName.TabIndex = 1;
            this.labelNewName.Text = "&Name:";
            // 
            // textBoxNewName
            // 
            this.textBoxNewName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNewName.Location = new System.Drawing.Point(59, 312);
            this.textBoxNewName.MaxLength = 127;
            this.textBoxNewName.Name = "textBoxNewName";
            this.textBoxNewName.Size = new System.Drawing.Size(279, 20);
            this.textBoxNewName.TabIndex = 2;
            // 
            // labelNewSecret
            // 
            this.labelNewSecret.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelNewSecret.AutoSize = true;
            this.labelNewSecret.Location = new System.Drawing.Point(12, 350);
            this.labelNewSecret.Name = "labelNewSecret";
            this.labelNewSecret.Size = new System.Drawing.Size(41, 13);
            this.labelNewSecret.TabIndex = 3;
            this.labelNewSecret.Text = "&Secret:";
            // 
            // textBoxNewSecret
            // 
            this.textBoxNewSecret.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNewSecret.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxNewSecret.Location = new System.Drawing.Point(59, 347);
            this.textBoxNewSecret.MaxLength = 32;
            this.textBoxNewSecret.Name = "textBoxNewSecret";
            this.textBoxNewSecret.Size = new System.Drawing.Size(212, 20);
            this.textBoxNewSecret.TabIndex = 4;
            this.textBoxNewSecret.UseSystemPasswordChar = true;
            // 
            // buttonNewAdd
            // 
            this.buttonNewAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNewAdd.Location = new System.Drawing.Point(288, 342);
            this.buttonNewAdd.Name = "buttonNewAdd";
            this.buttonNewAdd.Size = new System.Drawing.Size(50, 29);
            this.buttonNewAdd.TabIndex = 5;
            this.buttonNewAdd.Text = "&Add";
            this.buttonNewAdd.UseVisualStyleBackColor = true;
            this.buttonNewAdd.Click += new System.EventHandler(this.buttonNewAdd_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 381);
            this.Controls.Add(this.buttonNewAdd);
            this.Controls.Add(this.textBoxNewSecret);
            this.Controls.Add(this.labelNewSecret);
            this.Controls.Add(this.textBoxNewName);
            this.Controls.Add(this.labelNewName);
            this.Controls.Add(this.panelScrollable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Google Authenticator by Cylance";
            this.panelScrollable.ResumeLayout(false);
            this.panelScrollable.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelIdentities;
        private System.Windows.Forms.Panel panelScrollable;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.Label labelNewName;
        private System.Windows.Forms.TextBox textBoxNewName;
        private System.Windows.Forms.Label labelNewSecret;
        private System.Windows.Forms.TextBox textBoxNewSecret;
        private System.Windows.Forms.Button buttonNewAdd;


    }
}
