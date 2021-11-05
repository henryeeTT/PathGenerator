using System.Drawing;

namespace Final.View {
    partial class MainForm {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_SetRT = new System.Windows.Forms.ToolStripButton();
            this.toolStripReset = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_MeshGrouping = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_Select = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_Path = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLine = new System.Windows.Forms.ToolStripButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.GLView = new Final.View.OpenGLPanel();
            this.xyzwprPanel = new Final.View.XyzwprPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripOpen
            // 
            this.toolStripOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripOpen.Image")));
            this.toolStripOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOpen.Name = "toolStripOpen";
            this.toolStripOpen.Size = new System.Drawing.Size(34, 34);
            this.toolStripOpen.Text = "toolStripButton1";
            this.toolStripOpen.ToolTipText = "Open";
            this.toolStripOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // toolStrip_SetRT
            // 
            this.toolStrip_SetRT.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStrip_SetRT.Image = ((System.Drawing.Image)(resources.GetObject("toolStrip_SetRT.Image")));
            this.toolStrip_SetRT.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_SetRT.Name = "toolStrip_SetRT";
            this.toolStrip_SetRT.Size = new System.Drawing.Size(34, 34);
            this.toolStrip_SetRT.Text = "toolStripButton1";
            this.toolStrip_SetRT.ToolTipText = "Set rotate and translate";
            this.toolStrip_SetRT.Click += new System.EventHandler(this.btnSetRT_Click);
            // 
            // toolStripReset
            // 
            this.toolStripReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripReset.Image = ((System.Drawing.Image)(resources.GetObject("toolStripReset.Image")));
            this.toolStripReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripReset.Name = "toolStripReset";
            this.toolStripReset.Size = new System.Drawing.Size(34, 34);
            this.toolStripReset.Text = "toolStripButton5";
            this.toolStripReset.ToolTipText = "Reset";
            this.toolStripReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // toolStrip_MeshGrouping
            // 
            this.toolStrip_MeshGrouping.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStrip_MeshGrouping.Image = ((System.Drawing.Image)(resources.GetObject("toolStrip_MeshGrouping.Image")));
            this.toolStrip_MeshGrouping.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_MeshGrouping.Name = "toolStrip_MeshGrouping";
            this.toolStrip_MeshGrouping.Size = new System.Drawing.Size(34, 34);
            this.toolStrip_MeshGrouping.Text = "toolStripButton1";
            this.toolStrip_MeshGrouping.ToolTipText = "MeshGrouping";
            this.toolStrip_MeshGrouping.Click += new System.EventHandler(this.btnMeshGrouping_Click);
            // 
            // toolStrip_Select
            // 
            this.toolStrip_Select.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStrip_Select.Image = ((System.Drawing.Image)(resources.GetObject("toolStrip_Select.Image")));
            this.toolStrip_Select.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_Select.Name = "toolStrip_Select";
            this.toolStrip_Select.Size = new System.Drawing.Size(34, 34);
            this.toolStrip_Select.Text = "toolStripButton1";
            this.toolStrip_Select.ToolTipText = "Select";
            this.toolStrip_Select.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // toolStrip_Path
            // 
            this.toolStrip_Path.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStrip_Path.Image = ((System.Drawing.Image)(resources.GetObject("toolStrip_Path.Image")));
            this.toolStrip_Path.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_Path.Name = "toolStrip_Path";
            this.toolStrip_Path.Size = new System.Drawing.Size(34, 34);
            this.toolStrip_Path.Text = "toolStripButton1";
            this.toolStrip_Path.ToolTipText = "Generate Path";
            this.toolStrip_Path.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripOpen,
            this.toolStrip_SetRT,
            this.toolStrip_MeshGrouping,
            this.toolStrip_Select,
            this.toolStripLine,
            this.toolStrip_Path,
            this.toolStripReset});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1064, 37);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLine
            // 
            this.toolStripLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLine.Image = ((System.Drawing.Image)(resources.GetObject("toolStripLine.Image")));
            this.toolStripLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripLine.Name = "toolStripLine";
            this.toolStripLine.Size = new System.Drawing.Size(34, 34);
            this.toolStripLine.Text = "toolStripButton1";
            this.toolStripLine.Click += new System.EventHandler(this.btnLine_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 15;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 37);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.GLView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.xyzwprPanel);
            this.splitContainer1.Panel2MinSize = 300;
            this.splitContainer1.Size = new System.Drawing.Size(1064, 622);
            this.splitContainer1.SplitterDistance = 760;
            this.splitContainer1.TabIndex = 3;
            // 
            // GLView
            // 
            this.GLView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GLView.Location = new System.Drawing.Point(0, 0);
            this.GLView.Name = "GLView";
            this.GLView.Size = new System.Drawing.Size(760, 622);
            this.GLView.TabIndex = 0;
            // 
            // xyzwprPanel
            // 
            this.xyzwprPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xyzwprPanel.Location = new System.Drawing.Point(0, 0);
            this.xyzwprPanel.Name = "xyzwprPanel";
            this.xyzwprPanel.Size = new System.Drawing.Size(300, 622);
            this.xyzwprPanel.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 659);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1064, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(1049, 17);
            this.toolStripStatusLabel.Spring = true;
            this.toolStripStatusLabel.Text = "Status";
            this.toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 681);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.MinimumSize = new System.Drawing.Size(700, 480);
            this.Name = "MainForm";
            this.Text = "Deburr";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripButton toolStripOpen;
        private System.Windows.Forms.ToolStripButton toolStrip_SetRT;
        private System.Windows.Forms.ToolStripButton toolStripReset;
        private System.Windows.Forms.ToolStripButton toolStrip_MeshGrouping;
        private System.Windows.Forms.ToolStripButton toolStrip_Select;
        private System.Windows.Forms.ToolStripButton toolStrip_Path;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Timer timer1;
        private OpenGLPanel GLView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private XyzwprPanel xyzwprPanel;
        private System.Windows.Forms.ToolStripButton toolStripLine;
    }
}

