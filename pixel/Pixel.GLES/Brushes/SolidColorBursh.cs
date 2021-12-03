namespace Pixel.GLES.Brush;

public class SolidColorBursh: Pixel.Core.Domain.IBrush<float>
{

    public GLFragUniforms FragUniforms { get; } = new GLFragUniforms();

    public Pixel.Core.Domain.Color<float> Color
    {
        get => this.FragUniforms.InnerCol;
        set => this.FragUniforms.InnerCol = value;
    }

    public SolidColorBursh()
    {

        this.FragUniforms.PaintMat = new float[] {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0};
        this.FragUniforms.ScissorExt = new float[]{1, 1};
        this.FragUniforms.Extent = new float[]{1, 1};
        this.FragUniforms.Radius = 0;
        this.FragUniforms.Feather = 0;
        this.FragUniforms.ScissorScale = new float[]{1, 1};
        this.FragUniforms.StrokeMult = 1f;
        this.FragUniforms.StrokeThr = 1f;
        this.FragUniforms.Type = 2;
    }

    public float[] GetData()
    {
        this.FragUniforms.InnerCol = this.FragUniforms.OuterCol = this.FragUniforms.InnerCol;
        return this.FragUniforms.GetData();
    }
    
}