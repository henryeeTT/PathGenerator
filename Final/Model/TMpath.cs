using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;

namespace PathGenerator.Model {

    public class TMpath {
        private List<XYZABC> robotPath;
        private string RootPath;

        public TMpath (List<XYZABC> RobotPath) {
            robotPath = RobotPath;
        }

        public void SelectAndCreate (string FileName, int speed) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            var path = fbd.SelectedPath;
            if (path != null) {
                path = Path.Combine(path, FileName);
                Directory.CreateDirectory(path);
                RootPath = path;
                path += $@"\path1.PATH";
                CreatePathFile(path, speed);
                RunZip(RootPath);
            }
        }

        private void CreateConfig (string path) {
            using (StreamWriter sw = new StreamWriter(path + @"/ConfigData.XML")) {
                sw.WriteLine(@"<Configuration></Configuration>");
            }
        }

        private void CreatePathFile (string path, int speed) {
            var serilizer = new XmlSerializer(typeof(Root));
            var root = new Root();
            root.PointCount = robotPath.Count();
            for (int i = 0; i < robotPath.Count; i++) {
                root.Point.Add(new Point(robotPath[i], speed, i));
            }
            using (var stream = new FileStream(path, FileMode.Create))
                serilizer.Serialize(stream, root, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
        }

        private void RunZip (string path) {
            var proc = Process.Start("TMExportZip.exe", $@"/V:RoboHenry /P:1qaz2wsx {path}");
        }
    }

    [XmlRoot(ElementName = "Root", Namespace = "")]
    public class Root {
        [XmlAttribute]
        public int PointCount { get; set; } = 10;
        [XmlAttribute]
        public int PvtPointCount { get; set; } = 0;
        [XmlAttribute]
        public int TaskCount { get; set; } = 0;
        [XmlAttribute]
        public int PvtTaskCount { get; set; } = 0;
        [XmlAttribute]
        public string ID { get; set; } = "4YErH6fuLFvAgY95YCex/78hy9XGDhoM6xXSgaRkKPnuxr2XJJ9nQqDiWJzZMtfms6AmH6GxTj6XBE+gHA8zoQ==";

        public string Version { get; set; } = "1.4";
        public Base Base { get; set; } = new Base();
        public TCP TCP { get; set; } = new TCP();
        [XmlElement("Point")]
        public List<Point> Point { get; set; } = new List<Point>();
    }

    public class Base {
        public string Name { get; set; } = "Base1";
        public string Data { get; set; } = "500,0,0,0,0,0";
        public string Type { get; set; } = "C";
    }

    public class TCP {
        public string Name { get; set; } = "NOTOOL";
        public string Description { get; set; } = string.Empty;
        public GPTFF GPTFF { get; set; } = new GPTFF();
        public float Mass { get; set; } = 0;
        public GPTFF MassCenter { get; set; } = new GPTFF();
        public Inertia Inertia { get; set; } = new Inertia();
        public GPTFF GPTCF { get; set; } = new GPTFF();
        public string Studio_tcp { get; set; } = string.Empty;
        public string Studio_stp { get; set; } = string.Empty;
    }

    public class GPTFF {
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;
        public float Z { get; set; } = 0;
        public float W { get; set; } = 0;
        public float V { get; set; } = 0;
        public float U { get; set; } = 0;
    }

    public class Inertia {
        public float Ixx { get; set; } = 0;
        public float Iyy { get; set; } = 0;
        public float Izz { get; set; } = 0;
    }

    public class Point {
        [XmlAttribute]
        public int Index { get; set; } = 0;
        public string Motion { get; set; } = "PLine";
        public string coordinate { get; set; } = "0,0,0,0,0,0";
        public string joint_angle { get; set; } = "0,0,0,0,0,0";
        public string tool_mode { get; set; } = "0,0,0,0,0,0";
        public string Blend { get; set; } = "YES";
        public int BlendValue { get; set; } = 50;
        public string LineABS { get; set; } = "ON";
        public int LSAVelocity { get; set; } = 100;
        public int LSTTTS { get; set; } = 150;
        public int PLSAVelocity { get; set; } = 100;
        public int PLSTTTS { get; set; } = 150;
        public int LSPercentage { get; set; } = 100;
        public string PSTTTSOF { get; set; } = "ON";
        public int PSTTTS { get; set; } = 250;
        public int PSPercentage { get; set; } = 100;
        public string Config { get; set; } = "0,2,4";

        public Point () {
        }

        public Point (XYZABC p, int speed, int index) {
            coordinate =
                    p.X.ToString("f4") + "," +
                    p.Y.ToString("f4") + "," +
                    p.Z.ToString("f4") + "," +
                    p.A.ToString("f4") + "," +
                    p.B.ToString("f4") + "," +
                    p.C.ToString("f4");
            tool_mode = coordinate;
            LSAVelocity = speed;
            this.Index = index;
        }
    }
}
