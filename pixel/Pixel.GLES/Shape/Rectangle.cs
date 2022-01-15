
using System.Drawing;
using System.Numerics;
using Pixel.Core.Domain;

namespace Pixel.GLES.Shape;

public class Rectangle: Shape
{
    public RectangleF Rect { get; init; }

    public override IEnumerable<ICommand> Commands =>
    new ICommand[]
    {
        new Core.Domain.Command.MoveToCommand(this.Rect.X, this.Rect.Y),
        new Core.Domain.Command.LineToCommand(this.Rect.X, this.Rect.Y + this.Rect.Height),
        new Core.Domain.Command.LineToCommand(this.Rect.X + this.Rect.Width, this.Rect.Y + this.Rect.Height),
        new Core.Domain.Command.LineToCommand(this.Rect.X + this.Rect.Width, this.Rect.Y),
        new Core.Domain.Command.CloseCommand(),
    };

    public Rectangle(float x, float y, float w, float h, bool is3D = false): base(is3D)
    {
        this.Rect = new RectangleF(x, y, w, h);
    }
}

static class RectangleExtension
{
    public static IEnumerable<Vector2> ToVector(this RectangleF rect) =>
        new []
        {
            new System.Numerics.Vector2(rect.X, rect.Y),
            new System.Numerics.Vector2(rect.X, rect.Y + rect.Height),
            new System.Numerics.Vector2(rect.X + rect.Width, rect.Y + rect.Height),
            new System.Numerics.Vector2(rect.X + rect.Width, rect.Y),
        };
}


