using OpenTK;
using System;

namespace PathGenerator.Model {
    public class TriMesh {
        public Vector3 norm;
        public Vector3 v1;
        public Vector3 v2;
        public Vector3 v3;
        public Vector3[] vectors;
        public MyLine[] myLine;

        public TriMesh () {
            norm = new Vector3();
            v1 = new Vector3();
            v2 = new Vector3();
            v3 = new Vector3();
            vectors = new Vector3[3];
            myLine = new MyLine[3];
        }

        public TriMesh (TriMesh a) {
            norm = new Vector3(a.norm);
            v1 = new Vector3(a.v1);
            v2 = new Vector3(a.v2);
            v3 = new Vector3(a.v3);
            vectors = new Vector3[3] { v1, v2, v3 };
            myLine = new MyLine[3] { new MyLine(v1, v2), new MyLine(v2, v3), new MyLine(v1, v3) };
        }

        public TriMesh (Vector3 norm, Vector3 v1, Vector3 v2, Vector3 v3) {
            this.norm = norm;
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            vectors = new Vector3[3] { v1, v2, v3 };
            myLine = new MyLine[3] { new MyLine(v1, v2), new MyLine(v2, v3), new MyLine(v1, v3) };
        }

        public void CombineVector () {
            vectors = new Vector3[3] { v1, v2, v3 };
            myLine = new MyLine[3] { new MyLine(v1, v2), new MyLine(v2, v3), new MyLine(v1, v3) };
        }

        public void CheckZero () { 
            //if (Math.Abs(v1.X) < 0.000001)
            //    norm.X = 0.00001f;
            //if (Math.Abs(v1.Y) < 0.000001)
            //    norm.X = 0.00001f;
            //if (Math.Abs(v1.Z) < 0.000001)
            //    norm.X = 0.00001f;
            //if (Math.Abs(v2.X) < 0.000001)
            //    norm.X = 0.00001f;
            //if (Math.Abs(v2.Y) < 0.000001)
            //    norm.X = 0.00001f;
            //if (Math.Abs(v2.Z) < 0.000001)
            //    norm.X = 0.00001f;
            //if (Math.Abs(v3.X) < 0.000001)
            //    norm.X = 0.00001f;
            //if (Math.Abs(v3.Y) < 0.000001)
            //    norm.X = 0.00001f;
            //if (Math.Abs(v3.Z) < 0.000001)
            //    norm.X = 0.00001f;
        }
    }
}
