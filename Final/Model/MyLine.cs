using OpenTK;

namespace Final.Model {
    public class MyLine {
        public Vector3 v1;
        public Vector3 v2;
        public Vector3 Norm;

        public MyLine (Vector3 v1, Vector3 v2) {
            this.v1 = v1;
            this.v2 = v2;
        }

        public MyLine (Vector3 v1, Vector3 v2, Vector3 Norm) {
            this.v1 = v1;
            this.v2 = v2;
            this.Norm = Norm;
        }

        public static bool equal (MyLine left, MyLine right) {
            if (left.v1 == right.v1 && left.v2 == right.v2)
                return true;
            else if (left.v2 == right.v1 && left.v1 == right.v2)
                return true;
            else
                return false;
        }
    }
}
