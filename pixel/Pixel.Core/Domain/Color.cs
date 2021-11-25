namespace Pixel.Core.Domain;

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
}