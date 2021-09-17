using OpenTK;
using System;
using System.Threading;



namespace Final.Model {

    public class XYZABC {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public XYZABC (Vector3 Vector, Vector3 Normal) {
            Normal = -Normal;
            X = Vector.X;
            Y = Vector.Y;
            Z = Vector.Z;
            var q1 = FromV1toV2(new Vector3(0, 0, 1), Normal);
            var direction = Vector3.Cross(Vector3.UnitY, Normal);
            var Yaxis = (q1 * new Quaternion(Vector3.UnitY, 0) * q1.Inverted()).Xyz;
            var q2 = FromV1toV2(Yaxis, direction);
            var a = ToEulerAngles(q2 * q1);
            A = a.X;
            B = a.Y;
            C = a.Z;

            A *= (180 / Math.PI);
            B *= (180 / Math.PI);
            C *= (180 / Math.PI);
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
