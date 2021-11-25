using Pixel.Core.Domain;

namespace Pixel;

public delegate void PixelEngineRenderHandle(IGraphic<float> graphic);
public class PixelEngine
{
    public Core.Domain.ISurface? Surface { get; init; }

    public event PixelEngineRenderHandle? OnRender;

    public PixelEngine(Core.Domain.ISurface surface) => this.Surface = surface;

    public void Start() => this.Surface?.Start();


    

}
