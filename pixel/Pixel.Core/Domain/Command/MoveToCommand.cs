
using Pixel.Core.Enums;

namespace Pixel.Core.Domain.Command;

public struct MoveToCommand: Core.Domain.IValueCommand<System.Numerics.Vector2>
{
    public MoveToCommand(float x, float y)
    {
        this.Value = new System.Numerics.Vector2(x, y);
    }
    
    public CommandType Type => CommandType.MoveTo;

    public System.Numerics.Vector2 Value { get; set; }
}

