namespace Pixel.Core.Domain;

public interface IGraphic
{
    IRender Render { get; }
    Dictionary<IShape, IEnumerable<IGeometry>> ShapeGeometries { get; }
    IEnumerable<IShape> Shapes { get; }
    IEnumerable<IGeometry> Geometries { get; }
    void Add(IShape shape);
}


public interface IRender
{
    IGraphic Graphic { get; }
    System.Drawing.Size Size { get; set; }
    void OnInit();

    void OnRender();

    void OnSizeChange(System.Drawing.Size size);
    
    void OnEnd();

}





