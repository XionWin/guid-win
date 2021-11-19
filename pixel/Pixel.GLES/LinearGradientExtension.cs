namespace Pixel.GLES;

public partial class Extension
{
    private static bool FEATHER_DEBUG = true;
    private const float LARGE = (float)1e5;
    private const float MIN_THRESHOLD = 0.0001f;
    public static float[] GetLinearGradient(float sx, float sy, float ex, float ey)
    {
        float dx, dy, d;

        dx = ex - sx;
        dy = ey - sy;
        d = (float)Math.Sqrt(dx * dx + dy * dy);
        if (d > MIN_THRESHOLD)
        {
            dx /= d;
            dy /= d;
        }
        else
        {
            dx = 0;
            dy = 1;
        }

        var xform = new float[6];
        xform[0] = dy;
        xform[1] = -dx;
        xform[2] = dx;
        xform[3] = dy;
        xform[4] = sx - dx * LARGE;
        xform[5] = sy - dy * LARGE;

        var extent = new float[2];
        extent[0] = LARGE;
        extent[1] = LARGE + d * 0.5f;

        var radius = 0.0f;

        var feather = Math.Max(1.0f, FEATHER_DEBUG ? 0 : d);

        var invxform = new float[6];

        TransformInverse(invxform, xform);

        var paintMat = new float[12];
        xformToMat3x4(paintMat, invxform);

        GLFragUniforms frag = new GLFragUniforms()
        {
            paintMat = paintMat,
            innerCol = new Core.Domain.Color(1f, 0.5f, 0.5f),
            outerCol = new Core.Domain.Color(0, 0, 1f),
            scissorExt = extent,
            scissorScale = new float[] { 1f, 1f },
            extent = extent,
            Radius = radius,
            Feather = feather,
            StrokeMult = 1f,
            StrokeThr = -1f,
            texType = 0,
            Type = 0,
        };

        return frag.Floats;
    }

    public static int TransformInverse(float[] inv, float[] t)
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

    static void xformToMat3x4(float[] m3, float[] t)
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