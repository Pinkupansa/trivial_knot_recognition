using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace TIPE3
{
    //Fonctions utiles
    public static class Utilities
    {
        public static List<ColorizedPoint> PlotLine(Vector2 start, Vector2 end, Color c)
        {
            List<ColorizedPoint> line = new List<ColorizedPoint>();
            Vector2 dir = Vector2.Direction(start, end);
            Vector2 currentPoint = start;
            while (Vector2.Distance(currentPoint, start) < Vector2.Distance(end, start))
            {
                line.Add(new ColorizedPoint(currentPoint, c));
                currentPoint = currentPoint + dir;
            }
            return line;
        }
        
        public static List<ColorizedPoint> PlotBezier(Vector2 p0, Vector2 p1, Vector2 p2, Color c)
        {
            List<ColorizedPoint> bezierCurve = new List<ColorizedPoint>();
            for (int i = 0; i < 1000; i++)
            {
                double t = (double)i / (double)1000;
                Vector2 newPoint = p0 * Math.Pow(1 - t, 2) + 2 * p1 * t * (1 - t) + p2 * Math.Pow(t, 2);
                bezierCurve.Add(new ColorizedPoint(newPoint,c));

            }
            return bezierCurve;
        }
        //L'opérateur % en c# est défaillant pour les nombres négatifs
        public static int mod(int x, int m)
        {
            return (x % m + m) % m;
        }
        public static Vector2 ClosestPointOfCurve(Vector2 point, List<Vector2> curve)
        {
            Vector2 closestPoint = curve[0];
            foreach(Vector2 v in curve)
            {
                if(Vector2.Distance(closestPoint,point) > Vector2.Distance(point, v))
                {
                    closestPoint = v;
                }
            }
            return closestPoint;
        }
        
    }
}
