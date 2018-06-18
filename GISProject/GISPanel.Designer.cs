namespace MyGIS
{
    partial class GISPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullExtentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layerControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDocumentToolStripMenuItem,
            this.toolStripSeparator1,
            this.selectToolStripMenuItem,
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem,
            this.panToolStripMenuItem,
            this.toolStripSeparator2,
            this.fullExtentToolStripMenuItem,
            this.toolStripSeparator3,
            this.layerControlToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(211, 218);
            // 
            // openDocumentToolStripMenuItem
            // 
            this.openDocumentToolStripMenuItem.Name = "openDocumentToolStripMenuItem";
            this.openDocumentToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.openDocumentToolStripMenuItem.Text = "Open Document";
            this.openDocumentToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem_Click);
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.selectToolStripMenuItem.Text = "Select";
            this.selectToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem_Click);
            // 
            // zoomInToolStripMenuItem
            // 
            this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
            this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.zoomInToolStripMenuItem.Text = "ZoomIn";
            this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem_Click);
            // 
            // zoomOutToolStripMenuItem
            // 
            this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
            this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.zoomOutToolStripMenuItem.Text = "ZoomOut";
            this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem_Click);
            // 
            // panToolStripMenuItem
            // 
            this.panToolStripMenuItem.Name = "panToolStripMenuItem";
            this.panToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.panToolStripMenuItem.Text = "Pan";
            this.panToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem_Click);
            // 
            // fullExtentToolStripMenuItem
            // 
            this.fullExtentToolStripMenuItem.Name = "fullExtentToolStripMenuItem";
            this.fullExtentToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.fullExtentToolStripMenuItem.Text = "Full Extent";
            this.fullExtentToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem_Click);
            // 
            // layerControlToolStripMenuItem
            // 
            this.layerControlToolStripMenuItem.Name = "layerControlToolStripMenuItem";
            this.layerControlToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.layerControlToolStripMenuItem.Text = "Layer Control";
            this.layerControlToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(207, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(207, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(207, 6);
            // 
            // GISPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "GISPanel";
            this.Size = new System.Drawing.Size(736, 413);
            this.SizeChanged += new System.EventHandler(this.GISPanel_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GISPanel_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GISPanel_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GISPanel_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GISPanel_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GISPanel_MouseUp);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openDocumentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem panToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem fullExtentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem layerControlToolStripMenuItem;
    }
}
