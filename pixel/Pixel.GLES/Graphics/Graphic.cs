using System.Runtime.InteropServices;
using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using Pixel.Core.Domain;
using Pixel.GLES.Brushes;

namespace Pixel.GLES.Graphics;

public partial class Graphic : Pixel.Core.Domain.IGraphic<float>
{
    private List<IShape> shapes = new List<IShape>();

    public System.Drawing.Size? Size 
    { 
        get => this.Render?.Size; 
        set
        {
            if (this.Render is IRender<float> render && value is System.Drawing.Size v)
            {
                render.Size = v;
            }
        } 
    }

    public IRender<float> Render { get; } = new Render();
    public IEnumerable<IShape> Shapes => this.shapes;
}