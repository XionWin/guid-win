
using Pixel.Core.Enums;

namespace Pixel.Core.Domain.Command;

public struct LineToCommand: Core.Domain.IValueCommand<System.Numerics.Vector2>
{
    public LineToCommand(float x, float y)
    {
        this.Value = new System.Numerics.Vector2(x, y);
    }
    public CommandType Type => CommandType.LineTo;

    public System.Numerics.Vector2 Value { get; set; }
}

