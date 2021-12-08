using Pixel.Core.Domain;

namespace Pixel;

public class PixelEngine<T>
{
    public Core.Domain.ISurface<T>? Surface { get; init; }
    public Core.Domain.IGraphic<T>? Graphic { get; init; }


    public PixelEngine(Core.Domain.ISurface<T> surface, Core.Domain.IGraphic<T> graphic)
    {
        this.Surface = surface;
        this.Graphic = graphic;

        surface.OnSurfaceLoad += this.Graphic.OnLoad;
        surface.OnSurfaceRender += this.Graphic.OnRender;
        surface.OnSurfaceResize += this.Graphic.OnResize;
        surface.OnSurfaceUnload += this.Graphic.OnUnLoad;
    }

    public void Start() => this.Surface?.Start();


    

}
