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
        if (this.Graphic.Render is IRender<T> render)
        {
            surface.OnInit += render.OnInit;
            surface.OnRender += render.OnRender;
            surface.OnSizeChange += render.OnSizeChange;
            surface.OnEnd += render.OnEnd;
        }
    }

    public void Start() => this.Surface?.Start();
}
