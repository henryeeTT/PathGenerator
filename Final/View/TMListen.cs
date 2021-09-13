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

        public TMListen () {
            InitializeComponent();
        }

        private async void btn_connect_Click (object sender, EventArgs e) {
            try {
                client = new TcpClient();
                client.ReceiveTimeout = 3000;
                client.SendTimeout = 3000;
                await Task.WhenAny(client.ConnectAsync(txt_ip.Text, int.Parse(txt_port.Text)), Task.Delay(3000));
                stream = client.GetStream();
                btn_connect.BackColor = Color.Green;
            }
            catch (Exception ex) {
                btn_connect.BackColor = Color.Red;
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
            }
        }

        public string GetString () {
            int cs = 0;
            string s =
                "1,\r\n" +
                "ChangeBase(\"yy\")\r\n" +
                "ChangeTCP(\"pin2\")\r\n";
            foreach (var p in Source)
                //    s += $"PTP(\"CPP\",{p.X.ToString("f3")},{p.Y.ToString("f3")},{p.Z.ToString("f3")},{p.W.ToString("f3")},{p.P.ToString("f3")},{p.R.ToString("f3")},10,150,30,true)" + "\r\n";
                s += $"PLine(\"CAP\",{p.X.ToString("f3")},{p.Y.ToString("f3")},{p.Z.ToString("f3")},{p.A.ToString("f3")},{p.B.ToString("f3")},{p.C.ToString("f3")},100,150,30)" + "\r\n";
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
