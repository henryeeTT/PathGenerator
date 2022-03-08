using Final.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final.View {
    public partial class TMListen : Form {
        public List<XYZABC> Source;
        TcpClient client;
        NetworkStream stream;
        Object _lock;

        public TMListen (List<XYZABC> Source) {
            InitializeComponent();
            this.Source = Source;
            UpdateUI();
        }

        public void UpdateUI () {
            label3.Text += Source.Count.ToString();
        }

        private async void btn_connect_Click (object sender, EventArgs e) {
            try {
                client = new TcpClient();
                client.ReceiveTimeout = 3000;
                client.SendTimeout = 3000;
                await Task.WhenAny(client.ConnectAsync(txt_ip.Text, int.Parse(txt_port.Text)), Task.Delay(3000));
                stream = client.GetStream();
                btn_connect.BackColor = Color.Green;
                btn_connect.Enabled = false;
                await Task.Run(() => {
                    while (client.Connected)
                        Task.Delay(1000);
                });
                btn_connect.BackColor = Color.Red;
                stream.Close();
                client.Close();
            }
            catch (Exception ex) {
                btn_connect.BackColor = Color.Red;
                stream.Close();
                client.Close();
            }

        }

        private async void btn_star_Click (object sender, EventArgs e) {
            try {
                var bytes = Encoding.UTF8.GetBytes(GetString());
                await Task.WhenAny(stream.WriteAsync(bytes, 0, bytes.Length), Task.Delay(3000));
                btn_connect.BackColor = Color.Green;
            }
            catch (Exception) {
                btn_connect.BackColor = Color.Red;
                stream.Close();
                client.Close();
            }
        }

        public string GetString () {
            int cs = 0;
            XYZABC p;
            string s =
                "1,\r\n" +
                "ChangeBase(\"yy\")\r\n" +
                "ChangeTCP(\"pin2\")\r\n";

            for (int i = 0; i < Source.Count(); i++) {
                p = Source[i];
                if (i == 0 || i == 1 || i == Source.Count() || i == Source.Count() - 1)
                    s += $"PLine(\"CAP\",{p.X.ToString("f3")},{p.Y.ToString("f3")},{p.Z.ToString("f3")},{p.A.ToString("f3")},{p.B.ToString("f3")},{p.C.ToString("f3")},100,150,0)" + "\r\n";
                else
                    s += $"PLine(\"CAP\",{p.X.ToString("f3")},{p.Y.ToString("f3")},{p.Z.ToString("f3")},{p.A.ToString("f3")},{p.B.ToString("f3")},{p.C.ToString("f3")},100,150,50)" + "\r\n";

            }


            s = s.Remove(s.Length - 2, 2);
            s = s.Insert(0, $"TMSCT,{s.Length},");
            s = s.Insert(s.Length, ",");
            foreach (var x in s.ToArray())
                cs ^= x;
            s = s.Insert(0, "$");
            s = s.Insert(s.Length, $"*{cs.ToString("X")}\r\n");
            return s;
        }


    }

}
