namespace Pixel.Core.Domain;
public interface ISurface
{
    void OnLoadSurface();
    void OnRenderSurface();
    void OnUpdateSurface();
    void OnResizeSurface(System.Drawing.Size size);
    void OnUnloadSurface();

    void Start();
}
