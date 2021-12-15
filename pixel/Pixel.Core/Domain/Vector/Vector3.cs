using System.Runtime.InteropServices;

namespace Pixel.Core.Domain.Vector;

public struct Vector3
{
    public static readonly Vector3 Zero;
    //
    // Summary:
    //     Defines an instance with all components set to 1.
    public static readonly Vector3 One;

    static Vector3()
    {
        Zero = new Vector3(0, 0, 0);
        One = new Vector3(1, 1, 1);
    }

    //
    // Summary:
    //     The X component of the Vector3.
    public float X;
    //
    // Summary:
    //     The Y component of the Vector3.
    public float Y;
    //
    // Summary:
    //     The Z component of the Vector3.
    public float Z;
    
    public Vector3(float value)
    {
        this.X = this.Y = this.Z = value;
    }
    public Vector3(Vector2 v)
    {
        this.X = v.X;
        this.Y = v.Y;
        this.Z = 0;
    }
    public Vector3(Vector3 v)
    {
        this.X = v.X;
        this.Y = v.Y;
        this.Z = v.Z;
    }
    public Vector3(Vector4 v)
    {
        this.X = v.X;
        this.Y = v.Y;
        this.Z = v.Z;
    }

    public Vector3(float x, float y, float z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
    public float this[int index]
    {
        get => 
            index switch
            {
                0 => this.X,
                1 => this.Y,
                2 => this.Z,
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
                default:
                    throw new InvalidOperationException();
            }
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
