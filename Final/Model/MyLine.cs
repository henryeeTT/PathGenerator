using OpenTK;

namespace Final.Model {
    public class MyLine {
        public Vector3 Start;
        public Vector3 End;
        public Vector3 Norm;

        public MyLine (Vector3 Start, Vector3 End) {
            this.Start = Start;
            this.End = End;
        }

        public MyLine (Vector3 Start, Vector3 End, Vector3 Norm) {
            this.Start = Start;
            this.End = End;
            this.Norm = Norm;
        }

        public static bool equal (MyLine left, MyLine right) {
            if (left.Start == right.Start && left.End == right.End)
                return true;
            else if (left.End == right.Start && left.Start == right.End)
                return true;
            else
                return false;
        }
    }
}
