using System.Runtime.InteropServices;

namespace Pixel.Core.Domain;

[StructLayout(LayoutKind.Sequential)]
public struct Color<T>
{
    public T r;
    public T g;
    public T b;
    public T a;

    public Color(T r, T g, T b, T a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }


    public static Color<T> From(IEnumerable<T> values)
    {
        return values.ToArray() is var array && array.Length == 4 ? new Color<T>(array[0], array[1], array[2], array[3]) : throw new Exception(); 
    }
    public T[] GetData => new T[]{r, g, b, a};
}