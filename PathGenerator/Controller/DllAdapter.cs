using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace Final.Controller {

    static class DllAdapter {
        [DllImport("gdi32")]
        public static extern IntPtr SwapBuffers(IntPtr hdc);

        [DllImport("gdi32")]
        public static extern int SetPixelFormat(int hDC, int n, ref PIXELFORMATDESCRIPTOR pcPixelFormatDescriptor);

        [DllImport("gdi32")]
        public static extern int ChoosePixelFormat(int hDC, ref PIXELFORMATDESCRIPTOR pPixelFormatDescriptor);

        [DllImport("opengl32")]
        public static extern void glReadPixels(int x, int y, int width, int height, uint format, uint type, ref float pixel);// void* pixels);

        [DllImport("opengl32.dll")]
        public static extern IntPtr wglCreateContext(IntPtr HDC);

        [DllImport("opengl32.dll")]
        public static extern bool wglMakeCurrent(IntPtr HDC, IntPtr HGLRC);

        [DllImport("user32")]
        public static extern IntPtr GetDC(IntPtr hwnd);
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct PIXELFORMATDESCRIPTOR {
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
}
