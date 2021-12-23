using System.Drawing;
using System.Numerics;

namespace Pixel.Core.Domain;

public class Geometry
{
    private Matrix3x2 rMatrix = new Matrix3x2
    (
        1, 0,
        0, 1,
        0, 0
    );
    private Matrix3x2 tMatrix = new Matrix3x2
    (
        1, 0,
        0, 1,
        0, 0
    );
    public Geometry(IEnumerable<Vector2> points)
    {
        this.Points = points;
        this.Center = new PointF(
            (points.Min(x => x.X) + points.Max(x => x.X)) / 2,
            (points.Min(x => x.Y) + points.Max(x => x.Y)) / 2
        );
        
        this.Border = new RectangleF(
            points.Min(x => x.X), 
            points.Min(x => x.Y),
            points.Max(x => x.X) - points.Min(x => x.X), 
            points.Max(x => x.Y) - points.Min(x => x.Y) 
        );
    }

    IEnumerable<Vector2> Points { get; init; }
    public Matrix3x2 Matrix => this.rMatrix * this.tMatrix;


    public PointF Center { get; init; }
    public RectangleF Border { get; }

    private IEnumerable<Vector2> RenderPoints =>
        this.Matrix is Matrix3x2 mat ? this.Points.Select(x => Vector2.Transform(x, mat)) : throw new Exception();

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

    
    public void Rotate(float rad)
    {
        this.rMatrix = 
        new Matrix3x2
        (
            1, 0,
            0, 1,
            -this.Center.X, -this.Center.Y
        ) *
        new Matrix3x2
        (
            (float)Math.Cos(rad), -(float)Math.Sin(rad),
            (float)Math.Sin(rad), (float)Math.Cos(rad),
            0, 0
        ) *
        new Matrix3x2
        (
            1, 0,
            0, 1,
            this.Center.X, this.Center.Y
        );
    }

    public void Transform(PointF point)
    {
        this.tMatrix = new Matrix3x2
        (
            1, 0,
            0, 1,
            point.X, point.Y
        );
    }
}


