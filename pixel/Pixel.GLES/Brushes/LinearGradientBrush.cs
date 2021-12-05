namespace Pixel.GLES.Brush;

public class LinearGradientBrush: GradientBrush
{
    private Pixel.Core.Domain.Color<byte> color1;
    public Pixel.Core.Domain.Color<byte> Color1
    {
        get => this.color1;
        set
        {
            this.color1 = value;
            this.FragUniforms.InnerCol = ConvertColor(value);
        }
    }
    private Pixel.Core.Domain.Color<byte> color2;
    public Pixel.Core.Domain.Color<byte> Color2
    {
        get => this.color2;
        set
        {
            this.color2 = value;
            this.FragUniforms.OuterCol = ConvertColor(value);
        }
    }

    public System.Drawing.PointF Start { get; set; }
    public System.Drawing.PointF End { get; set; }

    public LinearGradientBrush(float sx, float sy, float ex, float ey)
    {
        this.Start = new System.Drawing.PointF(sx, sy);
        this.End = new System.Drawing.PointF(ex, ey);
    }

    public override float[] GetData()
    {
        float dx, dy, d;

        dx = this.End.X - this.Start.X;
        dy = this.End.Y - this.Start.Y;
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
        xform[4] = this.Start.X - dx * LARGE;
        xform[5] = this.Start.Y - dy * LARGE;

        var extent = new float[2];
        extent[0] = LARGE;
        extent[1] = LARGE + d * 0.5f;

        var radius = 0.0f;

        var feather = Math.Max(1.0f, FEATHER_DEBUG ? 0 : d);

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