using Pixel.Core.Domain;

namespace Pixel;

public class PixelEngine
{
    public Core.Domain.ISurface? Surface { get; init; }
    public Core.Domain.IGraphic? Graphic { get; init; }

    public PixelEngine(Core.Domain.ISurface surface, Core.Domain.IGraphic graphic)
    {
        this.Surface = surface;
        this.Graphic = graphic;
        surface.OnInit += this.Graphic.Render.OnInit;
        surface.OnRender += this.Graphic.Render.OnRender;
        surface.OnSizeChange += this.Graphic.Render.OnSizeChange;
        surface.OnEnd += this.Graphic.Render.OnEnd;
    }

    public void Start() => this.Surface?.Start();
}
