using OpenTK.Mathematics;

namespace Pixel.GLES.Brush;

public class RadialGradientBrush: GradientBrush
{
    private Pixel.Core.Domain.Color<byte> color1;
    public Pixel.Core.Domain.Color<byte> Color1
    {
        get => this.color1;
        set
        {
            this.color1 = value;
            this.FragUniforms.InnerColor = ConvertColor(value);
        }
    }
    private Pixel.Core.Domain.Color<byte> color2;
    public Pixel.Core.Domain.Color<byte> Color2
    {
        get => this.color2;
        set
        {
            this.color2 = value;
            this.FragUniforms.OuterColor = ConvertColor(value);
        }
    }

    public System.Drawing.PointF Center { get; set; }
    public float InR { get; set; }
    public float OutR { get; set; }

    public RadialGradientBrush(float cx, float cy, float inr, float outr)
    {
        this.Center = new System.Drawing.PointF(cx, cy);
        this.InR = inr;
        this.OutR = outr;
    }

    public override float[] GetData()
    {
        float r = (InR + OutR) * 0.5f;
        float f = (OutR - InR);

        var xform = new Matrix3x4();
        xform.Row0.X = 1;
        xform.Row0.Y = 0;
        xform.Row1.X = 0;
        xform.Row1.Y = 1;

        xform.Row2.X = Center.X;
        xform.Row2.Y = Center.Y;

        xform.Row2.Z = 1;

        var paintMat = TransformInverse(xform);

        var xform1 = new float[6];
        xform1[0] = 1;
        xform1[1] = 0;
        xform1[2] = 0;
        xform1[3] = 1;
        xform1[4] = Center.X;
        xform1[5] = Center.Y;
        var invxform = new float[6];
        TransformInverse(invxform, xform1);
        var paintMat1 = new float[12];
        xformToMat3x4(paintMat1, invxform);

        var extent = new Vector2(r, r);

        var radius = r;

        var feather = Math.Max(1.0f, FEATHER_DEBUG ? 0 : f);

        
        this.FragUniforms.PaintMat = paintMat;
        // this.FragUniforms.InnerCol = new Core.Domain.Color<float>(1f, 0f, 0f, 1f);
        // this.FragUniforms.OuterCol = new Core.Domain.Color<float>(0f, 0f, 1f, 1f);
        this.FragUniforms.ScissorExt = extent;
        // this.FragUniforms.ScissorScale = new float[] { 1f, 1f };
        this.FragUniforms.Extent = extent;
        this.FragUniforms.Radius = radius;
        this.FragUniforms.Feather = feather;
        // this.FragUniforms.StrokeMult = 1f;
        // this.FragUniforms.StrokeThr = 1f;
        // this.FragUniforms.TexType = 0;
        // this.FragUniforms.Type = 0;

        return this.FragUniforms.GetData();
    }

}