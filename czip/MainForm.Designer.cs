namespace czip
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tv_main = new System.Windows.Forms.TreeView();
            this.il_icons = new System.Windows.Forms.ImageList(this.components);
            this.ms_main = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gb_info = new System.Windows.Forms.GroupBox();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_unpackSelected = new System.Windows.Forms.Button();
            this.lb_fullpath = new System.Windows.Forms.Label();
            this.lbinfo2 = new System.Windows.Forms.Label();
            this.lbinfo3 = new System.Windows.Forms.Label();
            this.lb_type = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pb_main = new System.Windows.Forms.ProgressBar();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoMonitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ms_main.SuspendLayout();
            this.gb_info.SuspendLayout();
            this.SuspendLayout();
            // 
            // tv_main
            // 
            this.tv_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tv_main.ImageIndex = 0;
            this.tv_main.ImageList = this.il_icons;
            this.tv_main.Location = new System.Drawing.Point(12, 27);
            this.tv_main.Name = "tv_main";
            this.tv_main.SelectedImageIndex = 0;
            this.tv_main.Size = new System.Drawing.Size(360, 382);
            this.tv_main.TabIndex = 0;
            this.tv_main.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tv_main_BeforeCollapse);
            this.tv_main.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tv_main_BeforeExpand);
            this.tv_main.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tv_main_BeforeSelect);
            // 
            // il_icons
            // 
            this.il_icons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il_icons.ImageStream")));
            this.il_icons.TransparentColor = System.Drawing.Color.Transparent;
            this.il_icons.Images.SetKeyName(0, "Folder");
            this.il_icons.Images.SetKeyName(1, "FolderOpen");
            this.il_icons.Images.SetKeyName(2, "TextFile");
            // 
            // ms_main
            // 
            this.ms_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.ms_main.Location = new System.Drawing.Point(0, 0);
            this.ms_main.Name = "ms_main";
            this.ms_main.Size = new System.Drawing.Size(551, 24);
            this.ms_main.TabIndex = 1;
            this.ms_main.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openFolderToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "&Open Czip";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // gb_info
            // 
            this.gb_info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_info.Controls.Add(this.lb_type);
            this.gb_info.Controls.Add(this.lbinfo3);
            this.gb_info.Controls.Add(this.lb_fullpath);
            this.gb_info.Controls.Add(this.lbinfo2);
            this.gb_info.Controls.Add(this.btn_unpackSelected);
            this.gb_info.Location = new System.Drawing.Point(378, 28);
            this.gb_info.Name = "gb_info";
            this.gb_info.Size = new System.Drawing.Size(161, 141);
            this.gb_info.TabIndex = 2;
            this.gb_info.TabStop = false;
            this.gb_info.Text = "gb_info";
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openFolderToolStripMenuItem.Text = "O&pen Folder";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFileToolStripMenuItem,
            this.addFolderToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // addFileToolStripMenuItem
            // 
            this.addFileToolStripMenuItem.Name = "addFileToolStripMenuItem";
            this.addFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addFileToolStripMenuItem.Text = "Add File";
            // 
            // addFolderToolStripMenuItem
            // 
            this.addFolderToolStripMenuItem.Name = "addFolderToolStripMenuItem";
            this.addFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addFolderToolStripMenuItem.Text = "Add Folder";
            // 
            // btn_unpackSelected
            // 
            this.btn_unpackSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_unpackSelected.Location = new System.Drawing.Point(7, 112);
            this.btn_unpackSelected.Name = "btn_unpackSelected";
            this.btn_unpackSelected.Size = new System.Drawing.Size(148, 23);
            this.btn_unpackSelected.TabIndex = 0;
            this.btn_unpackSelected.Text = "&Unpack";
            this.btn_unpackSelected.UseVisualStyleBackColor = true;
            // 
            // lb_fullpath
            // 
            this.lb_fullpath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_fullpath.AutoEllipsis = true;
            this.lb_fullpath.Location = new System.Drawing.Point(8, 21);
            this.lb_fullpath.Margin = new System.Windows.Forms.Padding(5);
            this.lb_fullpath.Name = "lb_fullpath";
            this.lb_fullpath.Size = new System.Drawing.Size(145, 13);
            this.lb_fullpath.TabIndex = 0;
            this.lb_fullpath.Text = "lb_fullpath";
            // 
            // lbinfo2
            // 
            this.lbinfo2.AutoSize = true;
            this.lbinfo2.Location = new System.Drawing.Point(8, 67);
            this.lbinfo2.Margin = new System.Windows.Forms.Padding(5);
            this.lbinfo2.Name = "lbinfo2";
            this.lbinfo2.Size = new System.Drawing.Size(38, 13);
            this.lbinfo2.TabIndex = 1;
            this.lbinfo2.Text = "lbinfo2";
            // 
            // lbinfo3
            // 
            this.lbinfo3.AutoSize = true;
            this.lbinfo3.Location = new System.Drawing.Point(8, 90);
            this.lbinfo3.Margin = new System.Windows.Forms.Padding(5);
            this.lbinfo3.Name = "lbinfo3";
            this.lbinfo3.Size = new System.Drawing.Size(38, 13);
            this.lbinfo3.TabIndex = 2;
            this.lbinfo3.Text = "lbinfo3";
            // 
            // lb_type
            // 
            this.lb_type.AutoSize = true;
            this.lb_type.Location = new System.Drawing.Point(8, 44);
            this.lb_type.Margin = new System.Windows.Forms.Padding(5);
            this.lb_type.Name = "lb_type";
            this.lb_type.Size = new System.Drawing.Size(41, 13);
            this.lb_type.TabIndex = 3;
            this.lb_type.Text = "lb_type";
            // 
            // pb_main
            // 
            this.pb_main.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_main.Location = new System.Drawing.Point(12, 415);
            this.pb_main.Name = "pb_main";
            this.pb_main.Size = new System.Drawing.Size(527, 23);
            this.pb_main.TabIndex = 3;
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoMonitorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // infoMonitorToolStripMenuItem
            // 
            this.infoMonitorToolStripMenuItem.Name = "infoMonitorToolStripMenuItem";
            this.infoMonitorToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.infoMonitorToolStripMenuItem.Text = "&Info Monitor";
            this.infoMonitorToolStripMenuItem.Click += new System.EventHandler(this.infoMonitorToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 450);
            this.Controls.Add(this.pb_main);
            this.Controls.Add(this.gb_info);
            this.Controls.Add(this.tv_main);
            this.Controls.Add(this.ms_main);
            this.MainMenuStrip = this.ms_main;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ms_main.ResumeLayout(false);
            this.ms_main.PerformLayout();
            this.gb_info.ResumeLayout(false);
            this.gb_info.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tv_main;
        private System.Windows.Forms.ImageList il_icons;
        private System.Windows.Forms.MenuStrip ms_main;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.GroupBox gb_info;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFolderToolStripMenuItem;
        private System.Windows.Forms.Button btn_unpackSelected;
        private System.Windows.Forms.Label lbinfo3;
        private System.Windows.Forms.Label lb_fullpath;
        private System.Windows.Forms.Label lbinfo2;
        private System.Windows.Forms.Label lb_type;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ProgressBar pb_main;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoMonitorToolStripMenuItem;
    }
}