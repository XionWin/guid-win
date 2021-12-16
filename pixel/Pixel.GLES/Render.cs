using System.Runtime.InteropServices;
using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using Pixel.Core.Domain;
using Pixel.GLES.Brushes;

namespace Pixel.GLES.Graphics;

public class Render: Core.Domain.IRender<float>
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

    public void OnRender()
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
        };

        var width = 200;
        for (int i = 0; i < colors.Length; i++)
        {
            if(i < colors.Length - 1)
            {
                var rectShape = new Shapes.Rectangle(i * width, 0, width, width);
                var linearGradientBrush = new LinearGradientBrush(rectShape.Rect.X, rectShape.Rect.Y, rectShape.Rect.X + rectShape.Rect.Width, rectShape.Rect.Y) 
                    {Color1 = colors[i], Color2 = colors[i + 1]};
                this.DrawRect(rectShape, linearGradientBrush);
            }
        }

        // var colors2 = new []
        // {
        //     new Core.Domain.Color(255, 0, 0, 255),
        //     new Core.Domain.Color(0, 255, 0, 255),
        //     new Core.Domain.Color(0, 0, 255, 255),
        //     new Core.Domain.Color(0, 255, 0, 255),
        //     new Core.Domain.Color(255, 0, 0, 255),
        //     new Core.Domain.Color(0, 255, 0, 255),
        //     new Core.Domain.Color(0, 0, 255, 255),
        // };
        // var angle = DateTime.Now.Millisecond / 5 / 200f * (float)Math.PI * 2;
        // for (int i = 0; i < colors.Length; i++)
        // {
        //     if(i < colors.Length - 1)
        //     {
                
        //         var rect = new System.Drawing.RectangleF(i * width, 200, width, width);
        //         var radialGradientBrush = new RadialGradientBrush(rect.X + rect.Width / 2 * (1 + (float)Math.Cos(angle)), rect.Y + rect.Height / 2 *  (1 + (float)Math.Sin(angle)), 0, rect.Height / 2) 
        //             {Color1 = colors2[i], Color2 = colors2[i + 1]};
        //         this.DrawRect(rect, radialGradientBrush);
        //     }
        // }

        
        // for (int i = 0; i < colors.Length; i++)
        // {
        //     var color1 = colors2[i];
        //     var color2 = colors2[i];
        //     color2.a = 0;
        //     var rect = new System.Drawing.RectangleF(i * width, 400, width, width);
        //     var radialGradientBrush = new RadialGradientBrush(rect.X + rect.Width / 2 * (1 + (float)Math.Cos(angle)), rect.Y + rect.Height / 2 *  (1 + (float)Math.Sin(angle)), 0, rect.Height / 2) 
        //         {Color1 = color1, Color2 = color2};
        //     this.DrawRect(rect, radialGradientBrush);
        // }

        // for (int i = 0; i < colors.Length; i++)
        // {
        //     var color1 = colors2[i];
        //     var color2 = colors2[i];
        //     color1.a = 0;
        //     var rect = new System.Drawing.RectangleF(i * width, 600, width, width);
        //     var radialGradientBrush = new RadialGradientBrush(rect.X + rect.Width / 2 * (1 + (float)Math.Cos(angle)), rect.Y + rect.Height / 2 *  (1 + (float)Math.Sin(angle)), 0, rect.Height / 2) 
        //         {Color1 = color1, Color2 = color2};
        //     this.DrawRect(rect, radialGradientBrush);
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

    public void FillRect(Rect rect)
    {
        throw new NotImplementedException();
    }

    public void StrokeRect(Rect rect)
    {
        throw new NotImplementedException();
    }
    
    private void DrawRect(Shapes.Rectangle rect, Brush brush)
    {
        if(this.Shader is Shader shader)
        {
            shader.Use();
            GL.BindVertexArray(vao);

            var commands = rect.Commands;

            var vertexes1 = new List<Vertex>();
            foreach (var command in commands)
            {
                if(command is Shapes.Command.MoveToCommand moveTo)
                {
                    vertexes1.Add(new Vertex(moveTo.Value.X, moveTo.Value.Y, 0.5f, 1));
                }
                if(command is Shapes.Command.LineToCommand lineTo)
                {
                    vertexes1.Add(new Vertex(lineTo.Value.X, lineTo.Value.Y, 0.5f, 1));
                }
            }
            
            var vertexes = vertexes1.ToArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (int)(Marshal.SizeOf(typeof(Vertex)) * vertexes.Count()), vertexes.ToArray(), BufferUsageHint.StreamDraw);

            GL.VertexAttribPointer(shader["vertex"], 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), 0);
            GL.VertexAttribPointer(shader["tcoord"], 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), Marshal.SizeOf<float>() * 2);
            GL.Uniform2(shader["viewSize"], (float)this.Size.Width, (float)this.Size.Height);
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

    private void DrawRect(System.Drawing.RectangleF rect, Brush brush)
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
            GL.Uniform2(shader["viewSize"], (float)this.Size.Width, (float)this.Size.Height);
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
}