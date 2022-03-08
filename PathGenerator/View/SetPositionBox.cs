using PathGenerator.Controller;
using System;
using OpenTK;
using System.Windows.Forms;

namespace PathGenerator.View {
    public partial class SetPositionBox : Form {
        public STLController stl;

        public SetPositionBox (STLController stl) {
            InitializeComponent();
            this.stl = stl;
            var RotationV = ToEulerAngles(stl.Rotation);
            txt_ax.Text = (RotationV.X * 180 / 3.14f).ToString("0.000");
            txt_ay.Text = (RotationV.Y * 180 / 3.14f).ToString("0.000");
            txt_az.Text = (RotationV.Z * 180 / 3.14f).ToString("0.000");
            txt_tx.Text = (stl.Position.X).ToString("0.000");
            txt_ty.Text = (stl.Position.Y).ToString("0.000");
            txt_tz.Text = (stl.Position.Z).ToString("0.000");
            Event();
        }

        private void Form2_KeyDown (object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                StartMove();
            }
        }

        private void Event () {
            txt_ax.MouseWheel += txt_ax_MouseWheel;
            txt_ay.MouseWheel += txt_ay_MouseWheel;
            txt_az.MouseWheel += txt_az_MouseWheel;
            txt_tx.MouseWheel += txt_tx_MouseWheel;
            txt_ty.MouseWheel += txt_ty_MouseWheel;
            txt_tz.MouseWheel += txt_tz_MouseWheel;

            txt_ax.TextChanged += Txt_ax_TextChanged;
            txt_ay.TextChanged += Txt_ay_TextChanged;
            txt_az.TextChanged += Txt_az_TextChanged;
            txt_tx.TextChanged += Txt_tx_TextChanged;
            txt_ty.TextChanged += Txt_ty_TextChanged;
            txt_tz.TextChanged += Txt_tz_TextChanged;
        }

        private void Txt_tx_TextChanged (object sender, EventArgs e) {
            try {
                float.Parse(txt_tx.Text);
                StartMove();
            }
            catch (Exception) {
                txt_tx.Text = "0";
            }
        }

        private void Txt_ty_TextChanged (object sender, EventArgs e) {
            try {
                float.Parse(txt_ty.Text);
                StartMove();
            }
            catch (Exception) {
                txt_ty.Text = "0";
            }
        }

        private void Txt_tz_TextChanged (object sender, EventArgs e) {
            try {
                float.Parse(txt_tz.Text);
                StartMove();
            }
            catch (Exception) {
                txt_tz.Text = "0";
            }
        }

        private void Txt_ax_TextChanged (object sender, EventArgs e) {
            try {
                float.Parse(txt_ax.Text);
                StartMove();
            }
            catch (Exception) {
                txt_ax.Text = "0";
            }
        }

        private void Txt_ay_TextChanged (object sender, EventArgs e) {
            try {
                float.Parse(txt_ay.Text);
                StartMove();
            }
            catch (Exception) {
                txt_ay.Text = "0";
            }
        }

        private void Txt_az_TextChanged (object sender, EventArgs e) {
            try {
                float.Parse(txt_az.Text);
                StartMove();
            }
            catch (Exception) {
                txt_az.Text = "0";
            }
        }

        private void txt_tx_MouseWheel (object sender, MouseEventArgs e) {
            if (e.Delta > 0)
                txt_tx.Text = (float.Parse(txt_tx.Text) + 5).ToString("0.000");
            else
                txt_tx.Text = (float.Parse(txt_tx.Text) - 5).ToString("0.000");
        }

        private void txt_ty_MouseWheel (object sender, MouseEventArgs e) {
            if (e.Delta > 0)
                txt_ty.Text = (float.Parse(txt_ty.Text) + 5).ToString("0.000");
            else
                txt_ty.Text = (float.Parse(txt_ty.Text) - 5).ToString("0.000");
        }

        private void txt_tz_MouseWheel (object sender, MouseEventArgs e) {
            if (e.Delta > 0)
                txt_tz.Text = (float.Parse(txt_tz.Text) + 5).ToString("0.000");
            else
                txt_tz.Text = (float.Parse(txt_tz.Text) - 5).ToString("0.000");
        }

        private void txt_ax_MouseWheel (object sender, MouseEventArgs e) {
            if (e.Delta > 0)
                txt_ax.Text = (float.Parse(txt_ax.Text) + 5).ToString("0.000");
            else
                txt_ax.Text = (float.Parse(txt_ax.Text) - 5).ToString("0.000");
        }

        private void txt_ay_MouseWheel (object sender, MouseEventArgs e) {
            if (e.Delta > 0)
                txt_ay.Text = (float.Parse(txt_ay.Text) + 5).ToString("0.000");
            else
                txt_ay.Text = (float.Parse(txt_ay.Text) - 5).ToString("0.000");
        }

        private void txt_az_MouseWheel (object sender, MouseEventArgs e) {
            if (e.Delta > 0)
                txt_az.Text = (float.Parse(txt_az.Text) + 5).ToString("0.000");
            else
                txt_az.Text = (float.Parse(txt_az.Text) - 5).ToString("0.000");
        }

        private void StartMove () {
            stl.Position = new Vector3(float.Parse(txt_tx.Text), float.Parse(txt_ty.Text), float.Parse(txt_tz.Text));
            stl.Rotation = new Quaternion(float.Parse(txt_az.Text) / 180 * 3.14f, float.Parse(txt_ay.Text) / 180 * 3.14f, float.Parse(txt_ax.Text) / 180 * 3.14f);
            if (!stl.Sorted) {
                stl.SetOriginalPosition();
                stl.MakeOriginalList();
            }
            else {
                stl.SetSortedPosition();
                stl.MakeSortedList();
            }
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
    }
}
