using Pixel.Core.Domain.Shape;

namespace Pixel.Core.Domain;

public interface IGraphic
{
    IRender Render { get; }
    IEnumerable<IShape> Shapes { get; }
}


public interface IRender
{
    public System.Drawing.Size Size { get; set; }
    void OnInit();

    void OnRender();

    void OnSizeChange(System.Drawing.Size size);
    
    void OnEnd();

}





