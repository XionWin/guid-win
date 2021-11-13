namespace Pixel;
public class PixelEngine
{
    public Core.Domain.ISurface? Surface { get; init; }

    public PixelEngine(Core.Domain.ISurface surface) => this.Surface = surface;

    public void Start() => this.Surface?.Start();

}
