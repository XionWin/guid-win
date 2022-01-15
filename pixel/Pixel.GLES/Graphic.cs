using System.Numerics;
using Pixel.Core.Domain;

namespace Pixel.GLES;

public partial class Graphic : Pixel.Core.Domain.IGraphic
{
    public IRender Render { get; init; }
    public Dictionary<IShape, IEnumerable<IGeometry>> ShapeGeometries { get; set; } = new Dictionary<IShape, IEnumerable<IGeometry>>();

    public IEnumerable<IShape> Shapes => this.ShapeGeometries.Keys;
    public IEnumerable<IGeometry> Geometries => this.ShapeGeometries.Values.SelectMany(x => x);

    public Graphic()
    {
        this.Render = new Render(this);
    }

    public void Add(IShape shape)
    {
        if(this.ShapeGeometries.Keys.Contains(shape) is false)
        {
            this.ShapeGeometries.Add(shape, shape.ToGeometries());
        }
    }
}

static class GraphicExtension
{
    public static IEnumerable<IGeometry> ToGeometries(this IShape shape) =>
        new [] {new Geometry(shape.Commands.ToPoints(), shape.Is3D, shape.Rotate, shape.Transform)};
    private static IEnumerable<Vector2> ToPoints(this IEnumerable<ICommand> commands) =>
        commands.Select(x => x switch
        {
            Pixel.Core.Domain.Command.MoveToCommand moveTo => new Vector2(moveTo.Value.X, moveTo.Value.Y),
            Pixel.Core.Domain.Command.LineToCommand lineTo => new Vector2(lineTo.Value.X, lineTo.Value.Y),
            Pixel.Core.Domain.Command.CloseCommand _ => commands.First(x => x is Pixel.Core.Domain.Command.MoveToCommand) is Pixel.Core.Domain.Command.MoveToCommand moveTo
                ? new Vector2(moveTo.Value.X, moveTo.Value.Y)
                : throw new Exception("Move to Point not found"),
            _ => throw new Exception("Unknown command")
        });
}