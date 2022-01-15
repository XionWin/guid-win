using System.Drawing;
using System.Numerics;

namespace Pixel.Core.Domain;

public interface IGeometry
{
    IEnumerable<Vector2> Points { get; }
    PointF Center { get; }
    RectangleF Bound { get; }
    Matrix4x4 Matrix { get; }
    bool Is3D { get; }
}
