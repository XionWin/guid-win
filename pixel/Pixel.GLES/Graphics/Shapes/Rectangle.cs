
using Pixel.Core.Domain;
using Pixel.Core.Domain.Matrix;
using Pixel.Core.Enums;

namespace Pixel.GLES.Shapes;

public struct Rectangle: Core.Domain.IShape
{
    public Core.Domain.Rect Rect { get; set; }

    static double angle = 10f / 180f * Math.PI;
    public Matrix2x3 matrix { get; set; } = new Matrix2x3((float)Math.Cos(angle), -(float)Math.Sin(angle), 0, (float)Math.Sin(angle), (float)Math.Cos(angle), 0);

    
    public Rectangle(float x, float y, float w, float h)
    {
        this.Rect = new Rect(x, y, w, h);
    }

    public IEnumerable<ICommand> Commands => 
        new ICommand[]
        {
            Transform(this.Rect.TopLeft, this.matrix) is Point topLeft ? new Command.MoveToCommand(topLeft.X, topLeft.Y) : throw new Exception(),
            Transform(this.Rect.BottomLeft, this.matrix) is Point bottomLeft ? new Command.LineToCommand(bottomLeft.X, bottomLeft.Y) : throw new Exception(),
            Transform(this.Rect.BottomRight, this.matrix) is Point bottomRight ? new Command.LineToCommand(bottomRight.X, bottomRight.Y) : throw new Exception(),
            Transform(this.Rect.TopRight, this.matrix) is Point topRight ? new Command.LineToCommand(topRight.X, topRight.Y) : throw new Exception(),
            new Command.CloseCommand(),
        };
    
    private Core.Domain.Point Transform(Core.Domain.Point point, Matrix2x3 matrix)
    {
        return new Point
        (
            point.X * matrix.M11 + point.Y * matrix.M12 + matrix.M13,
            point.X * matrix.M21 + point.Y * matrix.M22 + matrix.M23
        );
    }

}

