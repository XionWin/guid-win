namespace Pixel.Core.Domain;

public interface IShape
{
    System.Numerics.Matrix3x2 Matrix { get; set; }
    System.Drawing.PointF Center { get; }
    
    IEnumerable<ICommand> Commands { get; }
}
