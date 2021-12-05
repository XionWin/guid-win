namespace Pixel.GLES.Brush;

public abstract class GradientBrush: Pixel.Core.Domain.IBrush<float>
{
    protected static bool FEATHER_DEBUG = false;
    protected const float LARGE = (float)1e5;
    protected const float MIN_THRESHOLD = 0.0001f;

    public GLFragUniforms FragUniforms { get; } = new GLFragUniforms();

    public GradientBrush()
    {
        this.FragUniforms.ScissorScale = new float[]{1, 1};
        this.FragUniforms.StrokeMult = 1f;
        this.FragUniforms.StrokeThr = 1f;
        this.FragUniforms.Type = 0;
    }
    protected Pixel.Core.Domain.Color<float> ConvertColor(Pixel.Core.Domain.Color<byte> color)
    {
        var a = color.a /255f;
        return new Core.Domain.Color<float>(color.r /255f * a, color.g  /255f * a, color.b  /255f * a,  a);
    }

    public abstract float[] GetData();
    
    protected static int TransformInverse(float[] inv, float[] t)
    {
        double invdet, det = (double)t[0] * t[3] - (double)t[2] * t[1];
        if (det > -1e-6 && det < 1e-6)
        {
            // nvgTransformIdentity(inv);
            return 0;
        }
        invdet = 1.0 / det;
        inv[0] = (float)(t[3] * invdet);
        inv[2] = (float)(-t[2] * invdet);
        inv[4] = (float)(((double)t[2] * t[5] - (double)t[3] * t[4]) * invdet);
        inv[1] = (float)(-t[1] * invdet);
        inv[3] = (float)(t[0] * invdet);
        inv[5] = (float)(((double)t[1] * t[4] - (double)t[0] * t[5]) * invdet);
        return 1;
    }

    protected static void xformToMat3x4(float[] m3, float[] t)
    {
        m3[0] = t[0];
        m3[1] = t[1];
        m3[2] = 0.0f;
        m3[3] = 0.0f;
        m3[4] = t[2];
        m3[5] = t[3];
        m3[6] = 0.0f;
        m3[7] = 0.0f;
        m3[8] = t[4];
        m3[9] = t[5];
        m3[10] = 1.0f;
        m3[11] = 0.0f;
    }
}