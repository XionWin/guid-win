
using System.Drawing;
using System.Numerics;
using Pixel.Core.Domain;

namespace Pixel.GLES.Shape;

public abstract class Shape: IShape
{
    public abstract IEnumerable<ICommand> Commands { get; }
    public IBrush Fill { get; set; } = Brushes.SolidColorBursh.Default;
    public IBrush Stroke { get; set; } = Brushes.SolidColorBursh.Default;
    public Vector3 Rotate { get; set; }
    public Vector2 Transform { get; set; }
    public bool Is3D { get; protected set; }

    public Shape(bool is3D = false)
    {
        this.Is3D = is3D;
    }

}



