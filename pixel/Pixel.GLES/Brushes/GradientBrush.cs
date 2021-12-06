using OpenTK.Mathematics;

namespace Pixel.GLES.Brush;

public abstract class GradientBrush: Brush
{
    public GradientBrush()
    {
        this.FragUniforms.ScissorScale = new Vector2(1, 1);
        this.FragUniforms.StrokeMult = 1f;
        this.FragUniforms.StrokeThr = 1f;
        this.FragUniforms.Type = 0;
    }

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