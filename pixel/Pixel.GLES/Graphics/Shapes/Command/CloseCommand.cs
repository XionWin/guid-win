
using Pixel.Core.Enums;

namespace Pixel.GLES.Shapes.Command;

public struct CloseCommand: Core.Domain.ICommand
{
    public CommandType Type => CommandType.Close;
}

