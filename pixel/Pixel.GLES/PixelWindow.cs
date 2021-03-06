using System.Runtime.InteropServices;
using System.Linq;
using OpenTK.Graphics.ES30;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Pixel.Core;
using Pixel.Core.Domain;

namespace Pixel.GLES;
public class PixelWindow: GameWindow, Core.Domain.ISurface
{
    private readonly static byte[] ICON_DATA = new byte[]
    {
        255, 51, 51, 255,
    };


    public PixelWindow(int width, int height):
        base(
            new GameWindowSettings()
            {
                RenderFrequency = 60,
                UpdateFrequency = 30,
            },
            new NativeWindowSettings()
            {
                Title = "Pixel Renderer",
                Size = new OpenTK.Mathematics.Vector2i(width, height),
                API = ContextAPI.OpenGLES,
                APIVersion = new Version(3, 2),
                Icon = new OpenTK.Windowing.Common.Input.WindowIcon(new OpenTK.Windowing.Common.Input.Image(1, 1, ICON_DATA)),
            }
        )
    {
    }

    public event Action? OnInit;
    public event Action? OnRender;
    public event Action<System.Drawing.Size>? OnSizeChange;
    public event Action? OnEnd;


    public void Start()
    {
        this.Run();
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        this.OnInit?.Invoke();
    }


    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        this.OnRender?.Invoke();
        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        this.OnSizeChange?.Invoke(new System.Drawing.Size(e.Width, e.Height)); 
        base.OnResize(e);
    }
    protected override void OnUnload()
    {
        base.OnUnload();
        this.OnEnd?.Invoke();
    }
}
