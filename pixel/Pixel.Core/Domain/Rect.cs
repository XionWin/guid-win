namespace Pixel.Core.Domain;

public struct Rect
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float height { get; set; }

    public Rect(float x, float y, float w, float h)
    {
        this.X = x;
        this.Y = y;
        this.Width = w;
        this.height = h;
    }

    public Point TopLeft => new Point(this.X, this.Y);
    public Point TopRight => new Point(this.X + this.Width, this.Y);
    public Point BottomLeft => new Point(this.X, this.Y + this.height);
    public Point BottomRight => new Point(this.X + this.Width, this.Y + this.height);
}