using Final.Controller;
using System;
using System.Windows.Forms;

namespace Final.View {
    public partial class SetPositionBox : Form {
        public STLController stl;
        float[] RnT = new float[6];

        public SetPositionBox (STLController stl) {
            InitializeComponent();
            this.stl = stl;
            stl.RnT.CopyTo(RnT, 0);
            textBox_ax.Text = (RnT[0] * 180 / 3.14f).ToString("F2");
            textBox_ay.Text = (RnT[1] * 180 / 3.14f).ToString("F2");
            textBox_az.Text = (RnT[2] * 180 / 3.14f).ToString("F2");
            textBox_tx.Text = (RnT[3]).ToString();
            textBox_ty.Text = (RnT[4]).ToString();
            textBox_tz.Text = (RnT[5]).ToString();
        }

        private void Form2_KeyDown (object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                stl.RnT = new float[6] {
                    float.Parse(textBox_ax.Text) / 180 * 3.14f,
                    float.Parse(textBox_ay.Text) / 180 * 3.14f,
                    float.Parse(textBox_az.Text) / 180 * 3.14f,
                    float.Parse(textBox_tx.Text),
                    float.Parse(textBox_ty.Text),
                    float.Parse(textBox_tz.Text)};
                if (!stl.Sorted) {
                    stl.SetOriginalPosition(stl.RnT[0], stl.RnT[1], stl.RnT[2], stl.RnT[3], stl.RnT[4], stl.RnT[5]);
                    stl.MakeOriginalList();
                }
                else {
                    stl.SetSortedPosition(stl.RnT[0], stl.RnT[1], stl.RnT[2], stl.RnT[3], stl.RnT[4], stl.RnT[5]);
                    stl.MakeSortedList();
                }
            }
        }

        private void button1_Click (object sender, EventArgs e) {
            this.Close();
        }

        private void button2_Click (object sender, EventArgs e) {
            RnT.CopyTo(stl.RnT, 0);
            if (!stl.Sorted) {
                stl.SetOriginalPosition(stl.RnT[0], stl.RnT[1], stl.RnT[2], stl.RnT[3], stl.RnT[4], stl.RnT[5]);
                stl.MakeOriginalList();
            }
            else {
                stl.SetSortedPosition(stl.RnT[0], stl.RnT[1], stl.RnT[2], stl.RnT[3], stl.RnT[4], stl.RnT[5]);
                stl.MakeSortedList();
            }
            Close();
        }
    }
}
