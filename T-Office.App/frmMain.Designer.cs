namespace T_Office.App
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mainPanel = new System.Windows.Forms.Panel();
            this.btnSettings = new Bunifu.Framework.UI.BunifuFlatButton();
            this.btnReports = new Bunifu.Framework.UI.BunifuFlatButton();
            this.btnClients = new Bunifu.Framework.UI.BunifuFlatButton();
            this.btnHome = new Bunifu.Framework.UI.BunifuFlatButton();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.controlBoxPanel = new System.Windows.Forms.Panel();
            this.btnMinimize = new Bunifu.Framework.UI.BunifuImageButton();
            this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.lblMainHeader = new System.Windows.Forms.Label();
            this.pageClients = new T_Office.App.pages.clients.ucClients();
            this.pageMessages = new T_Office.App.pages.messages.ucMessages();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.controlBoxPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.mainPanel.CausesValidation = false;
            this.mainPanel.Controls.Add(this.btnSettings);
            this.mainPanel.Controls.Add(this.btnReports);
            this.mainPanel.Controls.Add(this.btnClients);
            this.mainPanel.Controls.Add(this.btnHome);
            this.mainPanel.Controls.Add(this.pbLogo);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(288, 768);
            this.mainPanel.TabIndex = 0;
            // 
            // btnSettings
            // 
            this.btnSettings.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSettings.BorderRadius = 0;
            this.btnSettings.ButtonText = "   Podešavanja";
            this.btnSettings.CausesValidation = false;
            this.btnSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSettings.DisabledColor = System.Drawing.Color.Gray;
            this.btnSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(240)))), ((int)(((byte)(17)))));
            this.btnSettings.Iconcolor = System.Drawing.Color.Transparent;
            this.btnSettings.Iconimage = ((System.Drawing.Image)(resources.GetObject("btnSettings.Iconimage")));
            this.btnSettings.Iconimage_right = null;
            this.btnSettings.Iconimage_right_Selected = null;
            this.btnSettings.Iconimage_Selected = null;
            this.btnSettings.IconMarginLeft = 0;
            this.btnSettings.IconMarginRight = 0;
            this.btnSettings.IconRightVisible = true;
            this.btnSettings.IconRightZoom = 0D;
            this.btnSettings.IconVisible = true;
            this.btnSettings.IconZoom = 50D;
            this.btnSettings.IsTab = false;
            this.btnSettings.Location = new System.Drawing.Point(0, 341);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.btnSettings.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.btnSettings.OnHoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(240)))), ((int)(((byte)(17)))));
            this.btnSettings.selected = false;
            this.btnSettings.Size = new System.Drawing.Size(288, 48);
            this.btnSettings.TabIndex = 3;
            this.btnSettings.Text = "   Podešavanja";
            this.btnSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettings.Textcolor = System.Drawing.Color.White;
            this.btnSettings.TextFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // btnReports
            // 
            this.btnReports.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.btnReports.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReports.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.btnReports.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReports.BorderRadius = 0;
            this.btnReports.ButtonText = "   Izveštaji";
            this.btnReports.CausesValidation = false;
            this.btnReports.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReports.DisabledColor = System.Drawing.Color.Gray;
            this.btnReports.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(240)))), ((int)(((byte)(17)))));
            this.btnReports.Iconcolor = System.Drawing.Color.Transparent;
            this.btnReports.Iconimage = ((System.Drawing.Image)(resources.GetObject("btnReports.Iconimage")));
            this.btnReports.Iconimage_right = null;
            this.btnReports.Iconimage_right_Selected = null;
            this.btnReports.Iconimage_Selected = null;
            this.btnReports.IconMarginLeft = 0;
            this.btnReports.IconMarginRight = 0;
            this.btnReports.IconRightVisible = true;
            this.btnReports.IconRightZoom = 0D;
            this.btnReports.IconVisible = true;
            this.btnReports.IconZoom = 50D;
            this.btnReports.IsTab = false;
            this.btnReports.Location = new System.Drawing.Point(0, 287);
            this.btnReports.Name = "btnReports";
            this.btnReports.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.btnReports.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.btnReports.OnHoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(240)))), ((int)(((byte)(17)))));
            this.btnReports.selected = false;
            this.btnReports.Size = new System.Drawing.Size(288, 48);
            this.btnReports.TabIndex = 2;
            this.btnReports.Text = "   Izveštaji";
            this.btnReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReports.Textcolor = System.Drawing.Color.White;
            this.btnReports.TextFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // btnClients
            // 
            this.btnClients.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.btnClients.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClients.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.btnClients.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClients.BorderRadius = 0;
            this.btnClients.ButtonText = "   Klijenti";
            this.btnClients.CausesValidation = false;
            this.btnClients.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClients.DisabledColor = System.Drawing.Color.Gray;
            this.btnClients.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(240)))), ((int)(((byte)(17)))));
            this.btnClients.Iconcolor = System.Drawing.Color.Transparent;
            this.btnClients.Iconimage = ((System.Drawing.Image)(resources.GetObject("btnClients.Iconimage")));
            this.btnClients.Iconimage_right = null;
            this.btnClients.Iconimage_right_Selected = null;
            this.btnClients.Iconimage_Selected = null;
            this.btnClients.IconMarginLeft = 0;
            this.btnClients.IconMarginRight = 0;
            this.btnClients.IconRightVisible = true;
            this.btnClients.IconRightZoom = 0D;
            this.btnClients.IconVisible = true;
            this.btnClients.IconZoom = 50D;
            this.btnClients.IsTab = false;
            this.btnClients.Location = new System.Drawing.Point(0, 233);
            this.btnClients.Name = "btnClients";
            this.btnClients.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.btnClients.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.btnClients.OnHoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(240)))), ((int)(((byte)(17)))));
            this.btnClients.selected = false;
            this.btnClients.Size = new System.Drawing.Size(288, 48);
            this.btnClients.TabIndex = 2;
            this.btnClients.Text = "   Klijenti";
            this.btnClients.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClients.Textcolor = System.Drawing.Color.White;
            this.btnClients.TextFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClients.Click += new System.EventHandler(this.btnClients_Click);
            // 
            // btnHome
            // 
            this.btnHome.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.btnHome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.btnHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnHome.BorderRadius = 0;
            this.btnHome.ButtonText = "   Poruke";
            this.btnHome.CausesValidation = false;
            this.btnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHome.DisabledColor = System.Drawing.Color.Gray;
            this.btnHome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(240)))), ((int)(((byte)(17)))));
            this.btnHome.Iconcolor = System.Drawing.Color.Transparent;
            this.btnHome.Iconimage = ((System.Drawing.Image)(resources.GetObject("btnHome.Iconimage")));
            this.btnHome.Iconimage_right = null;
            this.btnHome.Iconimage_right_Selected = null;
            this.btnHome.Iconimage_Selected = null;
            this.btnHome.IconMarginLeft = 0;
            this.btnHome.IconMarginRight = 0;
            this.btnHome.IconRightVisible = true;
            this.btnHome.IconRightZoom = 0D;
            this.btnHome.IconVisible = true;
            this.btnHome.IconZoom = 50D;
            this.btnHome.IsTab = false;
            this.btnHome.Location = new System.Drawing.Point(0, 179);
            this.btnHome.Name = "btnHome";
            this.btnHome.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.btnHome.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.btnHome.OnHoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(240)))), ((int)(((byte)(17)))));
            this.btnHome.selected = false;
            this.btnHome.Size = new System.Drawing.Size(288, 48);
            this.btnHome.TabIndex = 1;
            this.btnHome.Text = "   Poruke";
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHome.Textcolor = System.Drawing.Color.White;
            this.btnHome.TextFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // pbLogo
            // 
            this.pbLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbLogo.Image")));
            this.pbLogo.Location = new System.Drawing.Point(44, 12);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(202, 105);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 0;
            this.pbLogo.TabStop = false;
            // 
            // controlBoxPanel
            // 
            this.controlBoxPanel.BackColor = System.Drawing.Color.DimGray;
            this.controlBoxPanel.Controls.Add(this.lblMainHeader);
            this.controlBoxPanel.Controls.Add(this.btnMinimize);
            this.controlBoxPanel.Controls.Add(this.btnClose);
            this.controlBoxPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlBoxPanel.Location = new System.Drawing.Point(288, 0);
            this.controlBoxPanel.Name = "controlBoxPanel";
            this.controlBoxPanel.Size = new System.Drawing.Size(1078, 55);
            this.controlBoxPanel.TabIndex = 1;
            this.controlBoxPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlBoxPanel_MouseDown);
            this.controlBoxPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlBoxPanel_MouseMove);
            this.controlBoxPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlBoxPanel_MouseUp);
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.DimGray;
            this.btnMinimize.Image = ((System.Drawing.Image)(resources.GetObject("btnMinimize.Image")));
            this.btnMinimize.ImageActive = null;
            this.btnMinimize.Location = new System.Drawing.Point(1014, 1);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(23, 47);
            this.btnMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnMinimize.TabIndex = 10;
            this.btnMinimize.TabStop = false;
            this.btnMinimize.Zoom = 10;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.DimGray;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageActive = null;
            this.btnClose.Location = new System.Drawing.Point(1043, 13);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(23, 23);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.TabIndex = 8;
            this.btnClose.TabStop = false;
            this.btnClose.Zoom = 10;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblMainHeader
            // 
            this.lblMainHeader.AutoSize = true;
            this.lblMainHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblMainHeader.ForeColor = System.Drawing.Color.White;
            this.lblMainHeader.Location = new System.Drawing.Point(6, 12);
            this.lblMainHeader.Name = "lblMainHeader";
            this.lblMainHeader.Size = new System.Drawing.Size(80, 25);
            this.lblMainHeader.TabIndex = 4;
            this.lblMainHeader.Text = "T-Office";
            // 
            // pageClients
            // 
            this.pageClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageClients.Location = new System.Drawing.Point(288, 55);
            this.pageClients.Name = "pageClients";
            this.pageClients.Size = new System.Drawing.Size(1078, 713);
            this.pageClients.TabIndex = 3;
            // 
            // pageMessages
            // 
            this.pageMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageMessages.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.pageMessages.Location = new System.Drawing.Point(288, 55);
            this.pageMessages.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pageMessages.Name = "pageMessages";
            this.pageMessages.Size = new System.Drawing.Size(1078, 713);
            this.pageMessages.TabIndex = 2;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.pageClients);
            this.Controls.Add(this.pageMessages);
            this.Controls.Add(this.controlBoxPanel);
            this.Controls.Add(this.mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "T-Office";
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.controlBoxPanel.ResumeLayout(false);
            this.controlBoxPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Panel controlBoxPanel;
        private Bunifu.Framework.UI.BunifuImageButton btnClose;
        private Bunifu.Framework.UI.BunifuImageButton btnMinimize;
        private Bunifu.Framework.UI.BunifuFlatButton btnHome;
        private Bunifu.Framework.UI.BunifuFlatButton btnClients;
        private Bunifu.Framework.UI.BunifuFlatButton btnSettings;
        private Bunifu.Framework.UI.BunifuFlatButton btnReports;
        private pages.messages.ucMessages pageMessages;
        private pages.clients.ucClients pageClients;
        private System.Windows.Forms.Label lblMainHeader;
    }
}