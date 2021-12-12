
using Pixel.Core.Enums;

namespace Pixel.GLES.Shapes;

public struct Command<T>
{
    public CommandType Type { get; set; }

    public T Value { get; set; }
}

