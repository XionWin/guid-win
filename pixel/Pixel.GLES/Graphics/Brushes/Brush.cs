using OpenTK.Mathematics;

namespace Pixel.GLES.Brushes;

public abstract class Brush: Pixel.Core.Domain.IBrush<float>
{
    protected static bool FEATHER_DEBUG = false;
    protected const float LARGE = (float)1e5;
    protected const float MIN_THRESHOLD = 0.0001f;
    public GLFragUniforms FragUniforms { get; } = new GLFragUniforms();
    public Brush()
    {
        this.FragUniforms.PaintMat = new Matrix3x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0);
        this.FragUniforms.ScissorExt = new Vector2(1, 1);
        this.FragUniforms.ScissorScale = new Vector2(1, 1);
        this.FragUniforms.Extent = new Vector2(1, 1);
        this.FragUniforms.Radius = 0;
        this.FragUniforms.Feather = 0;
        this.FragUniforms.StrokeMult = 1;
        this.FragUniforms.StrokeThr = 1;
        this.FragUniforms.Type = 2;
    }
    protected Vector4 ConvertColor(Pixel.Core.Domain.Color<byte> color)
    {
        var a = color.a /255f;
        return new Vector4(color.r /255f * a, color.g  /255f * a, color.b  /255f * a,  a);
    }

    public abstract float[] GetData();
    
    protected static Matrix3x4 TransformInverse(Matrix3x4 mat)
    {
        double invdet, det = (double)mat.Row0.X * mat.Row1.Y - (double)mat.Row1.X * mat.Row0.Y;
        invdet = 1.0 / det;

        if (det > -1e-6 && det < 1e-6)
        {
            return new Matrix3x4
            (
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0
            );
        }

        return new Matrix3x4
        (
            (float)(mat.Row1.Y * invdet), (float)(-mat.Row0.Y * invdet), 0, 0,
            (float)(-mat.Row1.X * invdet), (float)(mat.Row0.X * invdet), 0, 0,
            (float)(((double)mat.Row1.X * mat.Row2.Y - (double)mat.Row1.Y * mat.Row2.X) * invdet), (float)(((double)mat.Row0.Y * mat.Row2.X - (double)mat.Row0.X * mat.Row2.Y) * invdet), 1, 0
        );
    }
}