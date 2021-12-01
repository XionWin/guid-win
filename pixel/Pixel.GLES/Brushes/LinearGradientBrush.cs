namespace Pixel.GLES.Brush;

public class LinearGradientBrush: GradientBrush
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

    
    public System.Drawing.Point Start { get; set; }
    public System.Drawing.Point End { get; set; }

    public LinearGradientBrush(int sx, int sy, int ex, int ey)
    {
        this.Start = new System.Drawing.Point(sx, sy);
        this.End = new System.Drawing.Point(ex, ey);
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
        this.FragUniforms.ScissorScale = new float[] { 1f, 1f };
        this.FragUniforms.Extent = extent;
        this.FragUniforms.Radius = radius;
        this.FragUniforms.Feather = feather;
        this.FragUniforms.StrokeMult = 1f;
        this.FragUniforms.StrokeThr = 1f;
        this.FragUniforms.TexType = 0;
        this.FragUniforms.Type = 0;

        return this.FragUniforms.GetData();
    }

}