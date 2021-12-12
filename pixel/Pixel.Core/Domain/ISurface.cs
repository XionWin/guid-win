namespace Pixel.Core.Domain;

public interface ISurface
{
    event Action OnInit;
    event Action OnRender;
    event Action<System.Drawing.Size> OnSizeChange;
    event Action OnEnd;

    void Start();
}
