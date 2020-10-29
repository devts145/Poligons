using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Polygons
{
    public class Square : Cosmic_body
    {
        public Square(int x, int y) : base(x, y)
        {

        }
        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(new SolidBrush(Cosmic_body.Color), (int)(x - R / Math.Sqrt(2)), (int)(y - R / Math.Sqrt(2)), (int)(2 * R / Math.Sqrt(2)), (int)(2 * R / Math.Sqrt(2)));
        }
        public override bool CheckIsInside(int x1, int y1)
        {
            double a = R / Math.Sqrt(2);
            return ((x1 <= x + a) && (x1 >= x - a) && (y1 >= y - a) && (y1 <= y + a));
        }
    }
}
