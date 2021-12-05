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


    static float angle = 0;
    public void OnRenderSurface()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

        GL.Enable(EnableCap.Blend);

        GL.BlendFuncSeparate(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha, BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
        // GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        
        // GL.BlendFuncSeparate(BlendingFactorSrc.OneMinusSrcAlpha, BlendingFactorDest.OneMinusSrcAlpha, BlendingFactorSrc.One, BlendingFactorDest.Zero);


        // GL.Enable(EnableCap.CullFace);
        // GL.CullFace(CullFaceMode.Back);
        // GL.FrontFace(FrontFaceDirection.Ccw);
        // GL.Enable(EnableCap.Blend);
        // GL.Disable(EnableCap.DepthTest);
        // GL.Disable(EnableCap.ScissorTest);
        // GL.ColorMask(true, true, true, true);
        // GL.StencilMask(0xffffffff);
        // GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
        // GL.StencilFunc(StencilFunction.Always, 0, 0xffffffff);
        // GL.ActiveTexture(TextureUnit.Texture0);
        // GL.BindTexture(TextureTarget.Texture2D, 0);

        if(this.Shader is Shader shader)
        {

            float x = 0, y = 0, w = 400, h = 400;
            shader.Use();
            GL.BindVertexArray(vao);


            var vertexes = new Vertex[]
            {
                new Vertex(x, y, 0.5f, 1),
                new Vertex(x, y + h, 0.5f, 1),
                new Vertex(x + w, y + h, 0.5f, 1),
                new Vertex(x + w, y, 0.5f, 1),

                // new Vertex(x, y + h, 0.5f, 1),
                // new Vertex(x + w, y + h, 0.5f, 1),
                // new Vertex(x + w, y, 0.5f, 1),
                // new Vertex(x, y + h, 0.5f, 1),
                // new Vertex(x, y, 0.5f, 1),
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (int)(Marshal.SizeOf(typeof(Vertex)) * vertexes.Count()), vertexes.ToArray(), BufferUsageHint.StreamDraw);

            GL.VertexAttribPointer(shader["vertex"], 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), 0);
            GL.VertexAttribPointer(shader["tcoord"], 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), Marshal.SizeOf<float>() * 2);
            GL.Uniform2(shader["viewSize"], (float)this.Size.X, (float)this.Size.Y);

            // GL.Uniform4(shader["frag"], GLFragUniforms.UNIFORMARRAY_SIZE, Extension.GetLinearGradient(x + w / 2 - 50, y, x + w / 2 + 50, y + h));

            var lineGradinet = new Brush.LinearGradientBrush(x, y, x, y + h) 
                {Color1 = new Core.Domain.Color<byte>(255, 0, 0, 25), Color2 = new Core.Domain.Color<byte>(255, 0, 0, 125)};
            

            GL.Uniform4(shader["frag"], GLFragUniforms.UNIFORMARRAY_SIZE, lineGradinet.GetData());
            angle += 0.1f;
            angle %= 360;


            if(GL.GetError() is var err && err != OpenTK.Graphics.ES30.ErrorCode.NoError)
                throw new Exception();

            // GL.Enable(EnableCap.StencilTest);
            // GL.StencilMask(0xff);

            // GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
            // GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Incr);
            // GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertexes.Length);



            GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertexes.Length);



            // GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
            // GL.StencilOp(StencilOp.Zero, StencilOp.Zero, StencilOp.Zero);
            // GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertexes.Length);


            // GL.ColorMask(true, true, true, true);
            // GL.Disable(EnableCap.StencilTest);
        }

        // if(this.Shader is Shader shader0_2)
        // {

        //     float x = 500, y = 0, w = 400, h = 400;
        //     shader0_2.Use();
        //     GL.BindVertexArray(vao);


        //     var vertexes = new Vertex[]
        //     {

        //         new Vertex(x, y, 0.5f, 1),
        //         new Vertex(x, y + h, 0.5f, 1),
        //         new Vertex(x + w, y + h, 0.5f, 1),
        //         new Vertex(x + w, y, 0.5f, 1),

        //     };

        //     GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        //     GL.BufferData(BufferTarget.ArrayBuffer, (int)(Marshal.SizeOf(typeof(Vertex)) * vertexes.Count()), vertexes.ToArray(), BufferUsageHint.StreamDraw);

        //     GL.VertexAttribPointer(shader0_2["vertex"], 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), 0);
        //     GL.VertexAttribPointer(shader0_2["tcoord"], 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), Marshal.SizeOf<float>() * 2);
        //     GL.Uniform2(shader0_2["viewSize"], (float)this.Size.X, (float)this.Size.Y);

        //     // GL.Uniform4(shader["frag"], GLFragUniforms.UNIFORMARRAY_SIZE, Extension.GetLinearGradient(x + w / 2 - 50, y, x + w / 2 + 50, y + h));

        //     var solidColorBrush = new Brush.SolidColorBursh() 
        //         {Color = new Core.Domain.Color<float>(1f, 0f, 0f, 0.5f)};

        //     GL.Uniform4(shader0_2["frag"], GLFragUniforms.UNIFORMARRAY_SIZE, solidColorBrush.GetData());
        //     angle += 0.1f;
        //     angle %= 360;


        //     if(GL.GetError() is var err && err != OpenTK.Graphics.ES30.ErrorCode.NoError)
        //         throw new Exception();

        //     // GL.Enable(EnableCap.StencilTest);
        //     // GL.StencilMask(0xff);

        //     // GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
        //     // GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Incr);
        //     // GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertexes.Length);



        //     GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
        //     GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
        //     GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertexes.Length);



        //     // GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
        //     // GL.StencilOp(StencilOp.Zero, StencilOp.Zero, StencilOp.Zero);
        //     // GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertexes.Length);


        //     // GL.ColorMask(true, true, true, true);
        //     // GL.Disable(EnableCap.StencilTest);
        // }

        if(this.Shader is Shader shader2)
        {

            float x = 500, y = 0, w = 400, h = 400;
            shader2.Use();
            GL.BindVertexArray(vao);


            var vertexes = new Vertex[]
            {

                new Vertex(x, y, 0.5f, 1),
                new Vertex(x, y + h, 0.5f, 1),
                new Vertex(x + w, y + h, 0.5f, 1),
                new Vertex(x + w, y, 0.5f, 1),

            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (int)(Marshal.SizeOf(typeof(Vertex)) * vertexes.Count()), vertexes.ToArray(), BufferUsageHint.StreamDraw);


            GL.VertexAttribPointer(shader2["vertex"], 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), 0);
            GL.VertexAttribPointer(shader2["tcoord"], 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), Marshal.SizeOf<float>() * 2);
            GL.Uniform2(shader2["viewSize"], (float)this.Size.X, (float)this.Size.Y);

            // GL.Uniform4(shader["frag"], GLFragUniforms.UNIFORMARRAY_SIZE, Extension.GetLinearGradient(x + w / 2 - 50, y, x + w / 2 + 50, y + h));
             var radialGradient = new Brush.RadialGradientBrush(x + w / 2, y + h / 2, 0, h / 2) 
                {Color1 = new Core.Domain.Color<byte>(255, 0, 0, 200), Color2 = new Core.Domain.Color<byte>(255, 0, 0, 25)};

            GL.Uniform4(shader2["frag"], GLFragUniforms.UNIFORMARRAY_SIZE, radialGradient.GetData());
            angle += 0.1f;
            angle %= 360;


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

        // GL.Enable(EnableCap.Blend);
        // GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);

        // if(this.Shader is Shader shader_0)
        // {
        //     shader_0.Use();
        //     GL.BindVertexArray(vao);


        //     var vertexes = new Vertex[]
        //     {
        //         new Vertex(300, 0, 0.5f, 1),
        //         new Vertex(100, 300, 0.5f, 0.5f),
        //         new Vertex(500, 300, 0.5f, 1),
        //     };

        //     GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        //     GL.BufferData(BufferTarget.ArrayBuffer, (int)(Marshal.SizeOf(typeof(Vertex)) * vertexes.Count()), vertexes.ToArray(), BufferUsageHint.StreamDraw);


        //     GL.VertexAttribPointer(shader_0["vertex"], 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), 0);
        //     GL.VertexAttribPointer(shader_0["tcoord"], 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), Marshal.SizeOf<float>() * 2);
        //     GL.Uniform2(shader_0["viewSize"], (float)this.Size.X, (float)this.Size.Y);


        //     GL.Uniform4(shader_0["frag"], GLFragUniforms.UNIFORMARRAY_SIZE, frag.Floats);


        //     if(GL.GetError() is var err && err != OpenTK.Graphics.ES30.ErrorCode.NoError)
        //         throw new Exception();

        //     GL.Enable(EnableCap.StencilTest);
        //     GL.StencilMask(0xff);

        //     GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
        //     GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Incr);
        //     GL.DrawArrays(PrimitiveType.TriangleStrip, 0, vertexes.Length);



        //     GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
        //     GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
        //     GL.DrawArrays(PrimitiveType.TriangleStrip, 0, vertexes.Length);



        //     GL.StencilFunc(StencilFunction.Equal, 0x00, 0xff);
        //     GL.StencilOp(StencilOp.Zero, StencilOp.Zero, StencilOp.Zero);
        //     GL.DrawArrays(PrimitiveType.TriangleStrip, 0, vertexes.Length);


        //     GL.ColorMask(true, true, true, true);
        //     GL.Disable(EnableCap.StencilTest);
        // }
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
