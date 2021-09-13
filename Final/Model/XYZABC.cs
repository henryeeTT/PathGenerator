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

        public XYZABC (Vector3 Vector, Vector3 Normal, int i) {
            X = Vector.X;
            Y = Vector.Y;
            Z = Vector.Z;
            Normal = -Normal;
            // See https://math.stackexchange.com/questions/180418/calculate-rotation-matrix-to-align-vector-a-to-vector-b-in-3d
            var v = Vector3.Cross(new Vector3(0, 0, 1), Normal);
            float c = (float)Vector3.Dot(new Vector3(0, 0, 1), Normal);
            Matrix3 I = new Matrix3(1, 0, 0, 0, 1, 0, 0, 0, 1);
            Matrix3 vx = new Matrix3(0, -v.Z, v.Y, v.Z, 0, -v.X, -v.Y, v.X, 0);
            Matrix3 vx_2 = Matrix3.Mult(vx, vx);
            vx_2.Row0 = vx_2.Row0 / (1.0f + c);
            vx_2.Row1 = vx_2.Row1 / (1.0f + c);
            vx_2.Row2 = vx_2.Row2 / (1.0f + c);
            Matrix3 RM = Matrix3.Add(I, vx);
            RM = Matrix3.Add(RM, vx_2);

            //Vector3 axis = Vector3.Normalize(Vector3.Cross(new Vector3(0, 0, -1), Normal));
            //float dotProduct = Vector3.Dot(new Vector3(0, 0, -1), Normal);
            //if (dotProduct >= 1)
            //    dotProduct = 1;
            //if (dotProduct <= -1)
            //    dotProduct = -1;
            //float angleRadians = (float)Math.Asin(dotProduct);
            //float sinA = (float)Math.Sin(angleRadians);
            //float cosA = (float)Math.Cos(angleRadians);
            //float invCosA = 1.0f - cosA;
            //Matrix3 RM = new Matrix3((axis.X * axis.X * invCosA) + cosA,
            //     (axis.Y * axis.X * invCosA) - (sinA * axis.Z),
            //     (axis.Z * axis.X * invCosA) + (sinA * axis.Y),
            //     (axis.X * axis.Y * invCosA) + (sinA * axis.Z),
            //     (axis.Y * axis.Y * invCosA) + cosA,
            //     (axis.Z * axis.Y * invCosA) - (sinA * axis.X),
            //     (axis.X * axis.Z * invCosA) - (sinA * axis.Y),
            //     (axis.Y * axis.Z * invCosA) + (sinA * axis.X),
            //     (axis.Z * axis.Z * invCosA) + cosA);

            //// See http://www.kwon3d.com/theory/euler/euler_angles.html
            //P = Math.Asin(RM.M31);
            //if (RM.M33 * Math.Cos(P) > 0)
            //    W = Math.Atan(-RM.M32 / RM.M33);
            //else
            //    W = Math.Atan(-RM.M32 / RM.M33) + Math.PI;
            //if (RM.M11 * Math.Cos(P) > 0)
            //    R = Math.Atan(-RM.M21 / RM.M11);
            //else
            //    R = Math.Atan(-RM.M21 / RM.M11) + Math.PI;

            // See https://www.learnopencv.com/rotation-matrix-to-euler-angles/
            float sy = (float)Math.Sqrt(RM.M11 * RM.M11 + RM.M21 * RM.M21);
            if (sy > 0.00001) {
                A = Math.Atan2(RM.M32, RM.M33);
                B = Math.Atan2(-RM.M31, sy);
                C = Math.Atan2(RM.M21, RM.M11);
            }
            else {
                A = Math.Atan2(RM.M23, RM.M22);
                B = Math.Atan2(-RM.M31, sy);
                C = 0;
            }

            A *= (180 / Math.PI);
            B *= (180 / Math.PI);
            C *= (180 / Math.PI);
        }

        public XYZABC (Vector3 Vector, Vector3 Normal) {
            Normal = -Normal;
            X = Vector.X;
            Y = Vector.Y;
            Z = Vector.Z;
            var q1 = FromV1toV2(new Vector3(0, 0, 1), Normal);
            var q1i = q1.Inverted();
            var Xaxis = (q1 * new Quaternion(1, 0, 0, 0) * q1i);
            var Zaxis = (q1 * new Quaternion(0, 0, 1, 0) * q1i);
            var cross = Vector3.Cross(Zaxis.Xyz, Vector3.UnitY);
            var angle = Math.Acos(Vector3.Dot(Xaxis.Xyz.Normalized(), cross.Normalized()));
            var q2 = Quaternion.FromAxisAngle(Zaxis.Xyz, (float)angle);
            var a = ToEulerAngles(q2*q1);
            A = a.X;
            B = a.Y;
            C = a.Z;



            A *= (180 / Math.PI);
            B *= (180 / Math.PI);
            C *= (180 / Math.PI);

        }


        public Quaternion FromV1toV2 (Vector3 u, Vector3 v) {
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

        Vector3 ToEulerAngles (Quaternion q) {
            Vector3 angles = new Vector3();

            // roll (x-axis rotation)
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            angles.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch (y-axis rotation)
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)// use 90 degrees if out of range
                if (sinp > 0)
                    angles.Y = (float)Math.PI;
                else
                    angles.Y = (float)-Math.PI;
            else
                angles.Y = (float)Math.Asin(sinp);

            // yaw (z-axis rotation)
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            angles.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            return angles;
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
