using Pixel.Core.Domain;

namespace Pixel;

public class PixelEngine<T>
{
    public Core.Domain.ISurface? Surface { get; init; }
    public Core.Domain.IGraphic<T>? Graphic { get; init; }

    public PixelEngine(Core.Domain.ISurface surface, Core.Domain.IGraphic<T> graphic)
    {
        this.Surface = surface;
        this.Graphic = graphic;

        surface.OnInit += this.Graphic.OnInit;
        surface.OnRender += this.Graphic.OnRender;
        surface.OnSizeChange += this.Graphic.OnSizeChange;
        surface.OnEnd += this.Graphic.OnEnd;
    }

    public void Start() => this.Surface?.Start();
}
