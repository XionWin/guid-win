namespace Pixel.Core.Domain;

public struct Rect<T>
{
    public T x;
    public T y;
    public T w;
    public T h;

    public Rect(T x, T y, T w, T h)
    {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
    }
}