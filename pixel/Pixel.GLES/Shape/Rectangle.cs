
using System.Drawing;
using System.Numerics;
using Pixel.Core.Domain;

namespace Pixel.GLES.Shape;

public class Rectangle: IShape
{
    public RectangleF Rect { get; init; }
    public Geometry Geometry { get; init; }
    public IBrush Fill { get; set; } = Brushes.SolidColorBursh.Default;
    public IBrush Stroke { get; set; } = Brushes.SolidColorBursh.Default;

    public Rectangle(float x, float y, float w, float h)
    {
        this.Rect = new RectangleF(x, y, w, h);
        this.Geometry = new Geometry(this.Rect.ToVector2());
    }

    public void Rotate(float rad)
    {
        this.Geometry.Rotate(rad);
    }

    public void Transform(PointF point)
    {
        this.Geometry.Transform(point);
    }

    public (Vector2 topLeft, Vector2 bottomLeft, Vector2 bottomRight, Vector2 topRight) GetRenderRect() =>
        (
            Vector2.Transform(new Vector2(Rect.X, Rect.Y), this.Geometry.Matrix),
            Vector2.Transform(new Vector2(Rect.X, Rect.Y + Rect.Height), this.Geometry.Matrix),
            Vector2.Transform(new Vector2(Rect.X + Rect.Width, Rect.Y + Rect.Height), this.Geometry.Matrix),
            Vector2.Transform(new Vector2(Rect.X + Rect.Width, Rect.Y), this.Geometry.Matrix)
        );
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


