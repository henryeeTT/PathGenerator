using Final.Model;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tao.OpenGl;

namespace Final.Controller {
    public class STLPathController : STLController {
        public int PathNum;
        public MyLine[] Path;
        private HashSet<MyLine> Vectors = new HashSet<MyLine>();
        private List<MeshGroup> SelectedGroup = new List<MeshGroup>();

        public STLPathController (int ListNum, int PathNum) : base(ListNum) {
            this.ListNum = ListNum;
            this.PathNum = PathNum;
        }

        public void ResetPath () {
            SelectedGroup.Clear();
            Vectors.Clear();
            Gl.glNewList(PathNum, Gl.GL_COMPILE);
            Gl.glEndList();
        }

        public void MakeUnsortPathList (byte[] color) {
            int ID = 0;
            bool hit = false;

            //-----------search object by color
            foreach (var face in Meshgroup)
                if (color[0] == face.color[0] && color[1] == face.color[1] && color[2] == face.color[2]) {
                    hit = true;
                    ID = face.ID;
                    SelectedGroup.Add(Meshgroup[ID]);
                    break;
                }
            if (hit == false)
                return;

            //-----------set group and color
            if (SelectedGroup.Where(p => p.ID == ID).Count() == 2) {
                // change group and color when double select
                SelectedGroup.RemoveAt(SelectedGroup.Count() - 1); //remove repeat item
                if (Meshgroup[ID].groupB == true) {
                    Meshgroup[ID].groupB = false;
                    Meshgroup[ID].groupA = true;
                }
                else {
                    Meshgroup[ID].groupB = true;
                    Meshgroup[ID].groupA = false;
                }
                MakeSortedList();
            }
            else {
                // set group and color on selected item
                Meshgroup[ID].groupB = false;
                Meshgroup[ID].groupA = true;
                MakeSortedList();
            }

            //-----------intersect edge betewwn groupA and groupB
            Vectors.Clear();
            var points = new HashSet<Vector3>();
            foreach (var gA in SelectedGroup.Where(p => p.groupA == true))
                foreach (var gB in SelectedGroup.Where(p => p.groupB == true))
                    foreach (var a in gA.meshes)
                        foreach (var b in gB.meshes) {
                            var intersect = a.vectors.Intersect(b.vectors).ToArray();
                            if (intersect.Count() == 2) {
                                Vectors.Add(new MyLine(intersect[0], intersect[1]) { Norm = b.norm });
                                foreach (var c in intersect)
                                    points.Add(c);
                            }
                        }

            //-----------draw edge
            Gl.glNewList(PathNum, Gl.GL_COMPILE);
            Gl.glColor3f(0, 1, 1);
            foreach (var p in points) {
                Gl.glPushMatrix();
                Gl.glTranslated(p.X, p.Y, p.Z);
                Glu.gluSphere(Glu.gluNewQuadric(), 0.8, 36, 36);
                Gl.glPopMatrix();
            }
            Gl.glBegin(Gl.GL_LINES);
            Gl.glLineWidth(2);
            Gl.glColor3ub(255, 0, 0);
            foreach (var p in Vectors) {
                Gl.glVertex3f(p.v1.X, p.v1.Y, p.v1.Z);
                Gl.glVertex3f(p.v1.X + p.Norm.X * 15, p.v1.Y + p.Norm.Y * 15, p.v1.Z + p.Norm.Z * 15);
                //Gl.glVertex3f(p.v2.X, p.v2.Y, p.v2.Z);
                //Gl.glVertex3f(p.v2.X + p.Norm.X * 15, p.v2.Y + p.Norm.Y * 15, p.v2.Z + p.Norm.Z * 15);
            }
            Gl.glEnd();
            Gl.glEndList();
        }

        public void MakePathList (double angle) {
            try {
                Path = CalcPath(Vectors, angle);
            }
            catch (Exception) {
                MessageBox.Show("Path Error, please check your path.");
            }
            if (Path == null)
                return;

            Gl.glNewList(PathNum, Gl.GL_COMPILE);
            int b = 0, g = 255;
            for (int i = 0; i < Path.Count(); i++) {
                Gl.glColor3ub(0, (byte)g, (byte)b);
                int j = 255 / Path.Count() - 1;
                b += j;
                g -= j;
                Gl.glPushMatrix();
                Gl.glTranslated(Path[i].v1.X, Path[i].v1.Y, Path[i].v1.Z);
                //Glu.gluSphere(Glu.gluNewQuadric(), 1.5, 36, 36);
                Gl.glPopMatrix();
                Gl.glBegin(Gl.GL_LINES);
                Gl.glLineWidth(2);
                Gl.glVertex3f(Path[i].v1.X, Path[i].v1.Y, Path[i].v1.Z);
                Gl.glVertex3f(
                Path[i].v1.X + Path[i].Norm.X * 15,
                Path[i].v1.Y + Path[i].Norm.Y * 15,
                Path[i].v1.Z + Path[i].Norm.Z * 15);
                //Gl.glVertex3f(Path[i].v2.X, Path[i].v2.Y, Path[i].v2.Z);
                //Gl.glVertex3f(
                //Path[i].v2.X + Path[i].Norm.X * 15,
                //Path[i].v2.Y + Path[i].Norm.Y * 15,
                //Path[i].v2.Z + Path[i].Norm.Z * 15);
                Gl.glEnd();
            }
            Gl.glEndList();
            MakeSortedList();
        }

        MyLine[] CalcPath (HashSet<MyLine> vectors, double angle) {
            var tmp = new MyLine[vectors.Count() + 1];
            tmp[0] = new MyLine(vectors.ElementAt(0).v1, vectors.ElementAt(0).v2, vectors.ElementAt(0).Norm); // Choose a head 
            for (int i = 1, cnt = 0; i < vectors.Count(); i++) {
                foreach (var p in vectors) {
                    // Add new vector to the end
                    if (tmp[i - 1].v2 == p.v1 && tmp[i - 1].v1 != p.v2)
                        tmp[i] = new MyLine(p.v1, p.v2, p.Norm);
                    else if (tmp[i - 1].v2 == p.v2 && tmp[i - 1].v1 != p.v1)
                        tmp[i] = new MyLine(p.v2, p.v1, p.Norm);
                }
                if (tmp[i] == null) {
                    // If no vector at the end, choose the last one and go around. Try three time.
                    tmp[0] = new MyLine(tmp[i - 1].v2, tmp[i - 1].v1, tmp[i - 1].Norm);
                    if (cnt > 3) {
                        MessageBox.Show("Try to make poly line or polygon");
                        return null;
                    }
                    cnt++;
                    i = 0;
                }
            }

            var Path = tmp.ToList();
            if (Path.Last() == null) {
                Path.RemoveAt(vectors.Count());
            }
            var tmpV1 = Path.First().Norm;
            var tmpV2 = Path.Last().Norm;

            // See https://en.wikipedia.org/wiki/Rotation_matrix#Rotation_matrix_from_axis_and_angle
            double a = angle * Math.PI / 180.0;
            foreach (var p in Path) {
                Vector3 n = (p.v1 - p.v2).Normalized();
                Matrix3 rotate = new Matrix3(
                    (float)(Math.Cos(a) + n.X * n.X * (1.0 - Math.Cos(a))),
                    (float)(n.X * n.Y * (1.0 - Math.Cos(a)) - n.Z * Math.Sin(a)),
                    (float)(n.X * n.Z * (1.0 - Math.Cos(a)) + n.Y * Math.Sin(a)),
                    (float)(n.Y * n.X * (1.0 - Math.Cos(a)) + n.Z * Math.Sin(a)),
                    (float)(Math.Cos(a) + n.Y * n.Y * (1.0 - Math.Cos(a))),
                    (float)(n.Y * n.Z * (1.0 - Math.Cos(a)) - n.X * Math.Sin(a)),
                    (float)(n.Z * n.X * (1.0 - Math.Cos(a)) - n.Y * Math.Sin(a)),
                    (float)(n.Z * n.Y * (1.0 - Math.Cos(a)) + n.X * Math.Sin(a)),
                    (float)(Math.Cos(a) + n.Z * n.Z * (1.0 - Math.Cos(a))));
                Matrix4 t = new Matrix4(rotate);
                p.Norm = Vector3.Transform(p.Norm, t);
            }
            Path.Insert(0, new MyLine(Path.First().v1 + tmpV1 * 50, Path.First().v1, Path.First().Norm));
            Path.Add(new MyLine(Path.Last().v2, Path.Last().v2 + tmpV2 * 50, Path.Last().Norm));
            Path.Add(new MyLine(Path.Last().v2, Path.Last().v2 + tmpV2 * 50, Path.Last().Norm));

            return Path.ToArray();
        }

        public BindingSource MakeBindingSource () {
            var source = new BindingSource();
            if (Path != null)
                foreach (var p in Path) {
                    if ((p.v1 - p.v2).Length >= 3)
                        source.Add(new XYZABC(p.v1, p.Norm));
                    //source.Add(new XYZWPR((p.v1 + p.v2) / 2, p.Norm)); //interpolation
                    //source.Add(new XYZWPR(p.v2, p.Norm));
                }
            return source;
        }

    }
}
