using System.Drawing;

public static class RectExtension
{
    public static PointF TopLeft(this RectangleF rect) => new PointF(rect.X, rect.Y);
    public static PointF TopRight(this RectangleF rect) => new PointF(rect.X + rect.Width, rect.Y);
    public static PointF BottomLeft(this RectangleF rect) => new PointF(rect.X, rect.Y + rect.Height);
    public static PointF BottomRight(this RectangleF rect) => new PointF(rect.X + rect.Width, rect.Y + rect.Height);

    public static PointF Center(this RectangleF rect) => new System.Drawing.PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
}