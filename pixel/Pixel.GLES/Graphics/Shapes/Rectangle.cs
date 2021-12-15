
using Pixel.Core.Domain;
using Pixel.Core.Enums;

namespace Pixel.GLES.Shapes;

public struct Rectangle: Core.Domain.IShape
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public IEnumerable<ICommand>? Commands => 
        new ICommand[]
        {
            new Command.MoveToCommand(this.X, this.Y),
            new Command.LineToCommand(this.X, this.Y + this.Height),
            new Command.LineToCommand(this.X + this.Width, this.Y + this.Height),
            new Command.LineToCommand(this.X + this.Width, this.Y),
            new Command.CloseCommand(),
        };
}

