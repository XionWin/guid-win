using System.Drawing;
using Pixel.Core.Domain;

namespace Pixel.Core.Domain;

public interface IShape
{
    Geometry Geometry { get; }
    
    public IBrush Fill { get; }
    
    public IBrush Stroke { get; }

    void Rotate(float z, float x = 0, float y = 0);
    void Transform(PointF point);
}
