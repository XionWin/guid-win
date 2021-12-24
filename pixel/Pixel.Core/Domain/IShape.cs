using System.Drawing;
using Pixel.Core.Domain;

namespace Pixel.Core.Domain;

public interface IShape
{
    Geometry Geometry { get; }
    
    public IBrush Fill { get; }
    
    public IBrush Stroke { get; }

    void Rotate(float rad);
    void Transform(PointF point);
}
