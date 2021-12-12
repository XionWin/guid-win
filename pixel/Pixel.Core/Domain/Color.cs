using System.Runtime.InteropServices;

namespace Pixel.Core.Domain;

[StructLayout(LayoutKind.Sequential)]
public struct Color
{
    public byte r;
    public byte g;
    public byte b;
    public byte a;

    public Color(byte r, byte g, byte b, byte a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }
}