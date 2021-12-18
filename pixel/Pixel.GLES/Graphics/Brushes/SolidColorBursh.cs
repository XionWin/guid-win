using OpenTK.Mathematics;

namespace Pixel.GLES.Brushes;

public class SolidColorBursh: Pixel.Core.Domain.IBrush
{
    public GLFragUniforms FragUniforms { get; } = new GLFragUniforms();

    private Pixel.Core.Domain.Color color;
    public Pixel.Core.Domain.Color Color
    {
        get => this.color;
        set
        {
            this.color = value;
            this.FragUniforms.InnerColor = this.FragUniforms.OuterColor = ConvertColor(value);
        }
    }

    public SolidColorBursh()
    {
        this.FragUniforms.PaintMat = new Matrix3x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0);
        this.FragUniforms.ScissorExt = new Vector2(1, 1);
        this.FragUniforms.ScissorScale = new Vector2(1, 1);
        this.FragUniforms.Extent = new Vector2(1, 1);
        this.FragUniforms.Radius = 0;
        this.FragUniforms.Feather = 0;
        this.FragUniforms.StrokeMult = 1;
        this.FragUniforms.StrokeThr = 1;
        this.FragUniforms.Type = 2;
    }

    public float[] GetData()
    {
        return this.FragUniforms.GetData();
    }
    
    protected Vector4 ConvertColor(Pixel.Core.Domain.Color color)
    {
        var a = color.a /255f;
        return new Vector4(color.r /255f * a, color.g  /255f * a, color.b  /255f * a,  a);
    }
    
}