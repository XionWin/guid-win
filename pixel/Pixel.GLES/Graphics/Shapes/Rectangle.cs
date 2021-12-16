
using System.Numerics;
using Pixel.Core.Domain;
using Pixel.Core.Domain.Matrix;
using Pixel.Core.Enums;

namespace Pixel.GLES.Shapes;

public struct Rectangle: Core.Domain.IShape
{
    public Core.Domain.Rect Rect { get; set; }

    public Matrix3x2 Matrix { get; set; } = new Matrix3x2(1, 0, 0, 1, 0 ,0);

    
    public Rectangle(float x, float y, float w, float h)
    {
        this.Rect = new Rect(x, y, w, h);
    }

    public IEnumerable<ICommand> Commands => 
        new ICommand[]
        {
            Transform(this.Rect.TopLeft, this.Matrix) is Point topLeft ? new Command.MoveToCommand(topLeft.X, topLeft.Y) : throw new Exception(),
            Transform(this.Rect.BottomLeft, this.Matrix) is Point bottomLeft ? new Command.LineToCommand(bottomLeft.X, bottomLeft.Y) : throw new Exception(),
            Transform(this.Rect.BottomRight, this.Matrix) is Point bottomRight ? new Command.LineToCommand(bottomRight.X, bottomRight.Y) : throw new Exception(),
            Transform(this.Rect.TopRight, this.Matrix) is Point topRight ? new Command.LineToCommand(topRight.X, topRight.Y) : throw new Exception(),
            new Command.CloseCommand(),
        };

    public System.Drawing.PointF Center => new System.Drawing.PointF(this.Rect.X + this.Rect.Width / 2, this.Rect.Y + this.Rect.height / 2);

    private Core.Domain.Point Transform(Core.Domain.Point point, Matrix3x2 matrix)
    {
        var rad = (float)(DateTime.Now.Millisecond % 360f / 180f * Math.PI);

        point = Transform2(point, new Matrix3x2(1, 0, 0, 1, -this.Center.X, -this.Center.Y));
        point = Transform2(point, new Matrix3x2((float)Math.Cos(rad), (float)Math.Sin(rad), -(float)Math.Sin(rad), (float)Math.Cos(rad), 0, 0));
        point = Transform2(point, new Matrix3x2(1, 0, 0, 1, this.Center.X, this.Center.Y));

        return point;
    }

    private Core.Domain.Point Transform2(Core.Domain.Point point, Matrix3x2 matrix)
    {
        return new Point
        (
            point.X * matrix.M11 + point.Y * matrix.M21 + matrix.M31,
            point.X * matrix.M12 + point.Y * matrix.M22 + matrix.M32
        );
    }

}

