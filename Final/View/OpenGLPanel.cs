using Final.Model;
using System;
using OpenTK;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tao.OpenGl;
using Point = System.Drawing.Point;

namespace Final.View {
    class OpenGLPanel : Panel {
        public Action<int, int> SelectEvent;
        Vector3 cmrCenter = new Vector3(0, 0, 0);
        Vector3 cmrX = new Vector3(-1, 0, 0);
        Vector3 cmrY = new Vector3(0, 1, 0);
        IntPtr hdc, hrc;
        Point CurrentRotate = new Point();
        Point CurrentTranslate = new Point();
        double scale = 2;
        int Mouse_sensitive = 1;

        public OpenGLPanel () {
            MouseDown += OpenGLPanel_MouseDown;
            MouseMove += OpenGLPanel_MouseMove;
            MouseWheel += OpenGLPanel_MouseWheel;
            SizeChanged += OpenGLPanel_SizeChanged;
            InitOpenGL();
        }

        public void Render () {
            #region Control
            Gl.glFlush();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            Gl.glOrtho(-Width / 2, Width / 2, -Height / 2, Height / 2, -20000, 10000);
            SetCameraPosition();
            Gl.glScaled(scale, scale, scale);
            #endregion

            #region Axis
            Gl.glColor3f(1, 1, 1);
            //Glu.gluSphere(Glu.gluNewQuadric(), 3, 36, 36);
            Gl.glLineWidth(3);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glColor3f(1, 0, 0);
            Gl.glVertex3f(20, 0, 0);
            Gl.glVertex3f(0, 0, 0);
            Gl.glColor3f(0, 0, 1);
            Gl.glVertex3f(0, 0, 20);
            Gl.glVertex3f(0, 0, 0);
            Gl.glColor3f(0, 1, 0);
            Gl.glVertex3f(0, 20, 0);
            Gl.glVertex3f(0, 0, 0);
            Gl.glEnd();
            //Gl.glBegin(Gl.GL_QUADS);
            //Gl.glColor3f(1, 1, 1);
            //Gl.glVertex3f(1000, 1000, -200);
            //Gl.glVertex3f(-1000, 1000, -200);
            //Gl.glVertex3f(-1000, -1000, -200);
            //Gl.glVertex3f(1000, -1000, -200);
            //Gl.glEnd();
            #endregion

            #region List   
            Gl.glColor3d(0.5, 0.6, 0.6);
            Gl.glCallList((int)GlList.Workpiece);
            Gl.glCallList((int)GlList.Path);
            Gl.glCallList((int)GlList.GridBall);
            Gl.glColor3ub(40, 40, 40);
            Gl.glCallList((int)GlList.Machine);
            #endregion
        }

        public void SwapBuffer () {
            SwapBuffers(hdc);
        }

        private void SetCameraPosition () {
            var cmrZ = Vector3.Cross(cmrX, cmrY);
            var cmrPosition = cmrCenter - 500 * cmrZ;
            Glu.gluLookAt(cmrPosition.X, cmrPosition.Y, cmrPosition.Z, cmrCenter.X, cmrCenter.Y, cmrCenter.Z, cmrY.X, cmrY.Y, cmrY.Z);
        }

        private void OpenGLPanel_MouseMove (object sender, MouseEventArgs e) {
            switch (e.Button) {
                case MouseButtons.Left:
                    break;
                case MouseButtons.None:
                    break;
                case MouseButtons.Right:
                    var x = Matrix4.CreateFromAxisAngle(cmrY, (CurrentRotate.X - e.X) / 200f / Mouse_sensitive);
                    var y = Matrix4.CreateFromAxisAngle(cmrX, -(CurrentRotate.Y - e.Y) / 200f / Mouse_sensitive);
                    cmrY = Vector3.Transform(cmrY, y);
                    cmrX = Vector3.Transform(cmrX, x);
                    CurrentRotate = e.Location;
                    break;
                case MouseButtons.Middle:
                    cmrCenter -= cmrX * ((CurrentTranslate.X - e.X) / Mouse_sensitive);
                    cmrCenter -= cmrY * ((CurrentTranslate.Y - e.Y) / Mouse_sensitive);
                    CurrentTranslate = e.Location;
                    break;
                case MouseButtons.XButton1:
                    break;
                case MouseButtons.XButton2:
                    break;
                default:
                    break;
            }
        }

        private void OpenGLPanel_MouseDown (object sender, MouseEventArgs e) {
            switch (e.Button) {
                case MouseButtons.Left:
                    SelectEvent(e.X, e.Y);
                    break;
                case MouseButtons.None:
                    break;
                case MouseButtons.Right:
                    CurrentRotate = e.Location;
                    break;
                case MouseButtons.Middle:
                    CurrentTranslate = e.Location;
                    break;
                case MouseButtons.XButton1:
                    break;
                case MouseButtons.XButton2:
                    break;
                default:
                    break;
            }
        }

        private void OpenGLPanel_MouseWheel (object sender, MouseEventArgs e) {
            if (scale > 0.1) {
                if (e.Delta > 0)
                    scale *= 1.05;
                else
                    scale *= 0.95;
                if (scale < 0.1)
                    scale = 0.11;
            }
        }

        private void OpenGLPanel_SizeChanged (object sender, EventArgs e) {
            Gl.glViewport(0, 0, Width, Height);
        }

        private void InitOpenGL () {
            hdc = GetDC(Handle);
            SetPixelFormatDescriptor();
            hrc = wglCreateContext(hdc);
            if (wglMakeCurrent(hdc, hrc) == false) {
                MessageBox.Show("Initial Fail!!");
                return;
            }
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            //Gl.glEnable(Gl.GL_CULL_FACE);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);
            Gl.glColorMaterial(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT_AND_DIFFUSE);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glClearColor(0.9f, 0.9f, 0.9f, 1.0f);

            SetLight(0, 0, 1000, 1000);
            SetLight(1, 500, -500, 1000);
            SetLight(2, -500, -500, 1000);
        }

        private void SetLight (int Light, int x, int y, int z) {
            Gl.glLightfv(Gl.GL_LIGHT0 + Light, Gl.GL_AMBIENT, new float[4] { 0.1f, 0.1f, 0.1f, 1.0f });
            Gl.glLightfv(Gl.GL_LIGHT0 + Light, Gl.GL_DIFFUSE, new float[4] { 0.2f, 0.2f, 0.2f, 1.0f });
            Gl.glLightfv(Gl.GL_LIGHT0 + Light, Gl.GL_SPECULAR, new float[4] { 0.2f, 0.2f, 0.2f, 1.0f });
            Gl.glLightfv(Gl.GL_LIGHT0 + Light, Gl.GL_POSITION, new float[4] { x, y, z, 0f });
            Gl.glLightModeli(Gl.GL_LIGHT_MODEL_TWO_SIDE, Gl.GL_TRUE);
            Gl.glEnable(Gl.GL_LIGHT0 + Light);
        }

        private void SetPixelFormatDescriptor () {
            const uint PFD_DOUBLEBUFFER = 0x00000001;
            const uint PFD_STEREO = 0x00000002;
            const uint PFD_DRAW_TO_WINDOW = 0x00000004;
            const uint PFD_DRAW_TO_BITMAP = 0x00000008;
            const uint PFD_SUPPORT_GDI = 0x00000010;
            const uint PFD_SUPPORT_OPENGL = 0x00000020;
            const uint PFD_GENERIC_FORMAT = 0x00000040;
            const uint PFD_NEED_PALETTE = 0x00000080;
            const uint PFD_NEED_SYSTEM_PALETTE = 0x00000100;
            const uint PFD_SWAP_EXCHANGE = 0x00000200;
            const uint PFD_SWAP_COPY = 0x00000400;
            const uint PFD_SWAP_LAYER_BUFFERS = 0x00000800;
            const uint PFD_GENERIC_ACCELERATED = 0x00001000;
            const uint PFD_SUPPORT_DIRECTDRAW = 0x00002000;
            const byte PFD_TYPE_RGBA = 0;
            const byte PFD_MAIN_PLANE = 0;

            PIXELFORMATDESCRIPTOR pfd = new PIXELFORMATDESCRIPTOR();
            pfd.nSize = (ushort)Marshal.SizeOf(pfd);
            pfd.nVersion = 1;
            pfd.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER;
            pfd.iPixelType = PFD_TYPE_RGBA;
            pfd.cColorBits = 24;
            pfd.cRedBits = 0;
            pfd.cRedShift = 0;
            pfd.cGreenBits = 0;
            pfd.cGreenShift = 0;
            pfd.cBlueBits = 0;
            pfd.cBlueShift = 0;
            pfd.cAlphaBits = 0;
            pfd.cAlphaShift = 0;
            pfd.cAccumBits = 0;
            pfd.cAccumRedBits = 0;
            pfd.cAccumGreenBits = 0;
            pfd.cAccumBlueBits = 0;
            pfd.cAccumAlphaBits = 0;
            pfd.cDepthBits = 32;
            pfd.cStencilBits = 0;
            pfd.cAuxBuffers = 0;
            pfd.iLayerType = PFD_MAIN_PLANE;
            pfd.bReserved = 0;
            pfd.dwLayerMask = 0;
            pfd.dwVisibleMask = 0;
            pfd.dwDamageMask = 0;
            int PixelFormat = ChoosePixelFormat((int)hdc, ref pfd);
            SetPixelFormat((int)hdc, PixelFormat, ref pfd);
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct PIXELFORMATDESCRIPTOR {
            [FieldOffset(0)]
            public ushort nSize;
            [FieldOffset(2)]
            public ushort nVersion;
            [FieldOffset(4)]
            public uint dwFlags;
            [FieldOffset(8)]
            public byte iPixelType;
            [FieldOffset(9)]
            public byte cColorBits;
            [FieldOffset(10)]
            public byte cRedBits;
            [FieldOffset(11)]
            public byte cRedShift;
            [FieldOffset(12)]
            public byte cGreenBits;
            [FieldOffset(13)]
            public byte cGreenShift;
            [FieldOffset(14)]
            public byte cBlueBits;
            [FieldOffset(15)]
            public byte cBlueShift;
            [FieldOffset(16)]
            public byte cAlphaBits;
            [FieldOffset(17)]
            public byte cAlphaShift;
            [FieldOffset(18)]
            public byte cAccumBits;
            [FieldOffset(19)]
            public byte cAccumRedBits;
            [FieldOffset(20)]
            public byte cAccumGreenBits;
            [FieldOffset(21)]
            public byte cAccumBlueBits;
            [FieldOffset(22)]
            public byte cAccumAlphaBits;
            [FieldOffset(23)]
            public byte cDepthBits;
            [FieldOffset(24)]
            public byte cStencilBits;
            [FieldOffset(25)]
            public byte cAuxBuffers;
            [FieldOffset(26)]
            public byte iLayerType;
            [FieldOffset(27)]
            public byte bReserved;
            [FieldOffset(28)]
            public uint dwLayerMask;
            [FieldOffset(32)]
            public uint dwVisibleMask;
            [FieldOffset(36)]
            public uint dwDamageMask;
        }

        [DllImport("gdi32")]
        private static extern IntPtr SwapBuffers (IntPtr hdc);

        [DllImport("gdi32")]
        private static extern int SetPixelFormat (int hDC, int n, ref PIXELFORMATDESCRIPTOR pcPixelFormatDescriptor);

        [DllImport("gdi32")]
        private static extern int ChoosePixelFormat (int hDC, ref PIXELFORMATDESCRIPTOR pPixelFormatDescriptor);

        [DllImport("opengl32.dll")]
        private static extern IntPtr wglCreateContext (IntPtr HDC);

        [DllImport("opengl32.dll")]
        private static extern bool wglMakeCurrent (IntPtr HDC, IntPtr HGLRC);

        [DllImport("user32")]
        private static extern IntPtr GetDC (IntPtr hwnd);

    }
}
