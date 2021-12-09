using OpenTK.Mathematics;

namespace Pixel.GLES.Brushes;

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
        var extent = new Vector2(r, r);
        var radius = r;
        var feather = Math.Max(1.0f, FEATHER_DEBUG ? 0 : f);
        
        this.FragUniforms.PaintMat = paintMat;
        this.FragUniforms.ScissorExt = extent;
        this.FragUniforms.Extent = extent;
        this.FragUniforms.Radius = radius;
        this.FragUniforms.Feather = feather;

        return this.FragUniforms.GetData();
    }

}