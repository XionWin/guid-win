using System.Drawing;
using System.Numerics;
using Pixel.Core.Domain;

namespace Pixel.GLES;

public class Geometry: IGeometry
{
    public Geometry(IEnumerable<Vector2> points, bool is3D, Vector3 rotate, Vector2 translation)
    {
        this.Points = points;
        this.Is3D = is3D;
        this.Rotate(rotate.Z, rotate.X, rotate.Y);
        this.Transform(translation);
    }

    public IEnumerable<Vector2> Points { get; set; }
    public Matrix4x4 Matrix => this._rMatrix * this._tMatrix;
    public bool Is3D { get; protected set; }
    
    public PointF Center => 
        this.Points is IEnumerable<Vector2> points 
        ? points.GetCenter()
        : new PointF();

    public RectangleF Bound => 
        this.Points is IEnumerable<Vector2> points 
        ? points.GetBound()
        : new RectangleF();

    private Matrix4x4 _rMatrix = new Matrix4x4
    (
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1
    );
    private Matrix4x4 _tMatrix = new Matrix4x4
    (
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1
    );
    public void Rotate(float z, float x = 0, float y = 0)
    {
        this._rMatrix = 
        Matrix4x4.CreateTranslation(-this.Center.X, -this.Center.Y, 0) *
        Matrix4x4.CreateRotationX(x) *
        Matrix4x4.CreateRotationY(y) *
        Matrix4x4.CreateRotationZ(z) *
        Matrix4x4.CreateTranslation(this.Center.X, this.Center.Y, 0);
    }

    public void Transform(Vector2 point)
    {
        this._tMatrix = new Matrix4x4
        (
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            point.X, point.Y, 0, 1
        );
    }
}

static class GeometryExtension
{
    public static PointF GetCenter(this IEnumerable<Vector2> vects) => 
        new PointF((vects.Max(x => x.X) + vects.Min(x => x.X)) / 2, (vects.Max(x => x.Y) + vects.Min(x => x.Y)) / 2);

    public static RectangleF GetBound(this IEnumerable<Vector2> vects) =>
        new RectangleF(
            vects.Min(x => x.X), 
            vects.Min(x => x.Y),
            vects.Max(x => x.X) - vects.Min(x => x.X), 
            vects.Max(x => x.Y) - vects.Min(x => x.Y) 
        );
}



