namespace Pixel.Core.Domain;

public interface IGraphic<T>
{
    IBrush<T> Background { get; set; }
    IBrush<T> StrokeColor { get; set; }

    void FillRect(Rect<T> rect);
    void StrokeRect(Rect<T> rect);
}
