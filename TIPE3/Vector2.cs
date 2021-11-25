using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPE3
{
    //Définit les vecteurs et leurs opérations usuelles en 2D
    public class Vector2
    {
        public double x { get; private set; }
        public double y { get; private set; }
        public double Norm
        {
            get
            {
                return Math.Sqrt(x * x + y * y);
            }
        }
        public Vector2 normalized
        {
            get
            {
                return this / Norm;
            }
        }

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public static Vector2 operator +(Vector2 v1, Vector2 v2) => new Vector2(v1.x + v2.x, v1.y + v2.y);
        public static Vector2 operator -(Vector2 v) => new Vector2(-v.x, -v.y);
        public static Vector2 operator -(Vector2 v1, Vector2 v2) => v1 + (-v2);
        public static Vector2 operator /(Vector2 v, double p) => new Vector2(v.x / p, v.y / p);
        public static Vector2 operator *(Vector2 v, double p) => new Vector2(v.x * p, v.y * p);
        public static Vector2 operator *(double p, Vector2 v) => new Vector2(v.x * p, v.y * p);
        
        public static Vector2 Direction(Vector2 v1, Vector2 v2)
        {
            return (v2 - v1).normalized;
        }
        public static double Distance(Vector2 v1, Vector2 v2)
        {
            return (v2 - v1).Norm;
        }
        public static double CrossProduct(Vector2 v1, Vector2 v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }
    }
}
