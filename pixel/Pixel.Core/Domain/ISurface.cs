namespace Pixel.Core.Domain;

public interface ISurface<T>
{
    event Action OnSurfaceLoad;
    event Action OnSurfaceRender;
    event Action<System.Drawing.Size> OnSurfaceResize;
    event Action OnSurfaceUnload;

    void Start();
}
