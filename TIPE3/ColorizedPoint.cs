using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace TIPE3
{
    public struct ColorizedPoint
    {
        public Vector2 point;
        public Color color;
        public ColorizedPoint(Vector2 point, Color c)
        {
            this.point = point;
            color = c;
        }
    }
}
