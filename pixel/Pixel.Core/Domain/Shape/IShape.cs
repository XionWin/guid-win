using System.Drawing;

namespace Pixel.Core.Domain.Shape;

public interface IShape
{
    Geometry Geometry { get; }

    void Rotate(float rad);
    void Transform(PointF point);
}
