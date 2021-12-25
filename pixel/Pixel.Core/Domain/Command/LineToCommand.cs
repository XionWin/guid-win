
using Pixel.Core.Enums;

namespace Pixel.Core.Domain.Command;

public struct LineToCommand: Core.Domain.IValueCommand<System.Numerics.Vector3>
{
    public LineToCommand(float x, float y, float z)
    {
        this.Value = new System.Numerics.Vector3(x, y, z);
    }
    public CommandType Type => CommandType.LineTo;

    public System.Numerics.Vector3 Value { get; set; }
}

