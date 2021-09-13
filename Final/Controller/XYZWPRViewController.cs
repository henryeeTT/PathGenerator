//using Final.Model;
//using OpenTK.Graphics.ES11;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Security.Permissions;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using Tao.OpenGl;

//namespace Final.Controller {
//    class XYZWPRViewController {
//        DataGridView View;
//        ToolStripComboBox Box;
//        ToolStripButton Save;
//        ToolStripButton Delete; 
//        ToolStripButton Export;
//        List<List<XYZWPR>> Sources = new List<List<XYZWPR>>();
//        BindingSource tmpSource;
//        int cnt = 0;
//        int index = 0;

//        public XYZWPRViewController (DataGridView view, ToolStripComboBox box, ToolStripButton save, ToolStripButton delete, ToolStripButton export) {
//            View = view;
//            Box = box;
//            Save = save;
//            Delete = delete;
//            Export = export;
//            Save.Click += Save_Click;
//            Delete.Click += Delete_Click;
//            Export.Click += Export_Click; 
//            box.SelectedIndexChanged += Box_SelectedIndexChanged;
//            view.SelectionChanged += View_SelectionChanged;
//        }

//        private void Export_Click (object sender, EventArgs e) {
//            List<XYZWPR> tmp = new List<XYZWPR>();
//            foreach (var p in Sources)
//                tmp.AddRange(p);
//            ExportTool ET = new ExportTool(tmp);
//            ET.Export();
//        }

//        public void RefreshSource (BindingSource source) {
//            tmpSource = source;
//            View.DataSource = source;
//        }

//        public List<XYZWPR> GetAllPoint () {
//            List<XYZWPR> tmp = new List<XYZWPR>();
//            foreach (var p in Sources) 
//                tmp.AddRange(p);
//            return tmp;
//        }

//        private void View_SelectionChanged (object sender, EventArgs e) {
//            //if (true) {
//            //    var tmp = View.CurrentRow.Cells;
//            //    Gl.glNewList((int)GlList.GridBall, Gl.GL_COMPILE);
//            //    Gl.glPushMatrix();
//            //    Gl.glColor3d(0.3, 0.3, 0.3);
//            //    //Gl.glTranslated(tmp.X, tmp.Y, tmp.Z - 2);
//            //    //Glu.gluCylinder(Glu.gluNewQuadric(), 0, 2, 2, 20, 20);
//            //    //Gl.glTranslated(0, 0, 2);
//            //    //Glu.gluCylinder(Glu.gluNewQuadric(), 2, 2, 15, 20, 20);
//            //    Gl.glTranslated(float.Parse(tmp[3].ToString()), float.Parse(tmp[4].ToString()), float.Parse(tmp[5].ToString()));
//            //    Glu.gluSphere(Glu.gluNewQuadric(), 3, 20, 20);
//            //    Gl.glPopMatrix();
//            //    Gl.glEndList();
//            //}
//        }

//        private void Box_SelectedIndexChanged (object sender, EventArgs e) {
//            index = Box.SelectedIndex;
//            View.DataSource = Sources[index];
//            Gl.glNewList((int)GlList.Path, Gl.GL_COMPILE);
//            Gl.glColor3ub(200, 200, 200);
//            foreach (var p in Sources[index]) {
//                Gl.glPushMatrix();
//                Gl.glTranslated(p.X, p.Y, p.Z);
//                Glu.gluSphere(Glu.gluNewQuadric(), 2, 10, 10);
//                Gl.glPopMatrix();
//            }
//            Gl.glEndList();
//        }

//        private void Delete_Click (object sender, EventArgs e) {
//            Sources.RemoveAt(index);
//            Box.Items.Remove("Path" + cnt);
//        }

//        private void Save_Click (object sender, EventArgs e) {
//            BindingSource source = (BindingSource)View.DataSource;
//            XYZWPR[] Path = new XYZWPR[source.Count];
//            source.CopyTo(Path, 0);
//            Sources.Add(Path.ToList());
//            Box.Items.Add("Path" + cnt);
//            cnt++;
//            MessageBox.Show("done");
//        }

//    }
//}
