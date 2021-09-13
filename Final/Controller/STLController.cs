using Final.Model;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;
using Tao.OpenGl;

namespace Final.Controller {

    public class STLController {
        public bool Sorted = false;
        public int ListNum;
        public TriMesh[] STLdata;
        public TriMesh[] _STLdata;
        public List<MeshGroup> Meshgroup = new List<MeshGroup>();
        public List<MeshGroup> _Meshgroup = new List<MeshGroup>();
        public HashSet<MyLine> Edge = new HashSet<MyLine>();
        public HashSet<MyLine> _Edge = new HashSet<MyLine>();
        public float[] RnT = new float[6]; //Rotate angle x, y, z, translate distance x, y, z.

        public STLController (int listName) {
            ListNum = listName;
        }

        public void ReadBinaryFile (string filePath) {
            List<TriMesh> meshList = new List<TriMesh>();
            int numOfMesh;
            int i;
            int byteIndex;
            byte[] fileBytes = File.ReadAllBytes(filePath);

            byte[] temp = new byte[4];

            /* 80 bytes title + 4 byte num of triangles + 50 bytes (1 of triangular mesh)  */
            if (fileBytes.Length > 120) {

                temp[0] = fileBytes[80];
                temp[1] = fileBytes[81];
                temp[2] = fileBytes[82];
                temp[3] = fileBytes[83];

                numOfMesh = System.BitConverter.ToInt32(temp, 0);

                byteIndex = 84;

                for (i = 0; i < numOfMesh; i++) {
                    TriMesh newMesh = new TriMesh();

                    /* this try-catch block will be reviewed */
                    /* face normal */
                    newMesh.norm.X = System.BitConverter.ToSingle(new byte[] { fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3] }, 0);
                    byteIndex += 4;
                    newMesh.norm.Y = System.BitConverter.ToSingle(new byte[] { fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3] }, 0);
                    byteIndex += 4;
                    newMesh.norm.Z = System.BitConverter.ToSingle(new byte[] { fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3] }, 0);
                    byteIndex += 4;

                    /* vertex 1 */
                    newMesh.v1.X = System.BitConverter.ToSingle(new byte[] { fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3] }, 0);
                    byteIndex += 4;
                    newMesh.v1.Y = System.BitConverter.ToSingle(new byte[] { fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3] }, 0);
                    byteIndex += 4;
                    newMesh.v1.Z = System.BitConverter.ToSingle(new byte[] { fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3] }, 0);
                    byteIndex += 4;

                    /* vertex 2 */
                    newMesh.v2.X = System.BitConverter.ToSingle(new byte[] { fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3] }, 0);
                    byteIndex += 4;
                    newMesh.v2.Y = System.BitConverter.ToSingle(new byte[] { fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3] }, 0);
                    byteIndex += 4;
                    newMesh.v2.Z = System.BitConverter.ToSingle(new byte[] { fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3] }, 0);
                    byteIndex += 4;

                    /* vertex 3 */
                    newMesh.v3.X = System.BitConverter.ToSingle(new byte[] { fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3] }, 0);
                    byteIndex += 4;
                    newMesh.v3.Y = System.BitConverter.ToSingle(new byte[] { fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3] }, 0);
                    byteIndex += 4;
                    newMesh.v3.Z = System.BitConverter.ToSingle(new byte[] { fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3] }, 0);
                    byteIndex += 4;

                    byteIndex += 2; // Attribute byte count

                    newMesh.CombineVector();
                    meshList.Add(newMesh);

                }

            }
            else {
                // nitentionally left blank
            }

            STLdata = meshList.ToArray();
            _STLdata = new TriMesh[STLdata.Count()];
            for (i = 0; i < STLdata.Count(); i++)
                _STLdata[i] = new TriMesh(meshList[i]); // Make a deep copy
        }

        public void ReadASCIIFile (string filePath) {
            List<TriMesh> meshList = new List<TriMesh>();

            StreamReader txtReader = new StreamReader(filePath);

            string lineString;

            while (!txtReader.EndOfStream) {
                lineString = txtReader.ReadLine().Trim(); /* delete whitespace in front and tail of the string */
                string[] lineData = lineString.Split(' ');

                if (lineData[0] == "solid") {
                    while (lineData[0] != "endsolid") {
                        lineString = txtReader.ReadLine().Trim(); // facetnormal
                        lineData = lineString.Split(' ');

                        if (lineData[0] == "endsolid") // check if we reach at the end of file
                        {
                            break;
                        }
                        if (lineData[0] == "") // check if we reach at the end of file
                        {
                            continue;
                        }

                        TriMesh newMesh = new TriMesh(); // define new mesh object

                        /* this try-catch block will be reviewed */
                        //try {
                        // FaceNormal 
                        newMesh.norm.X = float.Parse(lineData[2]);
                        newMesh.norm.Y = float.Parse(lineData[3]);
                        newMesh.norm.Z = float.Parse(lineData[4]);

                        //----------------------------------------------------------------------
                        lineString = txtReader.ReadLine(); // Just skip the OuterLoop line
                                                           //----------------------------------------------------------------------

                        // Vertex1
                        lineString = txtReader.ReadLine().Trim();
                        /* reduce spaces until string has proper format for split */
                        while (lineString.IndexOf("  ") != -1) lineString = lineString.Replace("  ", " ");
                        lineData = lineString.Split(' ');

                        newMesh.v1.X = float.Parse(lineData[1]); // x1
                        newMesh.v1.Y = float.Parse(lineData[2]); // y1
                        newMesh.v1.Z = float.Parse(lineData[3]); // z1

                        // Vertex2
                        lineString = txtReader.ReadLine().Trim();
                        /* reduce spaces until string has proper format for split */
                        while (lineString.IndexOf("  ") != -1) lineString = lineString.Replace("  ", " ");
                        lineData = lineString.Split(' ');

                        newMesh.v2.X = float.Parse(lineData[1]); // x2
                        newMesh.v2.Y = float.Parse(lineData[2]); // y2
                        newMesh.v2.Z = float.Parse(lineData[3]); // z2

                        // Vertex3
                        lineString = txtReader.ReadLine().Trim();
                        /* reduce spaces until string has proper format for split */
                        while (lineString.IndexOf("  ") != -1) lineString = lineString.Replace("  ", " ");
                        lineData = lineString.Split(' ');

                        newMesh.v3.X = float.Parse(lineData[1]); // x3
                        newMesh.v3.Y = float.Parse(lineData[2]); // y3
                        newMesh.v3.Z = float.Parse(lineData[3]); // z3
                        //}
                        //catch (Exception e) {
                        //    MessageBox.Show(e.ToString());
                        //    break;
                        //}

                        //----------------------------------------------------------------------
                        lineString = txtReader.ReadLine(); // Just skip the endloop
                        //----------------------------------------------------------------------
                        lineString = txtReader.ReadLine(); // Just skip the endfacet
                        newMesh.CombineVector();
                        meshList.Add(newMesh); // add mesh to meshList

                    } // while linedata[0]
                } // if solid
            } // while !endofstream

            STLdata = meshList.ToArray();
            _STLdata = new TriMesh[STLdata.Count()];
            for (int i = 0; i < STLdata.Count(); i++)
                _STLdata[i] = new TriMesh(meshList[i]); // Make a deep copy
        }

        public bool OpenFile () {
            OpenFileDialog dgOpenFile = new OpenFileDialog();
            dgOpenFile.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            dgOpenFile.Filter = "STL file(*.stl)|*.stl";
            if (dgOpenFile.ShowDialog() != DialogResult.OK)
                return false;
            var path = System.IO.Path.GetFullPath(dgOpenFile.FileName);
            try {
                ReadASCIIFile(path);
            }
            catch (Exception) {
                ReadBinaryFile(path);
            }
            return true;
        }

        public void SetOriginalPosition (float ax, float ay, float az, float tx, float ty, float tz) {
            RnT = new float[6] { ax, ay, az, tx, ty, tz };
            var tmp = new List<TriMesh>();
            foreach (var x in _STLdata) {
                var v1 = Transform(x.v1, ax, ay, az, tx, ty, tz);
                var v2 = Transform(x.v2, ax, ay, az, tx, ty, tz);
                var v3 = Transform(x.v3, ax, ay, az, tx, ty, tz);
                var n = Transform(x.norm, ax, ay, az, 0, 0, 0);
                tmp.Add(new TriMesh(n, v1, v2, v3));
            }
            STLdata = tmp.ToArray();
        }

        public void SetSortedPosition (float ax, float ay, float az, float tx, float ty, float tz) {
            RnT = new float[6] { ax, ay, az, tx, ty, tz };
            
            for (int i = 0; i < _Meshgroup.Count(); i++) {
                var tmp = new List<TriMesh>();
                foreach (var x in _Meshgroup[i].meshes) {
                    var v1 = Transform(x.v1, ax, ay, az, tx, ty, tz);
                    var v2 = Transform(x.v2, ax, ay, az, tx, ty, tz);
                    var v3 = Transform(x.v3, ax, ay, az, tx, ty, tz);
                    var n = Transform(x.norm, ax, ay, az, 0, 0, 0);
                    tmp.Add(new TriMesh(n, v1, v2, v3));
                }
                Meshgroup[i].meshes = tmp.ToArray();
            }

            Edge = _Edge.Select(p => new MyLine(p.v1, p.v2)).ToHashSet(); // Restore bcackup
            foreach (var x in Edge) {
                x.v1 = Transform(x.v1, ax, ay, az, tx, ty, tz);
                x.v2 = Transform(x.v2, ax, ay, az, tx, ty, tz);
            }
        }

        private Vector3 Transform (Vector3 vector, float ax, float ay, float az, float tx, float ty, float tz) {
            var v = new Vector4(vector) { W = 1 };
            Matrix4 rx = Matrix4.CreateRotationX(ax);
            Matrix4 ry = Matrix4.CreateRotationY(ay);
            Matrix4 rz = Matrix4.CreateRotationZ(az);
            Matrix4 t = Matrix4.CreateTranslation(tx, ty, tz);
            var R = t * rx * ry * rz;
            vector.X = Vector4.Dot(v, R.Column0);
            vector.Y = Vector4.Dot(v, R.Column1);
            vector.Z = Vector4.Dot(v, R.Column2);
            return vector;
        }

        public void MeshGrouping (float eps, int minpts) {
            RnT = new float[6];
            Meshgroup.Clear();
            _Meshgroup.Clear();
            Edge.Clear();
            _Edge.Clear();

            //--------------Do DBSCAN
            Sorted = true;
            HashSet<TriMesh[]> clusters;
            var dbs = new DbscanAlgorithm<TriMesh>((TriMesh s1, TriMesh s2) => {
                if (s1.v1.X == s2.v1.X)
                    if (s1.v1.Y == s2.v1.Y)
                        if (s1.v1.Z == s2.v1.Z)
                            goto End;
                if (s1.v1.X == s2.v2.X)
                    if (s1.v1.Y == s2.v2.Y)
                        if (s1.v1.Z == s2.v2.Z)
                            goto End;
                if (s1.v1.X == s2.v3.X)
                    if (s1.v1.Y == s2.v3.Y)
                        if (s1.v1.Z == s2.v3.Z)
                            goto End;
                //--------------------------------
                if (s1.v2.X == s2.v1.X)
                    if (s1.v2.Y == s2.v1.Y)
                        if (s1.v2.Z == s2.v1.Z)
                            goto End;
                if (s1.v2.X == s2.v2.X)
                    if (s1.v2.Y == s2.v2.Y)
                        if (s1.v2.Z == s2.v2.Z)
                            goto End;
                if (s1.v2.X == s2.v3.X)
                    if (s1.v2.Y == s2.v3.Y)
                        if (s1.v2.Z == s2.v3.Z)
                            goto End;
                //--------------------------------
                if (s1.v3.X == s2.v1.X)
                    if (s1.v3.Y == s2.v1.Y)
                        if (s1.v3.Z == s2.v1.Z)
                            goto End;
                if (s1.v3.X == s2.v2.X)
                    if (s1.v3.Y == s2.v2.Y)
                        if (s1.v3.Z == s2.v2.Z)
                            goto End;
                if (s1.v3.X == s2.v3.X)
                    if (s1.v3.Y == s2.v3.Y)
                        if (s1.v3.Z == s2.v3.Z)
                            goto End;
                return 10;
            End:
                return Vector3.CalculateAngle(s1.norm, s2.norm);
            });
            dbs.ComputeClusterDbscan(STLdata, eps, minpts, out clusters);

            //--------------Set color and mesh group
            var rand = new Random();
            for (int i = 0; i < clusters.Count(); i++) {
                var color = new byte[3];
                rand.NextBytes(color);
                Meshgroup.Add(new MeshGroup(clusters.ElementAt(i), color, i));
                _Meshgroup.Add(new MeshGroup(clusters.ElementAt(i), color, i)); // Make a copy
            }

            //--------------Calculate edge
            HashSet<MyLine> AllLineWithoutEdge = new HashSet<MyLine>();
            HashSet<MyLine> AllLine = new HashSet<MyLine>();
            foreach (var x in Meshgroup)
                foreach (var a in x.meshes)
                    foreach (var b in x.meshes)
                        if (a != b)
                            foreach (var va in a.myLine)
                                foreach (var vb in b.myLine) {
                                    AllLine.Add(va);
                                    if (MyLine.equal(va, vb))
                                        AllLineWithoutEdge.Add(va);
                                }
            Edge = AllLine.Except(AllLineWithoutEdge).ToHashSet();
            _Edge = Edge.Select(p => new MyLine(p.v1, p.v2)).ToHashSet(); // Make a deep copy
        }

        public void MakeOriginalList () {
            Gl.glNewList(ListNum, Gl.GL_COMPILE);

            // draw mesh
            Gl.glEnable(Gl.GL_POLYGON_OFFSET_FILL);
            Gl.glPolygonOffset(1.0f, 1.0f);
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
            foreach (var x in STLdata) {
                Gl.glBegin(Gl.GL_TRIANGLES);
                Gl.glNormal3f(x.norm.X, x.norm.Y, x.norm.Z);
                Gl.glVertex3f(x.v1.X, x.v1.Y, x.v1.Z);
                Gl.glVertex3f(x.v2.X, x.v2.Y, x.v2.Z);
                Gl.glVertex3f(x.v3.X, x.v3.Y, x.v3.Z);
                Gl.glEnd();
            }
            Gl.glDisable(Gl.GL_POLYGON_OFFSET_FILL);

            //draw edge
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(1);
            foreach (var x in STLdata) {
                Gl.glBegin(Gl.GL_LINE_LOOP);
                Gl.glVertex3f(x.v1.X, x.v1.Y, x.v1.Z);
                Gl.glVertex3f(x.v2.X, x.v2.Y, x.v2.Z);
                Gl.glVertex3f(x.v3.X, x.v3.Y, x.v3.Z);
                Gl.glEnd();
            }
            Gl.glEndList();
        }

        public void MakeSortedList () {
            Gl.glNewList(ListNum, Gl.GL_COMPILE);

            // draw mesh group 
            Gl.glEnable(Gl.GL_POLYGON_OFFSET_FILL);
            Gl.glPolygonOffset(1.0f, 1.0f);
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
            Gl.glBegin(Gl.GL_TRIANGLES);
            foreach (var face in Meshgroup) {
                Gl.glColor3d(0.5, 0.6, 0.6);
                if (face.groupA)
                    Gl.glColor3ub(40, 40, 40);
                if (face.groupB)
                    Gl.glColor3ub(210, 210, 210);
                foreach (var p in face.meshes) {
                    Gl.glNormal3f(p.norm.X, p.norm.Y, p.norm.Z);
                    Gl.glVertex3f(p.v1.X, p.v1.Y, p.v1.Z);
                    Gl.glVertex3f(p.v2.X, p.v2.Y, p.v2.Z);
                    Gl.glVertex3f(p.v3.X, p.v3.Y, p.v3.Z);
                }

            }
            Gl.glEnd();
            Gl.glDisable(Gl.GL_POLYGON_OFFSET_FILL);

            // draw edge
            Gl.glLineWidth(1);
            Gl.glBegin(Gl.GL_LINES);
            int r = 0, g = 255;
            foreach (var p in Edge) {
                Gl.glColor3ub(0, 0, 0);
                int i = i = 255 / Edge.Count() + 1;
                r += i;
                g -= i;
                Gl.glVertex3f(p.v1.X, p.v1.Y, p.v1.Z);
                Gl.glVertex3f(p.v2.X, p.v2.Y, p.v2.Z);
            }
            Gl.glEnd();

            Gl.glEndList();
        }

        public void MakeSortedColorList () {
            Gl.glNewList(ListNum, Gl.GL_COMPILE);
            foreach (var face in Meshgroup) {
                foreach (var p in face.meshes) {
                    Gl.glBegin(Gl.GL_TRIANGLES);
                    Gl.glColor3ubv(face.color);
                    Gl.glNormal3f(p.norm.X, p.norm.Y, p.norm.Z);
                    Gl.glVertex3f(p.v1.X, p.v1.Y, p.v1.Z);
                    Gl.glVertex3f(p.v2.X, p.v2.Y, p.v2.Z);
                    Gl.glVertex3f(p.v3.X, p.v3.Y, p.v3.Z);
                    Gl.glEnd();
                }
            }
            Gl.glEndList();
        }

        public void MakeOriginalListWithoutLine () {
            Gl.glNewList(ListNum, Gl.GL_COMPILE);

            // draw mesh
            Gl.glEnable(Gl.GL_POLYGON_OFFSET_FILL);
            Gl.glPolygonOffset(1.0f, 1.0f);
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
            foreach (var x in STLdata) {
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

        public void ResetMeshGroup () {
            foreach (var p in Meshgroup) {
                p.groupA = false;
                p.groupB = false;
            }
        }
    }
}
