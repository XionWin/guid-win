
using Pixel.Core.Enums;

namespace Pixel.GLES.Shapes.Command;

public struct BezierToCommand: Core.Domain.IValueCommand<(System.Drawing.PointF, System.Drawing.PointF, System.Drawing.PointF)>
{
    public CommandType Type => CommandType.BezierTo;

    public (System.Drawing.PointF, System.Drawing.PointF, System.Drawing.PointF) Value { get; set; }
}

