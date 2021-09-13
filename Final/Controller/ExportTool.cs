using Final.Model;
using Final.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final.Controller {
    public class ExportTool {
        List<XYZABC> Source;

        public ExportTool (List<XYZABC> source) {
            Source = source;
        }

        public void Export () {
            if (Source.Count != 0) {
                SaveFileDialog dgSaveFile = new SaveFileDialog();
                dgSaveFile.Filter = "ls files(*.ls)|*.ls";
                if (dgSaveFile.ShowDialog() == DialogResult.OK) {
                    try {
                        if (dgSaveFile.FileName != "") {
                            StringBuilder sb = new StringBuilder();
                            string tmpTime = DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                            string[] tmp = tmpTime.Split(' ');
                            MessageBox.Show("Save: " + dgSaveFile.FileName + " at : " + DateTime.Now.ToString("yy-MM-dd HH:mm:ss"));
                            sb.Append("/PROG  " + System.IO.Path.GetFileNameWithoutExtension(dgSaveFile.FileName) + "\r\n");
                            sb.Append("/ATTR\r\n");
                            sb.Append("OWNER         = MNEDITOR;\r\n");
                            sb.Append("COMMENT       = \"Build by EdgeDetector\";\r\n");
                            sb.Append("PROG_SIZE     = 200;\r\n");
                            sb.Append("CREATE        = DATE " + tmp[0] + "  TIME " + tmp[1] + ";\r\n");
                            sb.Append("MODIFIED      = DATE " + tmp[0] + "  TIME " + tmp[1] + ";\r\n");
                            sb.Append("FILE_NAME     = ;\r\n");
                            sb.Append("VERSION       = 0;\r\n");
                            sb.Append("LINE_COUNT    = " + Source.Count() + ";\r\n");
                            sb.Append("MEMORY_SIZE   = 1197;\r\n");
                            sb.Append("PROTECT       = READ_WRITE;\r\n");
                            sb.Append("TCD:  STACK_SIZE    = 0,\r\n");
                            sb.Append("      TASK_PRIORITY = 50,\r\n");
                            sb.Append("      TIME_SLICE    = 0,\r\n");
                            sb.Append("      BUSY_LAMP_OFF = 0,\r\n");
                            sb.Append("      ABORT_REQUEST = 0,\r\n");
                            sb.Append("      PAUSE_REQUEST = 0;\r\n");
                            sb.Append("DEFAULT_GROUP = 1,*,*,*,*;\r\n");
                            sb.Append("CONTROL_CODE	 = 00000000 00000000;\r\n");
                            sb.Append("/MN\r\n");
                            sb.Append("1: UTOOL_NUM=1;\r\n");
                            sb.Append("2: UFRAME_NUM=1;\r\n");
                            sb.Append("3: ;\r\n");
                            sb.Append("4: J P[1] 60% CNT50;\r\n");
                            for (int i = 0; i < Source.Count(); i++) {
                                sb.Append(i + 5);
                                sb.Append(":L P[");
                                sb.Append(i + 2);
                                sb.Append("] 30mm/sec CNT10;\r\n");
                            }
                            //sb.Append("] " + txt_LS_spd.Text.ToString() + "mm/sec CNT" + txt_CNT.Text.ToString() + "  ;\r\n");
                            sb.Append("/POS\r\n");
                            sb.Append("P[1]{\r\n");
                            sb.Append("   GP1:\r\n");
                            sb.Append("	UF : 1, UT : 1,		CONFIG : 'N U T, 0, 0, 0',\r\n");
                            sb.AppendFormat("	X =    {0:0.000}  mm,	Y =  {1:0.000}  mm,	 Z =   {2:0.000}  mm,\r\n", Source[0].X, Source[0].Y, Source[0].Z);
                            sb.AppendFormat("	W =    {0:0.000}  deg,	P =  {1:0.000}  deg,    R =   {2:0.000}  deg\r\n", Source[0].A, Source[0].B, Source[0].C);
                            sb.Append("};\r\n");
                            for (int i = 0; i < Source.Count(); i++) {
                                sb.Append("P[");
                                sb.Append(i + 2);
                                sb.Append("]{\r\n");
                                sb.Append("   GP1:\r\n");
                                sb.Append("	UF : 1, UT : 1,		CONFIG : 'N U T, 0, 0, 0',\r\n");
                                sb.AppendFormat("	X =    {0:0.000}  mm,	Y =  {1:0.000}  mm,	 Z =   {2:0.000}  mm,\r\n", Source[i].X, Source[i].Y, Source[i].Z);
                                sb.AppendFormat("	W =    {0:0.000}  deg,	P =  {1:0.000}  deg,    R =   {2:0.000}  deg\r\n", Source[i].A, Source[i].B, Source[i].C);
                                sb.Append("};\r\n");
                            }
                            sb.Append("/END\r\n");
                            StreamWriter sw = new StreamWriter(dgSaveFile.FileName);
                            sw.WriteLine(sb.ToString());
                            sw.Close();
                        }
                    }
                    catch (Exception ex) {
                        MessageBox.Show("Save File Error :" + ex.Message);
                    }
                }
            }
            else
                MessageBox.Show("Please calculate path first.");
        }

        public void ExportCSV () {
            if (Source.Count != 0) {
                SaveFileDialog dgSaveFile = new SaveFileDialog();
                dgSaveFile.Filter = "csv files(*.csv)|*.csv";
                if (dgSaveFile.ShowDialog() == DialogResult.OK) {
                    try {
                        if (dgSaveFile.FileName != "") {
                            var Path = new XYZABC[this.Source.Count];
                            this.Source.CopyTo(Path, 0);
                            StringBuilder sb = new StringBuilder();
                            string tmpTime = DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                            string[] tmp = tmpTime.Split(' ');
                            MessageBox.Show("Save: " + dgSaveFile.FileName + " at : " + DateTime.Now.ToString("yy-MM-dd HH:mm:ss"));
                            for (int i = 0; i < Path.Count(); i++) {
                                sb.AppendFormat("{0:0.000},{1:0.000},{2:0.000},", Path[i].X, Path[i].Y, Path[i].Z);
                                sb.AppendFormat("{0:0.000},{1:0.000},{2:0.000}\r\n", Path[i].A, Path[i].B, Path[i].C);
                            }
                            StreamWriter sw = new StreamWriter(dgSaveFile.FileName);
                            sw.WriteLine(sb.ToString());
                            sw.Close();
                        }
                    }
                    catch (Exception ex) {
                        MessageBox.Show("Save File Error :" + ex.Message);
                    }
                }
            }
            else
                MessageBox.Show("Please calculate path first.");
        }

        public void ExportTM () {
            var tmp = new TMListen();
            tmp.Source = Source;
            tmp.ShowDialog();
        }
    }
}
