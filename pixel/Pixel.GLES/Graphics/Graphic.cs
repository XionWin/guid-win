using System.Runtime.InteropServices;
using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using Pixel.Core.Domain;
using Pixel.GLES.Brushes;
using Pixel.GLES.Shape;

namespace Pixel.GLES.Graphics;

public partial class Graphic : Pixel.Core.Domain.IGraphic
{
    private List<IShape> shapes = new List<IShape>();

    public System.Drawing.Size? Size 
    { 
        get => this.Render?.Size; 
        set
        {
            if (this.Render is IRender render && value is System.Drawing.Size v)
            {
                render.Size = v;
            }
        } 
    }

    public IRender Render { get; } = new Render();
    public IEnumerable<IShape> Shapes => this.shapes;
}