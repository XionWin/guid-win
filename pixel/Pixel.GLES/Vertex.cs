using System.Runtime.InteropServices;

namespace Pixel.GLES;

[StructLayout(LayoutKind.Sequential)]
public struct Vertex
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    public float u { get; set; }
    public float v { get; set; }

    public Vertex(float x, float y, float z, float u, float v)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.u = u;
        this.v = v;
    }
}