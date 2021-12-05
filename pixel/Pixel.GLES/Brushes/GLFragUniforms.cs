using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace Pixel.GLES.Brush;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GLFragUniforms
{
    public const int UNIFORMARRAY_SIZE = 11;
    public static int Size => Marshal.SizeOf(typeof(GLFragUniforms));

    public Matrix3x4 ScissorMat {get; set;}
    public Matrix3x4 PaintMat {get; set;}
    public Vector4 InnerColor {get; set;}
    public Vector4 OuterColor {get; set;}
    public Vector2 ScissorExt {get; set;}
    public Vector2 ScissorScale {get; set;}
    public Vector2 Extent {get; set;}
    public float Radius {get; set;}
    public float Feather {get; set;}
    public float StrokeMult {get; set;}
    public float StrokeThr {get; set;}
    public float TexType {get; set;}
    public float Type {get; set;}

    public float[] GetData()
    {
        return new []
        {
            this.ScissorMat.Row0.X, this.ScissorMat.Row0.Y, this.ScissorMat.Row0.Z, this.ScissorMat.Row0.W,
            this.ScissorMat.Row1.X, this.ScissorMat.Row1.Y, this.ScissorMat.Row1.Z, this.ScissorMat.Row1.W,
            this.ScissorMat.Row2.X, this.ScissorMat.Row2.Y, this.ScissorMat.Row2.Z, this.ScissorMat.Row2.W,

            this.PaintMat.Row0.X, this.PaintMat.Row0.Y, this.PaintMat.Row0.Z, this.PaintMat.Row0.W,
            this.PaintMat.Row1.X, this.PaintMat.Row1.Y, this.PaintMat.Row1.Z, this.PaintMat.Row1.W,
            this.PaintMat.Row2.X, this.PaintMat.Row2.Y, this.PaintMat.Row2.Z, this.PaintMat.Row2.W,

            this.InnerColor.X, this.InnerColor.Y, this.InnerColor.Z, this.InnerColor.W,
            this.OuterColor.X, this.OuterColor.Y, this.OuterColor.Z, this.OuterColor.W,

            this.ScissorExt.X, this.ScissorExt.Y,
            this.ScissorScale.X, this.ScissorScale.Y,
            this.Extent.X, this.Extent.Y,

            this.Radius,
            this.Feather,
            this.StrokeMult,
            this.StrokeThr,
            this.TexType,
            this.Type,
            
        };
    }
}