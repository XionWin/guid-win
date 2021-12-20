
using System.Drawing;
using System.Numerics;
using Pixel.Core.Domain;
using Pixel.GLES.Extensions;

namespace Pixel.GLES.Shapes;

public struct Rectangle: IShape
{
    public RectangleF Rect { get; set; }

    public Matrix3x2 Matrix { get; set; } = new Matrix3x2(1, 0, 0, 1, 0 ,0);

    public Rectangle(float x, float y, float w, float h)
    {
        this.Rect = new RectangleF(x, y, w, h);
        this.Angle = 0;
    }


    public IEnumerable<ICommand> Commands => 
        new ICommand[]
        {
            Transform(this.Rect.TopLeft(), this.Matrix) is PointF topLeft ? new Command.MoveToCommand(topLeft.X, topLeft.Y) : throw new Exception(),
            Transform(this.Rect.BottomLeft(), this.Matrix) is PointF bottomLeft ? new Command.LineToCommand(bottomLeft.X, bottomLeft.Y) : throw new Exception(),
            Transform(this.Rect.BottomRight(), this.Matrix) is PointF bottomRight ? new Command.LineToCommand(bottomRight.X, bottomRight.Y) : throw new Exception(),
            Transform(this.Rect.TopRight(), this.Matrix) is PointF topRight ? new Command.LineToCommand(topRight.X, topRight.Y) : throw new Exception(),

            
            // this.Rect.TopLeft() is PointF topLeft ? new Command.MoveToCommand(topLeft.X, topLeft.Y) : throw new Exception(),
            // this.Rect.BottomLeft() is PointF bottomLeft ? new Command.LineToCommand(bottomLeft.X, bottomLeft.Y) : throw new Exception(),
            // this.Rect.BottomRight() is PointF bottomRight ? new Command.LineToCommand(bottomRight.X, bottomRight.Y) : throw new Exception(),
            // this.Rect.TopRight() is PointF topRight ? new Command.LineToCommand(topRight.X, topRight.Y) : throw new Exception(),
            new Command.CloseCommand(),
        };

    public System.Drawing.PointF Center => this.Rect.Center();
    public System.Drawing.PointF Topleft => Transform(this.Rect.TopLeft(), this.Matrix);
    public System.Drawing.PointF TopRight => Transform(this.Rect.TopRight(), this.Matrix);
    public System.Drawing.PointF BottomLeft => Transform(this.Rect.BottomLeft(), this.Matrix);
    public System.Drawing.PointF BottomRight => Transform(this.Rect.BottomRight(), this.Matrix);

    public float Angle { get; set; }
    public PointF Transform(PointF point, Matrix3x2 matrix)
    {
        var rad = (float)(this.Angle / 180f * Math.PI);
        var mat = new Matrix3x2(1, 0, 0, 1, -this.Center.X, -this.Center.Y) 
        * new Matrix3x2((float)Math.Cos(rad), (float)Math.Sin(rad), -(float)Math.Sin(rad), (float)Math.Cos(rad), 0, 0)
        * new Matrix3x2(1, 0, 0, 1, this.Center.X, this.Center.Y);

        point = Transform2(point, mat);
        return point;
    }

    private PointF Transform2(PointF point, Matrix3x2 matrix) =>
        point.Mulitple(matrix);

}
public static class MatrixExtension
{
    public static PointF Mulitple(this PointF point, Matrix3x2 matrix) =>
    new PointF
    (
        point.X * matrix.M11 + point.Y * matrix.M21 + matrix.M31,
        point.X * matrix.M12 + point.Y * matrix.M22 + matrix.M32
    );
}

