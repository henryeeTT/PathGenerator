using OpenTK;
using System;
using System.Threading;



namespace PathGenerator.Model {

    public class XYZABC {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public XYZABC (Vector3 Position, Vector3 Normal) {
            Normal = -Normal;
            X = Position.X;
            Y = Position.Y;
            Z = Position.Z;

            var Yaxis = Vector3.Cross(Normal, Vector3.UnitY).Normalized();
            var Xaxis = Vector3.Cross(Yaxis, Normal).Normalized();
            var m = new Matrix3(Xaxis, Yaxis, Normal);
            m.Transpose();
            var a = RotationMatrixToEulerAngles(m);
            A = a.X;
            B = a.Y;
            C = a.Z;

            var r2pi = 180.0 / Math.PI;
            A *= r2pi;
            B *= r2pi;
            C *= r2pi;
            //A = -180;
            //B = 0;
            //C = 90;
        }

        private Quaternion FromV1toV2 (Vector3 u, Vector3 v) {
            if (u == -v) {
                float x = Math.Abs(u.X);
                float y = Math.Abs(u.Y);
                float z = Math.Abs(u.Z);
                Vector3 other = x < y ? (x < z ? new Vector3(1, 0, 0) : new Vector3(0, 0, 1)) : (y < z ? new Vector3(0, 1, 0) : new Vector3(0, 0, 1));
                var tmp = Vector3.Cross(v, other);
                return new Quaternion(Vector3.Normalize(tmp), 0);
            }
            u.Normalize();
            v.Normalize();
            return Quaternion.FromAxisAngle(Vector3.Cross(u, v), (float)Math.Acos(Vector3.Dot(u, v)));
        }

        private Quaternion fromV1toV2 (Vector3 u, Vector3 v) {
            if (u == -v) {
                float x = Math.Abs(u.X);
                float y = Math.Abs(u.Y);
                float z = Math.Abs(u.Z);
                Vector3 other = x < y ? (x < z ? new Vector3(1, 0, 0) : new Vector3(0, 0, 1)) : (y < z ? new Vector3(0, 1, 0) : new Vector3(0, 0, 1));
                var tmp = Vector3.Cross(v, other);
                return new Quaternion(Vector3.Normalize(tmp), 0);
            }

            Vector3 half = Vector3.Normalize(u + v);
            return new Quaternion(Vector3.Cross(u, half), Vector3.Dot(u, half));
        }

        private Vector3 ToEulerAngles (Quaternion q) {
            double x, y, z;
            float sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            float cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            x = Math.Atan2(sinr_cosp, cosr_cosp);

            float sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
                y = Math.PI / 2 * Math.Sign(sinp); // use 90 degrees if out of range
            else
                y = Math.Asin(sinp);
            float siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            float cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            z = Math.Atan2(siny_cosp, cosy_cosp);
            return new Vector3((float)x, (float)y, (float)z);
        }

        Vector3 RotationMatrixToEulerAngles (Matrix3 R) {
            var sy = Math.Sqrt(R.M11 * R.M11 + R.M21 * R.M21);
            bool singular = sy < 1e-6; // If
            double x, y, z;
            if (!singular) {
                x = Math.Atan2(R.M32, R.M33);
                y = Math.Atan2(-R.M31, sy);
                z = Math.Atan2(R.M21, R.M11);
            }
            else {
                x = Math.Atan2(-R.M23, R.M22);
                y = Math.Atan2(-R.M31, sy);
                z = 0;
            }
            return new Vector3((float)x, (float)y, (float)z);
        }

        public XYZABC () {
            X = 0;
            Y = 0;
            Z = 0;
            A = 0;
            B = 0;
            C = 0;
        }

    }
}
