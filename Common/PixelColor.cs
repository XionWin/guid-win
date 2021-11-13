namespace Common;

public struct PixelColor
{
    public float r;
    public float g;
    public float b;
    public float a;

    public PixelColor(float r, float g, float b, float a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public PixelColor(float r, float g, float b): this(r, g, b, 1f)
    {}
}