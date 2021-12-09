using OpenTK.Mathematics;

namespace Pixel.GLES.Brushes;

public abstract class GradientBrush: Brush
{
    public GradientBrush()
    {
        this.FragUniforms.ScissorScale = new Vector2(1, 1);
        this.FragUniforms.StrokeMult = 1f;
        this.FragUniforms.StrokeThr = 1f;
        this.FragUniforms.Type = 0;
    }

}