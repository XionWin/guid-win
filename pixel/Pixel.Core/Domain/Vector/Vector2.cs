namespace Pixel.Core.Domain.Vector;

public struct Vector2
{
    //
    // Summary:
    //     Defines a zero-length Vector3.
    public static readonly Vector2 Zero;
    //
    // Summary:
    //     Defines an instance with all components set to 1.
    public static readonly Vector2 One;
    //
    // Summary:
    //     The X component of the Vector3.
    public float X;
    //
    // Summary:
    //     The Y component of the Vector3.
    public float Y;
    
    public Vector2(float value)
    {
        this.X = this.Y = value;
    }
    public Vector2(Vector2 v)
    {
        this.X = v.X;
        this.Y = v.Y;
    }
    public Vector2(Vector3 v)
    {
        this.X = v.X;
        this.Y = v.Y;
    }
    public Vector2(Vector4 v)
    {
        this.X = v.X;
        this.Y = v.Y;
    }

    public Vector2(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }

    public float this[int index]
    {
        get => 
            index switch
            {
                0 => this.X,
                1 => this.Y,
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
                default:
                    throw new InvalidOperationException();
            }
        }
    }
    
}
