namespace Pixel.Core.Domain;

public interface IShape
{
    Matrix.Matrix2x3 matrix { get; set; }
    IEnumerable<ICommand> Commands { get; }
}
