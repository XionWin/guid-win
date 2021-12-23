
using Pixel.Core.Enums;

namespace Pixel.Core.Domain.Command;

public struct CloseCommand: Core.Domain.ICommand
{
    public CommandType Type => CommandType.Close;
}

