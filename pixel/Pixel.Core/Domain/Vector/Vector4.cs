using System.Runtime.InteropServices;

namespace Pixel.Core.Domain.Vector;

public struct Vector4
{
    public static readonly Vector4 Zero;
    //
    // Summary:
    //     Defines an instance with all components set to 1.
    public static readonly Vector4 One;

    static Vector4()
    {
        Zero = new Vector4(0, 0, 0, 0);
        One = new Vector4(1, 1, 1, 1);
    }

    //
    // Summary:
    //     The X component of the Vector4.
    public float X;
    //
    // Summary:
    //     The Y component of the Vector4.
    public float Y;
    //
    // Summary:
    //     The Z component of the Vector4.
    public float Z;
    //
    // Summary:
    //     The W component of the Vector4.
    public float W;

    public Vector4(float value)
    {
        this.X = this.Y = this.Z = this.W = value;
    }
    public Vector4(Vector2 v)
    {
        this.X = v.X;
        this.Y = v.Y;
        this.Z = this.W = 0;
    }
    public Vector4(Vector3 v)
    {
        this.X = v.X;
        this.Y = v.Y;
        this.Z = v.Z;
        this.W = 0;
    }
    public Vector4(Vector4 v)
    {
        this.X = v.X;
        this.Y = v.Y;
        this.Z = v.Z;
        this.W = v.W;
    }

    public Vector4(float x, float y, float z, float w)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.W = w;
    }
    public float this[int index]
    {
        get =>
            index switch
            {
                0 => this.X,
                1 => this.Y,
                2 => this.Z,
                3 => this.W,
                _ => throw new InvalidOperationException(),
            };
        set
        {
            switch (index)
            {
                case 0:
                    this.X = value;
                    break;
                case 1:
                    this.Y = value;
                    break;
                case 2:
                    this.Z = value;
                    break;
                case 3:
                    this.W = value;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
    public Vector4 Wyzx
    {
        get => new Vector4(this.W, this.Y, this.Z, this.X);
        set
        {
            this.W = value.X;
            this.Y = value.Y;
            this.Z = value.Z;
            this.X = value.W;
        }
    }
    public Vector4 Wyxz
    {
        get => new Vector4(this.W, this.Y, this.X, this.Z);
        set
        {
            this.W = value.X;
            this.Y = value.Y;
            this.X = value.Z;
            this.Z = value.W;
        }
    }
    public Vector4 Wxzy
    {
        get => new Vector4(this.W, this.X, this.Z, this.Y);
        set
        {
            this.W = value.X;
            this.X = value.Y;
            this.Z = value.Z;
            this.Y = value.W;
        }
    }
    public Vector4 Wxyz
    {
        get => new Vector4(this.W, this.X, this.Y, this.Z);
        set
        {
            this.W = value.X;
            this.X = value.Y;
            this.Y = value.Z;
            this.Z = value.W;
        }
    }
    public Vector4 Zwyx
    {
        get => new Vector4(this.Z, this.W, this.Y, this.X);
        set
        {
            this.Z = value.X;
            this.W = value.Y;
            this.Y = value.Z;
            this.X = value.W;
        }
    }
    public Vector4 Zwxy
    {
        get => new Vector4(this.Z, this.W, this.X, this.Y);
        set
        {
            this.Z = value.X;
            this.W = value.Y;
            this.X = value.Z;
            this.Y = value.W;
        }
    }
    public Vector4 Zywx
    {
        get => new Vector4(this.Z, this.Y, this.W, this.X);
        set
        {
            this.Z = value.X;
            this.Y = value.Y;
            this.W = value.Z;
            this.X = value.W;
        }
    }
    public Vector4 Zyxw
    {
        get => new Vector4(this.Z, this.Y, this.X, this.W);
        set
        {
            this.Z = value.X;
            this.Y = value.Y;
            this.X = value.Z;
            this.W = value.W;
        }
    }
    public Vector4 Zxwy
    {
        get => new Vector4(this.Z, this.X, this.W, this.Y);
        set
        {
            this.Z = value.X;
            this.X = value.Y;
            this.W = value.Z;
            this.Y = value.W;
        }
    }
    public Vector4 Zxyw
    {
        get => new Vector4(this.Z, this.X, this.Y, this.W);
        set
        {
            this.Z = value.X;
            this.X = value.Y;
            this.Y = value.Z;
            this.W = value.W;
        }
    }
    public Vector4 Ywzx
    {
        get => new Vector4(this.Y, this.W, this.Z, this.X);
        set
        {
            this.Y = value.X;
            this.W = value.Y;
            this.Z = value.Z;
            this.X = value.W;
        }
    }
    public Vector4 Ywxz
    {
        get => new Vector4(this.Y, this.W, this.X, this.Z);
        set
        {
            this.Y = value.X;
            this.W = value.Y;
            this.X = value.Z;
            this.Z = value.W;
        }
    }
    public Vector4 Yzwx
    {
        get => new Vector4(this.Y, this.Z, this.W, this.X);
        set
        {
            this.Y = value.X;
            this.Z = value.Y;
            this.W = value.Z;
            this.X = value.W;
        }
    }
    public Vector4 Yzxw
    {
        get => new Vector4(this.Y, this.Z, this.X, this.W);
        set
        {
            this.Y = value.X;
            this.Z = value.Y;
            this.X = value.Z;
            this.W = value.W;
        }
    }
    public Vector4 Yxwz
    {
        get => new Vector4(this.Y, this.X, this.W, this.Z);
        set
        {
            this.Y = value.X;
            this.X = value.Y;
            this.W = value.Z;
            this.Z = value.W;
        }
    }
    public Vector4 Yxzw
    {
        get => new Vector4(this.Y, this.X, this.Z, this.W);
        set
        {
            this.Y = value.X;
            this.X = value.Y;
            this.Z = value.Z;
            this.W = value.W;
        }
    }
    public Vector4 Xwzy
    {
        get => new Vector4(this.X, this.W, this.Z, this.Y);
        set
        {
            this.X = value.X;
            this.W = value.Y;
            this.Z = value.Z;
            this.Y = value.W;
        }
    }
    public Vector4 Xwyz
    {
        get => new Vector4(this.X, this.W, this.Y, this.Z);
        set
        {
            this.X = value.X;
            this.W = value.Y;
            this.Y = value.Z;
            this.Z = value.W;
        }
    }
    public Vector4 Xzyw
    {
        get => new Vector4(this.X, this.Z, this.Y, this.W);
        set
        {
            this.X = value.X;
            this.Z = value.Y;
            this.Y = value.Z;
            this.W = value.W;
        }
    }
    public Vector4 Xywz
    {
        get => new Vector4(this.X, this.Y, this.W, this.Z);
        set
        {
            this.X = value.X;
            this.Y = value.Y;
            this.W = value.Z;
            this.Z = value.W;
        }
    }
    public Vector4 Wzxy
    {
        get => new Vector4(this.W, this.Z, this.X, this.Y);
        set
        {
            this.W = value.X;
            this.Z = value.Y;
            this.X = value.Z;
            this.Y = value.W;
        }
    }
    public Vector4 Xzwy
    {
        get => new Vector4(this.X, this.Z, this.W, this.Y);
        set
        {
            this.X = value.X;
            this.Z = value.Y;
            this.W = value.Z;
            this.Y = value.W;
        }
    }
    public Vector4 Wzyx
    {
        get => new Vector4(this.W, this.Z, this.Y, this.X);
        set
        {
            this.W = value.X;
            this.Z = value.Y;
            this.Y = value.Z;
            this.X = value.W;
        }
    }

    public Vector3 Yzx
    {
        get => new Vector3(this.Y, this.Z, this.X);
        set
        {
            this.Y = value.X;
            this.Z = value.Y;
            this.X = value.Z;
        }
    }

    public Vector3 Yxz
    {
        get => new Vector3(this.Y, this.X, this.Z);
        set
        {
            this.Y = value.X;
            this.X = value.Y;
            this.Z = value.Z;
        }
    }

    public Vector3 Xzy
    {
        get => new Vector3(this.X, this.Z, this.Y);
        set
        {
            this.X = value.X;
            this.Z = value.Y;
            this.Y = value.Z;
        }
    }

    public Vector3 Xyz
    {
        get => new Vector3(this.X, this.Y, this.Z);
        set
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = value.Z;
        }
    }

    public Vector3 Zxy
    {
        get => new Vector3(this.Z, this.X, this.Y);
        set
        {
            this.Z = value.X;
            this.X = value.Y;
            this.Y = value.Z;
        }
    }
    public Vector3 Zyx
    {
        get => new Vector3(this.Z, this.Y, this.X);
        set
        {
            this.Z = value.X;
            this.Y = value.Y;
            this.X = value.Z;
        }
    }
    public Vector2 Zy
    {
        get => new Vector2(this.Z, this.Y);
        set
        {
            this.Z = value.X;
            this.Y = value.Y;
        }
    }
    public Vector2 Zx
    {
        get => new Vector2(this.Z, this.X);
        set
        {
            this.Z = value.X;
            this.X = value.Y;
        }
    }
    public Vector2 Yz
    {
        get => new Vector2(this.Y, this.Z);
        set
        {
            this.Y = value.X;
            this.Z = value.Y;
        }
    }
    public Vector2 Yx
    {
        get => new Vector2(this.Y, this.X);
        set
        {
            this.Y = value.X;
            this.X = value.Y;
        }
    }
    public Vector2 Xz
    {
        get => new Vector2(this.X, this.Z);
        set
        {
            this.X = value.X;
            this.Z = value.Y;
        }
    }
    public Vector2 Xy
    {
        get => new Vector2(this.X, this.Y);
        set
        {
            this.X = value.X;
            this.Y = value.Y;
        }
    }
}
