using System.Runtime.InteropServices;
using System.Linq;
using OpenTK.Graphics.ES30;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Pixel.Core;

namespace Pixel.GLES;
public class PixelWindow: GameWindow, Core.Domain.ISurface
{
    private readonly static byte[] ICON_DATA = new byte[]
    {
        255, 51, 51, 255,
    };

    public Shader? Shader {get; init;}

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
        this.Shader = new Shader().Load("resources/shaders/shader.vert", "resources/shaders/shader.frag", new [] {"vertex", "tcoord"});
    }

    private uint vbo, vao;
    public void OnLoadSurface()
    {
        GL.ClearColor(0.3f, 0.3f, 0.3f, 1);
        vao = (uint)GL.GenVertexArray();
        vbo = (uint)GL.GenBuffer();
    }

    GLFragUniforms frag = new GLFragUniforms()
    {
        PaintMat = new float[]{1f, 0, 0, 0, 0, 1f, 0, 0, 0, 0, 100, 100},
        InnerCol = new Core.Domain.Color<float>(0.2f, 0, 0, 0.08f),
        OuterCol = new Core.Domain.Color<float>(0, 0, 1f, 0.5f),
        ScissorExt = new float[]{1f, 1f},
        ScissorScale = new float[]{1f, 1f},
        Extent = new float[]{500, 100},
        Radius = 0f,
        Feather = 1f,
        StrokeMult = 1f,
        StrokeThr = -1f,
        TexType = 0,
        Type = 0,
    };


    public void OnRenderSurface()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

        GL.Enable(EnableCap.Blend);

        GL.BlendFuncSeparate(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha, BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
       
        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Back);
        GL.FrontFace(FrontFaceDirection.Ccw);
        GL.Enable(EnableCap.Blend);
        GL.Disable(EnableCap.DepthTest);
        GL.Disable(EnableCap.ScissorTest);
        GL.ColorMask(true, true, true, true);
        GL.StencilMask(0xffffffff);
        GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
        GL.StencilFunc(StencilFunction.Always, 0, 0xffffffff);
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, 0);

        byte alpha = (byte)(DateTime.Now.Millisecond / 4 % 255);
        var colors = new []
        {
            new Core.Domain.Color<byte>(255, 0, 0, alpha),
            new Core.Domain.Color<byte>(0, 255, 0, alpha),
            new Core.Domain.Color<byte>(0, 0, 255, alpha),
            new Core.Domain.Color<byte>(0, 255, 0, alpha),
            new Core.Domain.Color<byte>(255, 0, 0, alpha),
            new Core.Domain.Color<byte>(0, 255, 0, alpha),
            new Core.Domain.Color<byte>(0, 0, 255, alpha),
        };

        var width = 200;
        for (int i = 0; i < colors.Length; i++)
        {
            if(i < colors.Length - 1)
            {
                var rect = new System.Drawing.RectangleF(i * width, 0, width, width);
                var linearGradientBrush = new Brush.LinearGradientBrush(rect.X, rect.Y, rect.X + rect.Width, rect.Y) 
                    {Color1 = colors[i], Color2 = colors[i + 1]};
                this.DrawRect(rect, linearGradientBrush);
            }
        }

        
        var colors2 = new []
        {
            new Core.Domain.Color<byte>(255, 0, 0, 255),
            new Core.Domain.Color<byte>(0, 255, 0, 255),
            new Core.Domain.Color<byte>(0, 0, 255, 255),
            new Core.Domain.Color<byte>(0, 255, 0, 255),
            new Core.Domain.Color<byte>(255, 0, 0, 255),
            new Core.Domain.Color<byte>(0, 255, 0, 255),
            new Core.Domain.Color<byte>(0, 0, 255, 255),
        };
        var angle = DateTime.Now.Millisecond / 5 / 200f * (float)Math.PI * 2;
        for (int i = 0; i < colors.Length; i++)
        {
            if(i < colors.Length - 1)
            {
                
                var rect = new System.Drawing.RectangleF(i * width, 200, width, width);
                var radialGradientBrush = new Brush.RadialGradientBrush(rect.X + rect.Width / 2 * (1 + (float)Math.Sin(angle)), rect.Y + rect.Height / 2 *  (1 + (float)Math.Cos(angle)), 0, rect.Height / 2) 
                    {Color1 = colors2[i], Color2 = colors2[i + 1]};
                this.DrawRect(rect, radialGradientBrush);
            }
        }

        
        for (int i = 0; i < colors.Length; i++)
        {
            var color1 = colors2[i];
            var color2 = colors2[i];
            color2.a = 0;
            var rect = new System.Drawing.RectangleF(i * width, 400, width, width);
            var radialGradientBrush = new Brush.RadialGradientBrush(rect.X + rect.Width / 2 * (1 + (float)Math.Sin(angle)), rect.Y + rect.Height / 2 *  (1 + (float)Math.Cos(angle)), 0, rect.Height / 2) 
                {Color1 = color1, Color2 = color2};
            this.DrawRect(rect, radialGradientBrush);
        }

        for (int i = 0; i < colors.Length; i++)
        {
            var color1 = colors2[i];
            var color2 = colors2[i];
            color1.a = 0;
            var rect = new System.Drawing.RectangleF(i * width, 600, width, width);
            var radialGradientBrush = new Brush.RadialGradientBrush(rect.X + rect.Width / 2 * (1 + (float)Math.Sin(angle)), rect.Y + rect.Height / 2 *  (1 + (float)Math.Cos(angle)), 0, rect.Height / 2) 
                {Color1 = color1, Color2 = color2};
            this.DrawRect(rect, radialGradientBrush);
        }
        
    }

    private void DrawRect(System.Drawing.RectangleF rect, Brush.Brush brush)
    {
        if(this.Shader is Shader shader)
        {
            shader.Use();
            GL.BindVertexArray(vao);

            var vertexes = new Vertex[]
            {
                new Vertex(rect.X, rect.Y, 0.5f, 1),
                new Vertex(rect.X, rect.Y + rect.Height, 0.5f, 1),
                new Vertex(rect.X + rect.Width, rect.Y + rect.Height, 0.5f, 1),
                new Vertex(rect.X + rect.Width, rect.Y, 0.5f, 1),
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (int)(Marshal.SizeOf(typeof(Vertex)) * vertexes.Count()), vertexes.ToArray(), BufferUsageHint.StreamDraw);

            GL.VertexAttribPointer(shader["vertex"], 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), 0);
            GL.VertexAttribPointer(shader["tcoord"], 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), Marshal.SizeOf<float>() * 2);
            GL.Uniform2(shader["viewSize"], (float)this.Size.X, (float)this.Size.Y);
            GL.Uniform4(shader["frag"], GLFragUniforms.UNIFORMARRAY_SIZE, brush.GetData());


            if(GL.GetError() is var err && err != OpenTK.Graphics.ES30.ErrorCode.NoError)
                throw new Exception();

            GL.Enable(EnableCap.StencilTest);
            GL.StencilMask(0xff);

            GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Incr);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertexes.Length);



            GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertexes.Length);



            GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
            GL.StencilOp(StencilOp.Zero, StencilOp.Zero, StencilOp.Zero);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertexes.Length);


            GL.ColorMask(true, true, true, true);
            GL.Disable(EnableCap.StencilTest);
        }
    }

    public void OnUpdateSurface()
    {
        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    public void OnResizeSurface(System.Drawing.Size size)
    {
        GL.Viewport(0, 0, size.Width, size.Height);
    }

    public void OnUnloadSurface()
    {

    }

    public void Start()
    {
        this.Run();
    }



    protected override void OnLoad()
    {
        base.OnLoad();
        this.OnLoadSurface();
    }


    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        this.OnRenderSurface();
        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);
        this.OnUpdateSurface();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        this.OnResizeSurface(new System.Drawing.Size(e.Width, e.Height));
    }
    protected override void OnUnload()
    {
        base.OnUnload();
        this.OnUnloadSurface();
    }
}
