using Pixel.Core.Enums;

namespace Pixel.Core.Domain;

public interface IValueCommand<T>: ICommand
{
    T Value { get; set; }
}
