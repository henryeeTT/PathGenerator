using Final.Controller;
using Final.Model;
using System;
using System.Windows.Forms;
using Tao.OpenGl;
using OpenTK;

namespace Final.View {
    public partial class MainForm : Form {
        STLPathController Workpiece;
        bool SelectMode, isSorted;

        public MainForm () {
            InitializeComponent();
            GLView.SelectEvent = SelectEvent;

            Workpiece = new STLPathController((int)GlList.Workpiece, (int)GlList.Path);
            Workpiece.ReadASCIIFile(@"C:\Users\henry.tsai\Desktop\公司\完成\DemoRoom\demo2\demo.stl");
            Workpiece.SetOriginalPosition();
            Workpiece.MakeOriginalList();
            btnStatusHandler();
        }

        private void SelectEvent (int x, int y) {
            if (SelectMode == true) {
                byte[] color = new byte[3];
                Gl.glDisable(Gl.GL_LIGHTING);
                Workpiece.MakeSortedColorList();
                GLView.Render();
                Gl.glReadPixels(x, GLView.Height - y, 1, 1, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, color);
                Gl.glEnable(Gl.GL_LIGHTING);
                Workpiece.MakeSortedList();
                Workpiece.MakeUnsortPathList(color);
                GLView.Render();
                GLView.SwapBuffer();
            }
        }

        private void timer1_Tick (object sender, EventArgs e) {
            GLView.Render();
            GLView.SwapBuffer();
        }

        private void btnOpen_Click (object sender, EventArgs e) {
            if (Workpiece.OpenFile()) {
                xyzwprPanel.RefreshSource(new BindingSource());
                Workpiece.ResetPath();
                Workpiece.MakeOriginalList();
                isSorted = false;
                SelectMode = false;
                btnStatusHandler();
            }
        }

        private void btnReset_Click (object sender, EventArgs e) {
            if (Workpiece.Sorted) {
                Workpiece.ResetPath();
                Workpiece.ResetMeshGroup();
                Workpiece.MakeSortedList();
            }
        }

        private void btnSelect_Click (object sender, EventArgs e) {
            SelectMode = !SelectMode;
            btnStatusHandler();
        }

        private void btnMeshGrouping_Click (object sender, EventArgs e) {
            string value = "5";
            if (Dialog.InputBox("Set boundary angle", "Angle in degree:", ref value) == DialogResult.OK) {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Reset();
                sw.Start();
                toolStripStatusLabel.Text = "Calculating....";

                Workpiece.MeshGrouping(float.Parse(value) / 180 * (float)Math.PI, 1);
                Workpiece.MakeSortedList();
                isSorted = true;
                toolStripStatusLabel.Text = "Finished";

                sw.Stop();
                MessageBox.Show("Time Spent : " + sw.Elapsed.TotalSeconds.ToString("f2") + " sec");
                btnStatusHandler();
            }
        }

        private void btnSetRT_Click (object sender, EventArgs e) {
            new SetPositionBox(Workpiece).Show();
        }

        private void btnPath_Click (object sender, EventArgs e) {
            string value = "0";
            if (Dialog.InputBox("Set rotate angle", "Angle in degree:", ref value) == DialogResult.OK) {
                Workpiece.MakePathList(float.Parse(value));
                xyzwprPanel.RefreshSource(Workpiece.MakeBindingSource());
            }
        }

        private void btnStatusHandler () {
            if (isSorted) {
                toolStrip_Select.Enabled = true;
                toolStrip_Path.Enabled = true;
                toolStripStatusLabel.Text = "Finish";
            }
            else {
                toolStrip_Select.Enabled = false;
                toolStrip_Path.Enabled = false;
                toolStripStatusLabel.Text = "";
            }
            if (!SelectMode) {
                SelectMode = false;
                toolStrip_Select.Checked = false;
                toolStripStatusLabel.Text = "Idle";
            }
            else {
                SelectMode = true;
                toolStrip_Select.Checked = true;
                toolStripStatusLabel.Text = "Select mode";
            }
        }
    }
}
