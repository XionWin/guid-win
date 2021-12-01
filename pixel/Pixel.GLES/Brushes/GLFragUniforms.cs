using System.Runtime.InteropServices;

namespace Pixel.GLES.Brush;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GLFragUniforms
{
    public const int UNIFORMARRAY_SIZE = 11;
    
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
    private float[] scissorMat = new float[12];
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
    private float[] paintMat = new float[12];
    private Core.Domain.Color<float> innerCol;
    private Core.Domain.Color<float> outerCol;
    // float[2]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    private float[] scissorExt = new float[2];
    // float[2]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    private float[] scissorScale = new float[2];
    // float[2]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    private float[] extent = new float[2];
    private float radius;
    private float feather;
    private float strokeMult;
    private float strokeThr;
    private float texType;
    private float type;

    public static int Size => Marshal.SizeOf(typeof(GLFragUniforms));

    public float[] ScissorMat { get => this.scissorMat; set => this.scissorMat = value; }
    public float[] PaintMat { get => this.paintMat; set => this.paintMat = value; }
    public float[] ScissorExt { get => this.scissorExt; set => this.scissorExt = value; }
    public float[] ScissorScale { get => this.scissorScale; set => this.scissorScale = value; }
    public float[] Extent { get => this.extent; set => this.extent = value; }
    public Core.Domain.Color<float> InnerCol { get => this.innerCol; set => this.innerCol = value; }
    public Core.Domain.Color<float> OuterCol { get => this.outerCol; set => this.outerCol = value; }
    public float Radius { get => this.radius; set => this.radius = value; }
    public float Feather { get => this.feather; set => this.feather = value; }
    public float StrokeMult { get => this.strokeMult; set => this.strokeMult = value; }
    public float StrokeThr { get => this.strokeThr; set => this.strokeThr = value; }
    public float TexType { get => this.texType; set => this.texType = value; }
    public float Type { get => this.type; set => this.type = value; }

    public float[] GetData()
    {
        int size = GLFragUniforms_2.Size;
        int felements = (int)Math.Ceiling((float)(size / sizeof(float)));
        float[] farr = new float[felements];

        nint ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(this, ptr, true);
        Marshal.Copy(ptr, farr, 0, felements);
        Marshal.FreeHGlobal(ptr);
        return farr;
    }
}