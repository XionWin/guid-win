namespace Pixel.GLES.Brush;

public class RadialGradientBrush: GradientBrush
{
    public Pixel.Core.Domain.Color<float> Color1
    {
        get => this.FragUniforms.InnerCol;
        set => this.FragUniforms.InnerCol = value;
    }
    public Pixel.Core.Domain.Color<float> Color2
    {
        get => this.FragUniforms.OuterCol;
        set => this.FragUniforms.OuterCol = value;
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

        var xform = new float[6];
        xform[0] = 1;
        xform[1] = 0;
        xform[2] = 0;
        xform[3] = 1;
        xform[4] = Center.X;
        xform[5] = Center.Y;


        var extent = new float[2];
        extent[0] = r;
        extent[1] = r;

        var radius = r;

        var feather = Math.Max(1.0f, FEATHER_DEBUG ? 0 : f);

        var invxform = new float[6];

        TransformInverse(invxform, xform);

        var paintMat = new float[12];
        xformToMat3x4(paintMat, invxform);
        
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