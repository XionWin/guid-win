using Pixel.Core.Enums;

namespace Pixel.Core.Domain;

public interface ICommand
{
    CommandType Type { get; }
}
