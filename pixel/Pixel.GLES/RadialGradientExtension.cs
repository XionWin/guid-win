namespace Pixel.GLES;

public partial class Extension
{
    public static float[] GetRadialGradient(float cx, float cy, float inr, float outr)
    {
        float r = (inr + outr) * 0.5f;
        float f = (outr - inr);

        var xform = new float[6];
        xform[0] = 1;
        xform[1] = 0;
        xform[2] = 0;
        xform[3] = 1;
        xform[4] = cx;
        xform[5] = cy;


        var extent = new float[2];
        extent[0] = r;
        extent[1] = r;

        var radius = r;

        var feather = Math.Max(1.0f, FEATHER_DEBUG ? 0 : f);

        var invxform = new float[6];

        TransformInverse(invxform, xform);

        var paintMat = new float[12];
        xformToMat3x4(paintMat, invxform);

        GLFragUniforms frag = new GLFragUniforms()
        {
            PaintMat = paintMat,
            InnerCol = new Core.Domain.Color<float>(1f, 0.0f, 0.0f, 0.1f),
            OuterCol = new Core.Domain.Color<float>(0, 0, 1f, 0.1f),
            ScissorExt = extent,
            ScissorScale = new float[] { 1f, 1f },
            Extent = extent,
            Radius = radius,
            Feather = feather,
            StrokeMult = 1f,
            StrokeThr = 1f,
            TexType = 0,
            Type = 0,
        };

        return frag.Floats;
    }

}