namespace Pixel.Core.Domain;

public interface IShape
{
    IEnumerable<ICommand>? Commands { get; }
}
