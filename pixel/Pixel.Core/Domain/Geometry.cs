using System.Drawing;
using System.Numerics;

namespace Pixel.Core.Domain;

public class Geometry
{
    private Matrix4x4 rMatrix = new Matrix4x4
    (
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1
    );
    private Matrix4x4 tMatrix = new Matrix4x4
    (
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1
    );
    public Geometry(IEnumerable<Vector3> points)
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

    IEnumerable<Vector3> Points { get; init; }
    public Matrix4x4 Matrix => this.rMatrix * this.tMatrix;


    public PointF Center { get; init; }
    public RectangleF Border { get; }

    private IEnumerable<Vector3> RenderPoints =>
        this.Matrix is Matrix4x4 mat ? this.Points.Select(x => Vector3.Transform(x, mat)) : throw new Exception();

    public IEnumerable<ICommand> Commands
    {
        get
        {
            var rect = this;
            IEnumerable<ICommand> renderVectors = this.RenderPoints.Select<Vector3, ICommand>(
                (x, i) => i == 0
                ? new Command.MoveToCommand(x.X, x.Y)
                : new Command.LineToCommand(x.X, x.Y)
            );
            return renderVectors.Append(new Command.CloseCommand());
        }
    }

    
    public void Rotate(float x, float y, float z)
    {
        this.rMatrix = 
        new Matrix4x4
        (
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            -this.Center.X, -this.Center.Y, 0 , 1
        ) *
        new Matrix4x4
        (
            1, 0, 0, 0,
            0, (float)Math.Cos(x), -(float)Math.Sin(x), 0,
            0, (float)Math.Sin(x), (float)Math.Cos(x), 0,
            0, 0, 0, 1
        ) *
        new Matrix4x4
        (
            (float)Math.Cos(y), 0, (float)Math.Sin(y), 0,
            0, 1, 0, 0,
            -(float)Math.Sin(y), 0, (float)Math.Cos(y), 0,
            0, 0, 0, 1
        ) *
        new Matrix4x4
        (
            (float)Math.Cos(z), -(float)Math.Sin(z), 0, 0,
            (float)Math.Sin(z), (float)Math.Cos(z), 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        ) *
        new Matrix4x4
        (
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            this.Center.X, this.Center.Y, 0, 1
        );
    }

    public void Transform(PointF point)
    {
        this.tMatrix = new Matrix4x4
        (
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            point.X, point.Y, 0, 1
        );
    }
}


