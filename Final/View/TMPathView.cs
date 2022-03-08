using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PathGenerator.Model;

namespace PathGenerator.View {
    public partial class TMPathView : Form {
        List<XYZABC> Path;
        public TMPathView (List<XYZABC> Path) {
            InitializeComponent();
            this.Path = Path;
            label1.Text += Path.Count.ToString();
        }

        private void button1_Click (object sender, EventArgs e) {
            var tmpath = new TMpath(Path);
            tmpath.SelectAndCreate(txt_Name.Text, int.Parse(txt_speed.Text));
            Close();
        }

        private void textBox2_TextChanged (object sender, EventArgs e) {
            try {
                int.Parse(txt_speed.Text);
            }
            catch {
                txt_speed.Text = "0";
            }
        }
    }
}
