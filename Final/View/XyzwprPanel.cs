using Final.Controller;
using Final.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tao.OpenGl;

namespace Final.View {
    public partial class XyzwprPanel : UserControl {
        List<List<XYZABC>> Sources = new List<List<XYZABC>>();
        BindingSource tmpSource;
        int cnt = 0;
        int index = 0;

        public XyzwprPanel () {
            InitializeComponent();
        }

        public void RefreshSource (BindingSource source) {
            tmpSource = source;
            GridView.DataSource = source;
        }

        private void BtnSave_Click (object sender, EventArgs e) {
            if (GridView.DataSource != null) {
                BindingSource source = (BindingSource)GridView.DataSource;
                XYZABC[] Path = new XYZABC[source.Count];
                source.CopyTo(Path, 0);
                Sources.Add(Path.ToList());
                CboPath.Items.Add("Path" + cnt);
                CboPath.SelectedIndex = cnt;
                cnt++;
            }
        }

        private void BtnDelete_Click (object sender, EventArgs e) {
            try {
                Sources.RemoveAt(index);
                CboPath.Items.Remove("Path" + index);
            }
            catch (Exception) {
            }
        }

        private void BtnExport_Click (object sender, EventArgs e) {
            List<XYZABC> tmp = new List<XYZABC>();
            foreach (var p in Sources)
                tmp.AddRange(p);
            ExportTool ET = new ExportTool(tmp);
            ET.ExportTM();
        }

        private void CboPath_SelectedIndexChanged (object sender, EventArgs e) {
            index = CboPath.SelectedIndex;
            GridView.DataSource = Sources[index];
            Gl.glNewList((int)GlList.Path, Gl.GL_COMPILE);
            Gl.glColor3ub(200, 200, 200);
            foreach (var p in Sources[index]) {
                Gl.glPushMatrix();
                Gl.glTranslated(p.X, p.Y, p.Z);
                Glu.gluSphere(Glu.gluNewQuadric(), 2, 10, 10);
                Gl.glPopMatrix();
            }
            Gl.glEndList();
        }

        private void GridView_SelectionChanged (object sender, EventArgs e) {
            var current = GridView.CurrentRow.Cells;
            Gl.glNewList((int)GlList.GridBall, Gl.GL_COMPILE);
            Gl.glPushMatrix();
            Gl.glColor3d(0.8, 0.8, 1);
            Gl.glTranslated(
                float.Parse(current[0].Value.ToString()),
                float.Parse(current[1].Value.ToString()),
                float.Parse(current[2].Value.ToString()));
            Glu.gluSphere(Glu.gluNewQuadric(), 3, 20, 20);
            Gl.glPopMatrix();
            Gl.glEndList();
        }
    }
}
