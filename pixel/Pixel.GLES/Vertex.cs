using System.Runtime.InteropServices;

namespace Pixel.GLES;

[StructLayout(LayoutKind.Sequential)]
public struct Vertex
{
    public float x { get; set; }
    public float y { get; set; }
    public float u { get; set; }
    public float v { get; set; }

    public static List<Vertex> Defalut = 
    new List<Vertex>() {
        new Vertex(){
            x = 0f,
            y = 0f,
            u = 0f,
            v = 1f,
        },
        new Vertex(){
            x = 0f,
            y = 320f,
            u = 1f,
            v = 1f,
        },
        new Vertex(){
            x = 1280f,
            y = 0f,
            u = 1f,
            v = 0f,
        },
    };
}