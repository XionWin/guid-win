using System.Runtime.InteropServices;
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
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        vao = (uint)GL.GenVertexArray();
        vbo = (uint)GL.GenBuffer();
    }

    private List<Vertex> Vertexes = Vertex.Defalut;
    GLFragUniforms frag = new GLFragUniforms()
    {
        paintMat = new float[]{1f, 0, 0, 0, 0, 1f, 0, 0, 0, 0, 1f, 0},
        innerCol = new Core.Domain.Color(1f, 1f, 1f, 1f),
        outerCol = new Core.Domain.Color(0, 0, 1f, 1f),
        scissorExt = new float[]{1f, 1f},
        scissorScale = new float[]{1f, 1f},
        extent = new float[]{0, 0}, 
        Radius = 0f,
        Feather = 1f,
        StrokeMult = 1f,
        StrokeThr = -1f,
        texType = 0,
        Type = 0,
    };
    public void OnRenderSurface()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        if(this.Shader is Shader shader)
        {
            shader.Use();
            GL.BindVertexArray(vao);
                
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (int)(Marshal.SizeOf(typeof(Vertex)) * Vertexes.Count()), Vertexes.ToArray(), BufferUsageHint.StreamDraw);


            var vertexPos = GL.GetAttribLocation(Shader.Program, "vertex");
            GL.EnableVertexAttribArray(vertexPos);
            GL.VertexAttribPointer(vertexPos, 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), 0);

            var ftcoordPos = GL.GetAttribLocation(Shader.Program, "tcoord");
            GL.EnableVertexAttribArray(ftcoordPos);
            GL.VertexAttribPointer(ftcoordPos, 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), Marshal.SizeOf<float>() * 2);

            shader.SetVector2("viewSize", (float)this.Size.X, (float)this.Size.Y);
            // var viewSizePos = GL.GetUniformLocation(shader.Program, "viewSize");
            // GL.Uniform2(viewSizePos, (float)this.Size.X, (float)this.Size.Y);


            var fragPos = GL.GetUniformLocation(shader.Program, "frag");
            GL.Uniform4(fragPos, GLFragUniforms.UNIFORMARRAY_SIZE, frag.Floats);


            if(GL.GetError() is var err && err != OpenTK.Graphics.ES30.ErrorCode.NoError)
                throw new Exception();

            GL.Enable(EnableCap.StencilTest);
            GL.StencilMask(0xff);

            GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Incr);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 3);

            

            GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 3);

            

            GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
            GL.StencilOp(StencilOp.Zero, StencilOp.Zero, StencilOp.Zero);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 3);

            
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
