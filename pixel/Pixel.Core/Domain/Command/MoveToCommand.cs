
using Pixel.Core.Enums;

namespace Pixel.Core.Domain.Command;

public struct MoveToCommand: Core.Domain.IValueCommand<System.Drawing.PointF>
{
    public MoveToCommand(float x, float y)
    {
        this.Value = new System.Drawing.PointF(x, y);
    }
    
    public CommandType Type => CommandType.MoveTo;

    public System.Drawing.PointF Value { get; set; }
}

