using System.Runtime.InteropServices;

namespace Pixel.GLES;


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GLFragUniforms
{
    public const int UNIFORMARRAY_SIZE = 11;
    // matrices are actually 3 vec4s

    // float[12]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
    public float[] scissorMat;
    // float[12]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
    public float[] paintMat;
    // [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public Core.Domain.Color innerCol;
    // [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public Core.Domain.Color outerCol;
    // float[2]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public float[] scissorExt;
    // float[2]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public float[] scissorScale;
    // float[2]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public float[] extent;
    public float Radius { get; set; }
    public float Feather { get; set; }
    public float StrokeMult { get; set; }
    public float StrokeThr { get; set; }
    public float texType;
    public int TexType
    {
        get => (int)this.texType;
        set => this.texType = value;
    }
    public float type;
    public int Type
    {
        get => (int)this.type;
        set => this.type = value;
    }

    public GLFragUniforms()
    {
        scissorMat = new float[12];
        paintMat = new float[12];
        innerCol = new Core.Domain.Color();
        outerCol = new Core.Domain.Color();
        scissorExt = new float[2];
        scissorScale = new float[2];
        extent = new float[2];
    }

    public float[] Floats
    {
        get
        {
            int size = GLFragUniforms.GetSize;
            int felements = (int)Math.Ceiling((float)(size / sizeof(float)));
            float[] farr = new float[felements];

            nint ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(this, ptr, true);
            Marshal.Copy(ptr, farr, 0, felements);
            Marshal.FreeHGlobal(ptr);
            return farr;
        }
    }

    /// <summary>
    /// Gets the size of the <see cref="GLFragUniforms"/> in bytes.
    /// </summary>
    /// <value>The size of the GLNVGfragUniforms struct.</value>
    public static int GetSize => Marshal.SizeOf(typeof(GLFragUniforms));
}
