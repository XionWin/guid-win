using System.Drawing;
using System.Runtime.InteropServices;
using OpenTK.Graphics.ES30;
using Pixel.Core.Domain;
using Pixel.GLES.Brushes;

namespace Pixel.GLES.Graphics;

public class Render: Core.Domain.IRender
{
    public System.Drawing.Size Size { get; set; }
    public Shader? Shader { get; init; }

    public Render()
    {
        this.Shader = new Shader().Load("resources/shaders/shader.vert", "resources/shaders/shader.frag", new[] { "vertex", "tcoord" });
    }

    private uint vbo, vao;
    public void OnInit()
    {
        GL.ClearColor(0.3f, 0.3f, 0.3f, 1);
        vao = (uint)GL.GenVertexArray();
        vbo = (uint)GL.GenBuffer();
    }

    private static float angle = 0;
    private static float angle_inner = 0;
    public void OnRender()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

        GL.Enable(EnableCap.Blend);

        GL.BlendFuncSeparate(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha, BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
       
        // GL.Enable(EnableCap.CullFace);
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
        

        angle += 0.5f;

        if (angle > 360)
            angle %= 360;

        byte alpha = 255; //(byte)(DateTime.Now.Millisecond / 4 %
        var colors = new []
        {
            new Core.Domain.Color(255, 0, 0, alpha),
            new Core.Domain.Color(0, 255, 0, alpha),
            new Core.Domain.Color(0, 0, 255, alpha),
            new Core.Domain.Color(0, 255, 0, alpha),
            new Core.Domain.Color(255, 0, 0, alpha),
            new Core.Domain.Color(0, 255, 0, alpha),
            new Core.Domain.Color(0, 0, 255, alpha),
            new Core.Domain.Color(255, 0, 0, alpha),
            new Core.Domain.Color(0, 255, 0, alpha),
            new Core.Domain.Color(0, 0, 255, alpha),
            new Core.Domain.Color(0, 255, 0, alpha),
            new Core.Domain.Color(255, 0, 0, alpha),
            new Core.Domain.Color(0, 255, 0, alpha),
            new Core.Domain.Color(0, 0, 255, alpha)
        };


        var width = this.Size.Height / 4;
        var height = this.Size.Height / 4;
        for (int i = 0; i < colors.Length; i++)
        {
            if(i < colors.Length - 1)
            {
                var rectShape = new Shape.Rectangle(i * width, 0 * height, width, height, true);
                rectShape.Rotate(0, x: angle / 180f * (float)Math.PI);
                // rectShape.Transform(new PointF(100, 400));
                var rect = rectShape.Rect;
                rectShape.Fill = new LinearGradientBrush(rect.X, rect.Y, rect.X, rect.Y + rect.Height)
                    {Color1 = colors[i], Color2 = colors[i + 1]};
                this.DrawShape(rectShape);
            }
            if(i < colors.Length - 1)
            {
                var rectShape = new Shape.Rectangle(i * width, 1 * height, width, height);
                rectShape.Rotate(angle / 180f * (float)Math.PI);
                var rect = rectShape.Rect;
                var (tl, bl, br, tr) = rectShape.GetRenderRect();
                rectShape.Fill = new RadialGradientBrush(tl.X, tl.Y, 0, rect.Height)
                    {Color1 = colors[i], Color2 = new Core.Domain.Color(0, 0, 0, 200)};
                this.DrawShape(rectShape);
            }
            if(i < colors.Length - 1)
            {
                var color1 = colors[i];
                var color2 = colors[i];
                color2.a = 0;
                var rectShape = new Shape.Rectangle(i * width, 1.5f * width, width, width);
                rectShape.Rotate(0, y: angle / 180f * (float)Math.PI);
                var rect = rectShape.Rect;
                rectShape.Fill = new RadialGradientBrush(rect.X + rect.Width / 2 * (1 + (float)Math.Cos(angle_inner)), rect.Y + rect.Height / 2 *  (1 + (float)Math.Sin(angle_inner)), 0, rect.Height / 2) 
                    {Color1 = color1, Color2 = color2};
                this.DrawShape(rectShape);
            }
            if(i < colors.Length - 1)
            {
                var rectShape = new Shape.Rectangle(i * width, 2 * height, width, height);
                rectShape.Rotate(0, x: angle / 180f * (float)Math.PI);
                // rectShape.Transform(new PointF(100, 400));
                var rect = rectShape.Rect;
                rectShape.Fill = new LinearGradientBrush(rect.X, rect.Y, rect.X, rect.Y + rect.Height)
                    {Color1 = colors[i], Color2 = colors[i + 1]};
                this.DrawShape(rectShape);
            }

        }

        // for (int i = 0; i < colors.Length; i++)
        // {
        //     if(i < colors.Length - 1)
        //     {
                
        //         var rectShape = new Shape.Rectangle(i * width, 0.5f * width, width, width);
        //         rectShape.Rotate(angle / 180f * (float)Math.PI);
        //         var rect = rectShape.Rect;
        //         var (tl, bl, br, tr) = rectShape.GetRenderRect();
        //         rectShape.Fill = new RadialGradientBrush(tl.X, tl.Y, 0, rect.Height)
        //             {Color1 = colors[i], Color2 = new Core.Domain.Color(0, 0, 0, 200)};
        //         this.DrawShape(rectShape);
        //     }
        // }


        // angle_inner += 0.1f;

        // if (angle_inner > 360)
        //     angle_inner %= 360;
        // for (int i = 0; i < colors.Length; i++)
        // {
        //     var color1 = colors[i];
        //     var color2 = colors[i];
        //     color2.a = 0;
        //     var rectShape = new Shape.Rectangle(i * width, 1.5f * width, width, width);
        //     rectShape.Rotate(0, y: angle / 180f * (float)Math.PI);
        //     var rect = rectShape.Rect;
        //     rectShape.Fill = new RadialGradientBrush(rect.X + rect.Width / 2 * (1 + (float)Math.Cos(angle_inner)), rect.Y + rect.Height / 2 *  (1 + (float)Math.Sin(angle_inner)), 0, rect.Height / 2) 
        //         {Color1 = color1, Color2 = color2};
        //     this.DrawShape(rectShape);
        // }

        // for (int i = 0; i < colors.Length; i++)
        // {
        //     var color1 = colors[i];
        //     var color2 = colors[i];
        //     color1.a = 0;
        //     var rectShape = new Shape.Rectangle(i * width, 2.5f * width, width, width);
        //     rectShape.Rotate(0, x: angle / 180f * (float)Math.PI, y: angle / 180f * (float)Math.PI);
        //     var rect = rectShape.Rect;
        //     rectShape.Fill = new RadialGradientBrush(rect.X + rect.Width / 2 * (1 + (float)Math.Cos(angle_inner)), rect.Y + rect.Height / 2 *  (1 + (float)Math.Sin(angle_inner)), 0, rect.Height / 2) 
        //         {Color1 = color1, Color2 = color2};
        //     this.DrawShape(rectShape);
        // }

        // for (int i = 0; i < colors.Length; i++)
        // {
        //     var color1 = colors[i];
        //     var color2 = colors[i];
        //     color1.a = 0;
        //     var rectShape = new Shape.Rectangle(i * width, 3.5f * width, width, width);
        //     rectShape.Rotate(0, y: angle / 180f * (float)Math.PI);
        //     var rect = rectShape.Rect;
        //     rectShape.Fill = new RadialGradientBrush(rect.X + rect.Width / 2 * (1 + (float)Math.Cos(angle_inner)), rect.Y + rect.Height / 2 *  (1 + (float)Math.Sin(angle_inner)), 0, rect.Height / 2) 
        //         {Color1 = color1, Color2 = color2};
        //     this.DrawShape(rectShape);
        // }
    }

    public void OnSizeChange(System.Drawing.Size size)
    {
        GL.Viewport(0, 0, size.Width, size.Height);
        this.Size = size;
    }

    public void OnEnd()
    {
        throw new NotImplementedException();
    }

    
    private void DrawShape(IShape shape)
    {
        if(this.Shader is Shader shader)
        {
            shader.Use();
            GL.BindVertexArray(vao);

            var commands = shape.Geometry.Commands;

            var vertexes1 = new List<Vertex>();
            foreach (var command in commands)
            {
                if(command is Pixel.Core.Domain.Command.MoveToCommand moveTo)
                {
                    vertexes1.Add(new Vertex(moveTo.Value.X, moveTo.Value.Y, moveTo.Value.Z, 0.5f, 1));
                }
                if(command is Pixel.Core.Domain.Command.LineToCommand lineTo)
                {
                    vertexes1.Add(new Vertex(lineTo.Value.X, lineTo.Value.Y, lineTo.Value.Z, 0.5f, 1));
                }
            }
            
            var vertexes = vertexes1.ToArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (int)(Marshal.SizeOf(typeof(Vertex)) * vertexes.Count()), vertexes.ToArray(), BufferUsageHint.StreamDraw);

            GL.VertexAttribPointer(shader["vertex"], 3, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), 0);
            GL.VertexAttribPointer(shader["tcoord"], 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), Marshal.SizeOf<float>() * 3);
            GL.Uniform2(shader["viewSize"], (float)this.Size.Width, (float)this.Size.Height);
            GL.Uniform4(shader["frag"], GLFragUniforms.UNIFORMARRAY_SIZE, shape.Fill.GetData());

            var model = shape.Geometry.Matrix.ToMatrix4();
            GL.UniformMatrix4(shader["model"], true, ref model);
            var view = shape.Is3D ? OpenTK.Mathematics.Matrix4.CreateTranslation(0, 0, -2f) : OpenTK.Mathematics.Matrix4.Identity;
            GL.UniformMatrix4(shader["view"], true, ref view);
            var projection = shape.Is3D ? OpenTK.Mathematics.Matrix4.CreatePerspectiveFieldOfView((float) (90 * Math.PI / 180.0), 1f, 1f, 4f) : OpenTK.Mathematics.Matrix4.Identity;
            GL.UniformMatrix4(shader["projection"], true, ref projection);
            var viewZoom = shape.Is3D ? OpenTK.Mathematics.Matrix4.CreateScale(2) : OpenTK.Mathematics.Matrix4.Identity;
            GL.UniformMatrix4(shader["viewZoom"], true, ref viewZoom);

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
}


static class RenderExtension
{
    public static OpenTK.Mathematics.Matrix4 ToMatrix4(this System.Numerics.Matrix4x4 matrix) =>
        new OpenTK.Mathematics.Matrix4
        (
            matrix.M11, matrix.M12, matrix.M13, matrix.M14,
            matrix.M21, matrix.M22, matrix.M23, matrix.M24,
            matrix.M31, matrix.M32, matrix.M33, matrix.M34,
            matrix.M41, matrix.M42, matrix.M43, matrix.M44
        );
}