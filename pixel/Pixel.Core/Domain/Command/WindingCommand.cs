
using Pixel.Core.Enums;

namespace Pixel.Core.Domain.Command;

public struct WindingCommand: Core.Domain.IValueCommand<float>
{
    public CommandType Type => CommandType.Winding;

    public float Value { get; set; }
}

