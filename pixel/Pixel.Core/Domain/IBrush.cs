namespace Pixel.Core.Domain;

public interface IBrush
{
    static IBrush? Default { get; }
    float[] GetData();
}

