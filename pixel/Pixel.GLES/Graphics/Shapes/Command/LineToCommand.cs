
using Pixel.Core.Enums;

namespace Pixel.GLES.Shapes.Command;

public struct LineToCommand: Core.Domain.IValueCommand<System.Drawing.PointF>
{
    public LineToCommand(float x, float y)
    {
        this.Value = new System.Drawing.PointF(x, y);
    }
    public CommandType Type => CommandType.LineTo;

    public System.Drawing.PointF Value { get; set; }
}

