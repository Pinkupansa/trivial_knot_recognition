using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace TIPE3
{
    class Brin
    {
        public Vector2[] brin;
        public Color color;
        public Vector2 this[int i]
        {
            get
            {
                return brin[i];
            }
        }
        public int Length
        {
            get
            {
                return brin.Length;
            }
        }
        public Brin(Vector2[] brin, Color c)
        {
            this.brin = brin;
            color = c;
        }
    }
}
