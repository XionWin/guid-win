using System.Drawing;
using System.Numerics;

namespace Pixel.Core.Domain;

public struct Geometry
{
    public Geometry(IEnumerable<Vector2> points)
    {
        this.Points = points;
        this.Center = new PointF(
            points.Min(x => x.X) + points.Max(x => x.X) / 2,
            points.Min(x => x.Y) + points.Max(x => x.Y) / 2
        );
        
        this.Border = new RectangleF(
            points.Min(x => x.X), 
            points.Min(x => x.Y),
            points.Max(x => x.X) - points.Min(x => x.X), 
            points.Max(x => x.Y) - points.Min(x => x.Y) 
        );
    }

    IEnumerable<Vector2> Points { get; init; }
    System.Numerics.Matrix4x4 Matrix { get; set; }  = new Matrix4x4
    (
        1, 0, 0, 0,
        0, 1 ,0, 0,
        0, 0 ,1, 0,
        0, 0 ,0, 1
    );

    PointF Center { get; init; }
    RectangleF Border { get; }

    private IEnumerable<Vector2> RenderPoints =>
        this.Matrix is Matrix4x4 mat ? this.Points.Select(x => Vector2.Transform(x, mat)) : throw new Exception();

    public IEnumerable<ICommand> Commands
    {
        get
        {
            var rect = this;
            IEnumerable<ICommand> renderVectors = this.RenderPoints.Select<Vector2, ICommand>(
                (x, i) => i == 0
                ? new Command.MoveToCommand(x.X, x.Y)
                : new Command.LineToCommand(x.X, x.Y)
            );
            return renderVectors.Append(new Command.CloseCommand());
        }
    }
}

static class GeometryExtension
{
    public static Vector3 ToVector3(this PointF point) =>
        new Vector3(point.X, point.Y, 0);
}

