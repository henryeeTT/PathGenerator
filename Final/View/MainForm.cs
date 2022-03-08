using PathGenerator.Controller;
using PathGenerator.Model;
using System;
using System.Windows.Forms;
using Tao.OpenGl;
using OpenTK;

namespace PathGenerator.View {
    public partial class MainForm : Form {
        STLPathController Workpiece;
        bool SelectMode, LineMode, isSorted;

        public MainForm () {
            InitializeComponent();
            GLView.SelectEvent = SelectEvent;
            Workpiece = new STLPathController((int)GlList.Workpiece, (int)GlList.Path);
            Workpiece.ReadASCIIFile(@"C:\Users\henry.tsai\Desktop\公司\完成\DemoRoom\demo2\demo.stl");
            //Workpiece.ReadBinaryFile("487.stl");
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
            if (LineMode == true) {
                byte[] color = new byte[3];
                Gl.glDisable(Gl.GL_LIGHTING);
                Workpiece.MakeColorList();
                Workpiece.ClearPathList();
                GLView.Render();
                Workpiece.MakeLineList(x, GLView.Height - y);
                Gl.glEnable(Gl.GL_LIGHTING);
                Workpiece.MakeSortedList();
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
            LineMode = !LineMode;
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
            if(SelectMode)
            if (Dialog.InputBox("Set rotate angle", "Angle in degree:", ref value) == DialogResult.OK) {
                Workpiece.MakePathList(float.Parse(value));
                xyzwprPanel.RefreshSource(Workpiece.MakeFaceBindingSource());
            }
            if (LineMode) {
                xyzwprPanel.RefreshSource(Workpiece.MakeLineBindingSource());
            }
        }

        private void btnLine_Click (object sender, EventArgs e) {
            SelectMode = !SelectMode;
            LineMode = !LineMode;
            btnStatusHandler();
        }

        private void btnStatusHandler () {
            if (isSorted) {
                toolStripStatusLabel.Text = "Finish";
                toolStrip_Path.Enabled = true;
                if (!SelectMode) {
                    SelectMode = false;
                    LineMode = true;
                    toolStrip_Select.Checked = false;
                    toolStripLine.Checked = true;
                    toolStrip_Select.Enabled = false;
                    toolStripLine.Enabled = true;
                    toolStripStatusLabel.Text = "LineMode";
                }
                else {
                    SelectMode = true;
                    LineMode = false;
                    toolStrip_Select.Checked = true;
                    toolStripLine.Checked = false;
                    toolStrip_Select.Enabled = true;
                    toolStripLine.Enabled = false;
                    toolStripStatusLabel.Text = "Select mode";
                }
            }
            else {
                toolStrip_Select.Enabled = false;
                toolStripLine.Enabled = false;
                toolStrip_Path.Enabled = false;
                toolStripStatusLabel.Text = "";
            }

        }
    }
}
