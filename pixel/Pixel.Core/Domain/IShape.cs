using System.Drawing;
using System.Numerics;
using Pixel.Core.Domain;

namespace Pixel.Core.Domain;

public interface IShape
{
    IEnumerable<ICommand> Commands { get; }
    IBrush Fill { get; }
    IBrush Stroke { get; }
    public Vector3 Rotate { get; }
    public Vector2 Transform { get; }
    bool Is3D { get; }
}
