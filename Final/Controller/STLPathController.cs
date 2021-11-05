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
        private List<byte[]> ColorRef = new List<byte[]>();

        public STLPathController (int ListNum, int PathNum) : base(ListNum) {
            this.ListNum = ListNum;
            this.PathNum = PathNum;
            byte r = 0, g = 0, b = 0;
            while (true) {
                ColorRef.Add(new byte[] { r, g, b });
                if (r < 255) {
                    r++;
                }
                else if (g < 255) {
                    g++;
                    r = 0;
                }
                else if (b < 255) {
                    b++;
                    r = 0;
                    g = 0;
                }
                else {
                    break;
                }
            }
        }

        public void ResetPath () {
            SelectedGroup.Clear();
            Vectors.Clear();
            Gl.glNewList(PathNum, Gl.GL_COMPILE);
            Gl.glEndList();
        }

        #region test
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
                Gl.glVertex3f(p.Start.X, p.Start.Y, p.Start.Z);
                Gl.glVertex3f(p.Start.X + p.Norm.X * 15, p.Start.Y + p.Norm.Y * 15, p.Start.Z + p.Norm.Z * 15);
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
                Gl.glTranslated(Path[i].Start.X, Path[i].Start.Y, Path[i].Start.Z);
                //Glu.gluSphere(Glu.gluNewQuadric(), 1.5, 36, 36);
                Gl.glPopMatrix();
                Gl.glBegin(Gl.GL_LINES);
                Gl.glLineWidth(2);
                Gl.glVertex3f(Path[i].Start.X, Path[i].Start.Y, Path[i].Start.Z);
                Gl.glVertex3f(
                Path[i].Start.X + Path[i].Norm.X * 15,
                Path[i].Start.Y + Path[i].Norm.Y * 15,
                Path[i].Start.Z + Path[i].Norm.Z * 15);
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
            tmp[0] = new MyLine(vectors.ElementAt(0).Start, vectors.ElementAt(0).End, vectors.ElementAt(0).Norm); // Choose a head 
            for (int i = 1, cnt = 0; i < vectors.Count(); i++) {
                foreach (var p in vectors) {
                    // Add new vector to the end
                    if (tmp[i - 1].End == p.Start && tmp[i - 1].Start != p.End)
                        tmp[i] = new MyLine(p.Start, p.End, p.Norm);
                    else if (tmp[i - 1].End == p.End && tmp[i - 1].Start != p.Start)
                        tmp[i] = new MyLine(p.End, p.Start, p.Norm);
                }
                if (tmp[i] == null) {
                    // If no vector at the end, choose the last one and go around. Try three time.
                    tmp[0] = new MyLine(tmp[i - 1].End, tmp[i - 1].Start, tmp[i - 1].Norm);
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
                Vector3 n = (p.Start - p.End).Normalized();
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
            Path.Insert(0, new MyLine(Path.First().Start + tmpV1 * 50, Path.First().Start, Path.First().Norm));
            Path.Add(new MyLine(Path.Last().End, Path.Last().End + tmpV2 * 50, Path.Last().Norm));
            Path.Add(new MyLine(Path.Last().End, Path.Last().End + tmpV2 * 50, Path.Last().Norm));

            return Path.ToArray();
        }

        public BindingSource MakeBindingSource () {
            var source = new BindingSource();
            if (Path != null)
                foreach (var p in Path) {
                   // if ((p.Start - p.End).Length >= 2)
                        source.Add(new XYZABC(p.Start, p.Norm));
                    //source.Add(new XYZWPR((p.v1 + p.v2) / 2, p.Norm)); //interpolation
                    //source.Add(new XYZWPR(p.v2, p.Norm));
                }
            return source;
        }
        #endregion

        #region Line
        public void ClearPathList () {
            Gl.glNewList(PathNum, Gl.GL_COMPILE);
            Gl.glEndList();
        } 

        public void MakeLineList (int x, int y) {            
            byte[] color = new byte[3];
            var p = Transform2Dto3D(x, y);
            Gl.glReadPixels(x, y, 1, 1, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, color);
            if (IfGetMesh(color)) {
                int cnt = color[0] + color[1] * 256 + color[2] * 65536;
                if (Vectors.Count == 0)
                    Vectors.Add(new MyLine(p, p, STLdata[cnt].norm));
                else {
                    var cut = Math.Abs((int)(p - Vectors.Last().End).LengthFast / 2);
                    var start = Transform3Dto2D(Vectors.Last().End);
                    var dx = (x - start.X)/cut;
                    var dy = (y - start.Y)/cut;
                    for (int i =1; i<cut; i++) {
                        x = (int)(start.X + i* dx);
                        y = (int)(start.Y + i*dy);
                        Gl.glReadPixels(x, y, 1, 1, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, color);
                        if (IfGetMesh(color)) {
                            int c = color[0] + color[1] * 256 + color[2] * 65536;
                            Vectors.Add(new MyLine(Vectors.Last().End, Transform2Dto3D(x, y), STLdata[c].norm));
                        }
                    }
                    Vectors.Add(new MyLine(Vectors.Last().End, p, STLdata[cnt].norm));
                }
                Path = Vectors.ToArray();
                Gl.glNewList(PathNum, Gl.GL_COMPILE);
                int b = 0, g = 255;
                for (int i = 0; i < Path.Count(); i++) {
                    Gl.glColor3ub(0, (byte)g, (byte)b);
                    int j = 255 / Path.Count() - 1;
                    b += j;
                    g -= j;
                    Gl.glPushMatrix();
                    Gl.glTranslated(Path[i].Start.X, Path[i].Start.Y, Path[i].Start.Z);
                    Glu.gluSphere(Glu.gluNewQuadric(), 1.5, 36, 36);
                    Gl.glPopMatrix();
                    Gl.glBegin(Gl.GL_LINES);
                    Gl.glLineWidth(2);
                    Gl.glVertex3f(Path[i].Start.X, Path[i].Start.Y, Path[i].Start.Z);
                    Gl.glVertex3f(
                    Path[i].Start.X + Path[i].Norm.X * 15,
                    Path[i].Start.Y + Path[i].Norm.Y * 15,
                    Path[i].Start.Z + Path[i].Norm.Z * 15);
                    //Gl.glVertex3f(Path[i].v2.X, Path[i].v2.Y, Path[i].v2.Z);
                    //Gl.glVertex3f(
                    //Path[i].v2.X + Path[i].Norm.X * 15,
                    //Path[i].v2.Y + Path[i].Norm.Y * 15,
                    //Path[i].v2.Z + Path[i].Norm.Z * 15);
                    Gl.glEnd();
                    Gl.glBegin(Gl.GL_LINES);
                    Gl.glLineWidth(2);
                    Gl.glVertex3f(Path[i].Start.X, Path[i].Start.Y, Path[i].Start.Z);
                    Gl.glVertex3f(Path[i].End.X, Path[i].End.Y, Path[i].End.Z);
                    Gl.glEnd();
                }
                Gl.glEndList();
            }
        }

        private bool IfGetMesh (byte[] color) {
            if (color[0] != 229 && color[0] != 229 && color[0] != 229 && color[0] != 229)
                if (color[0] + color[1] * 256 + color[2] * 65536 <= STLdata.Length)
                    return true;
                else
                    return false;
            else 
                return false;
            
        }

        public BindingSource MakeLineBindingSource () {
            var source = new BindingSource();
            Vectors.Add(new MyLine(Vectors.Last().End, Vectors.Last().End + 30 * Vectors.Last().Norm, Vectors.Last().Norm));
            Vectors.Add(new MyLine(Vectors.Last().End, Vectors.Last().End + 30 * Vectors.Last().Norm, Vectors.Last().Norm));
            Path = Vectors.ToArray();
            Path[0].Start = Path[0].Start + 20 * Path[0].Norm;
            if (Path != null)
                foreach (var p in Path) {
                        source.Add(new XYZABC(p.Start, p.Norm));
                    //source.Add(new XYZWPR((p.v1 + p.v2) / 2, p.Norm)); //interpolation
                    //source.Add(new XYZWPR(p.v2, p.Norm));
                }
            return source;
        }

        public void MakeColorList () {
            int i = 0;
            Gl.glNewList(ListNum, Gl.GL_COMPILE);

            // draw mesh
            foreach (var x in STLdata) {
                Gl.glColor3ubv(ColorRef[i++]);
                Gl.glBegin(Gl.GL_TRIANGLES);
                Gl.glNormal3f(x.norm.X, x.norm.Y, x.norm.Z);
                Gl.glVertex3f(x.v1.X, x.v1.Y, x.v1.Z);
                Gl.glVertex3f(x.v2.X, x.v2.Y, x.v2.Z);
                Gl.glVertex3f(x.v3.X, x.v3.Y, x.v3.Z);
                Gl.glEnd();
            }
            Gl.glDisable(Gl.GL_POLYGON_OFFSET_FILL);
            Gl.glEndList();
        }

        private Vector3 Transform2Dto3D (int x, int y) {
            int[] viewport = new int[4];
            double[] modelview = new double[16];
            double[] projection = new double[16];
            double[] p = new double[3];
            float[] depth = new float[1];
            Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, modelview);
            Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, projection);
            Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
            Gl.glReadPixels(x, y, 1, 1, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, depth);
            Glu.gluUnProject(x, y, depth[0], modelview, projection, viewport, out p[0], out p[1], out p[2]);
            return new Vector3((float)p[0], (float)p[1], (float)p[2]);
        }

        private Vector2 Transform3Dto2D (Vector3 vec) {
            int[] viewport = new int[4];
            double[] modelview = new double[16];
            double[] projection = new double[16];
            Vector3d v;
            Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, modelview);
            Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, projection);
            Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
            Glu.gluProject(vec.X, vec.Y, vec.Z, modelview, projection, viewport, out v.X, out v.Y, out v.Z);
            return new Vector2((float)v.X, (float)v.Y);
        }
        #endregion
    }
}
