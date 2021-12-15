using Pixel.Core.Domain.Vector;

namespace Pixel.Core.Domain.Matrix;

public struct Matrix2x3
{
    public static readonly Matrix2x3 Zero;
    public static readonly Matrix2x3 One;

    static Matrix2x3()
    {
        Zero = new Matrix2x3(0, 0, 0, 0, 0, 0);
        One = new Matrix2x3(1, 0, 0, 0, 1, 0);
    }
    
    public Vector3 Row0;
    
    public Vector3 Row1;

    
        public Matrix2x3(Vector3 row0, Vector3 row1)
        {
            this.Row0 = row0;
            this.Row1 = row1;
        }
        public Matrix2x3(float m00, float m01, float m02, float m10, float m11, float m12)
        {
            this.Row0 = new Vector3(m00, m01, m02);
            this.Row1 = new Vector3(m10, m11, m12);
        }
        public float this[int rowIndex, int columnIndex]
        {
            get => 
                rowIndex switch
                {
                    0 => this.Row0[columnIndex],
                    1 => this.Row1[columnIndex],
                    _ => throw new InvalidOperationException(),
                };
            set
            {
                switch (rowIndex)
                {
                    case 0:
                        this.Row0[columnIndex] = value;
                        break;
                    case 1:
                        this.Row1[columnIndex] = value;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public Vector2 Column0
        {
            get => new Vector2(M11, M21);
            set
            {
                this.M11 = value.X;
                this.M21 = value.Y;
            }
        }
        public Vector2 Column1
        {
            get => new Vector2(M12, M22);
            set
            {
                this.M12 = value.X;
                this.M22 = value.Y;
            }
        }
        public Vector2 Column2
        {
            get => new Vector2(M13, M23);
            set
            {
                this.M13 = value.X;
                this.M23 = value.Y;
            }
        }
        
        public float M11
        {
            get => this.Row0[0];
            set => this.Row0[0] = value;
        }
        public float M12
        {
            get => this.Row0[1];
            set => this.Row0[1] = value;
        }
        public float M13
        {
            get => this.Row0[2];
            set => this.Row0[2] = value;
        }
        public float M21
        {
            get => this.Row1[0];
            set => this.Row1[0] = value;
        }
        public float M22
        {
            get => this.Row1[1];
            set => this.Row1[1] = value;
        }
        public float M23
        {
            get => this.Row1[2];
            set => this.Row1[2] = value;
        }
}