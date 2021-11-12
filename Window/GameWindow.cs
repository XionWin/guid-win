using System.Drawing;
using System.Runtime.Versioning;
using Common;
using OpenTK.Graphics.ES30;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Window;
public class GameWindow : OpenTK.Windowing.Desktop.GameWindow
{
    private GameWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {}

    public GameWindow(int width, int height):
        base(
            new GameWindowSettings()
            {
                
            },
            new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(width, height),
            }
        )
    {
        this.Shader = new Shader().Load("resources/shaders/shader.vert", "resources/shaders/shader.frag");
    }

    private readonly float[] _vertices =
    {
        -0.5f, -0.5f, 0.0f, // Bottom-left vertex
        0.5f, -0.5f, 0.0f, // Bottom-right vertex
        0.0f,  0.5f, 0.0f  // Top vertex
    };

    // These are the handles to OpenGL objects. A handle is an integer representing where the object lives on the
    // graphics card. Consider them sort of like a pointer; we can't do anything with them directly, but we can
    // send them to OpenGL functions that need them.

    // What these objects are will be explained in OnLoad.
    private int _vertexBufferObject;

    private int _vertexArrayObject;

    // This class is a wrapper around a shader, which helps us manage it.
    // The shader class's code is in the Common project.
    // What shaders are and what they're used for will be explained later in this tutorial.
    private Shader? Shader { get; init; }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        GL.EnableVertexAttribArray(0);

    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
        this.Shader?.Use();

        // Bind the VAO
        GL.BindVertexArray(_vertexArrayObject);

        // And then call our drawing function.
        // For this tutorial, we'll use GL.DrawArrays, which is a very simple rendering function.
        // Arguments:
        //   Primitive type; What sort of geometric primitive the vertices represent.
        //     OpenGL used to support many different primitive types, but almost all of the ones still supported
        //     is some variant of a triangle. Since we just want a single triangle, we use Triangles.
        //   Starting index; this is just the start of the data you want to draw. 0 here.
        //   How many vertices you want to draw. 3 for a triangle.
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        // OpenTK windows are what's known as "double-buffered". In essence, the window manages two buffers.
        // One is rendered to while the other is currently displayed by the window.
        // This avoids screen tearing, a visual artifact that can happen if the buffer is modified while being displayed.
        // After drawing, call this function to swap the buffers. If you don't, it won't display what you've rendered.
        SwapBuffers();
    }
    
    // This function runs on every update frame.
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        // Check if the Escape button is currently being pressed.
        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            // If it is, close the window.
            Close();
        }

        base.OnUpdateFrame(e);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        // When the window gets resized, we have to call GL.Viewport to resize OpenGL's viewport to match the new size.
        // If we don't, the NDC will no longer be correct.
        GL.Viewport(0, 0, Size.X, Size.Y);
    }
    protected override void OnUnload()
    {
        // Unbind all the resources by binding the targets to 0/null.
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        // Delete all the resources.
        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);

        GL.DeleteProgram(this.Shader?.Handle ?? 0);

        base.OnUnload();
    }
    

}


