using OpenTK.Mathematics;

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

        var xform = new Matrix3x4();
        xform.Row0.X = dy;
        xform.Row0.Y = -dx;
        xform.Row1.X = dx;
        xform.Row1.Y = dy;

        xform.Row2.X = this.Start.X - dx * LARGE;
        xform.Row2.Y = this.Start.Y - dy * LARGE;

        xform.Row2.Z = 1;

        // var xform = new float[6];
        // xform[0] = dy;
        // xform[1] = -dx;
        // xform[2] = dx;
        // xform[3] = dy;
        // xform[4] = this.Start.X - dx * LARGE;
        // xform[5] = this.Start.Y - dy * LARGE;



        var paintMat = TransformInverse(xform);

        var extent = new Vector2(LARGE, LARGE + d * 0.5f);

        var radius = 0.0f;

        var feather = Math.Max(1.0f, FEATHER_DEBUG ? 0 : d);

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