namespace Pixel.Core.Domain;

public interface IGraphic<T>
{
    IRender<T>? Render { get; }
    IEnumerable<IShape>? Shapes { get; }
}


public interface IRender<T>
{
    public System.Drawing.Size Size { get; set; }
    void OnInit();

    void OnRender();

    void OnSizeChange(System.Drawing.Size size);
    
    void OnEnd();

}





