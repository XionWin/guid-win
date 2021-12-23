
using System.Drawing;
using System.Numerics;

namespace Pixel.Core.Domain.Shape;

public struct Rectangle: IShape
{
    public RectangleF Rect { get; init; }
    public Geometry Geometry { get; init; }

    public Rectangle(float x, float y, float w, float h)
    {
        this.Rect = new RectangleF(x, y, w, h);
        this.Geometry = new Geometry(this.Rect.ToVector2());
    }
}

static class RectangleExtension
{
    public static IEnumerable<Vector2> ToVector2(this RectangleF rect) =>
        new []
        {
            new System.Numerics.Vector2(rect.X, rect.Y),
            new System.Numerics.Vector2(rect.X, rect.Y + rect.Height),
            new System.Numerics.Vector2(rect.X + rect.Width, rect.Y + rect.Height),
            new System.Numerics.Vector2(rect.X + rect.Width, rect.Y),
        };
}


