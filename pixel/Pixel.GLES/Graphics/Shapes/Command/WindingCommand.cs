
using Pixel.Core.Enums;

namespace Pixel.GLES.Shapes.Command;

public struct WindingCommand: Core.Domain.IValueCommand<float>
{
    public CommandType Type => CommandType.Winding;

    public float Value { get; set; }
}

