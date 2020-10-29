using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Polygons
{
    abstract public class Cosmic_body
    {
        protected static int R;
        protected static Color Color;
        protected int x, y, deltax, deltay;
        protected bool isMoving;
        public Cosmic_body(int x, int y)
        {
            this.x = x;
            this.y = y;
            deltax = 0;
            deltay = 0;
            isMoving = false;
        }
        static Cosmic_body()
        {
            R = 50;
            Color = Color.Green;
        }
        public bool Drag
        {
            get { return isMoving; }
            set { isMoving = value; }
        }
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public int Deltax
        {
            get { return deltax; }
            set { deltax = value; }
        }
        public int Deltay
        {
            get { return deltay; }
            set { deltay = value; }
        }
        public abstract void Draw(Graphics graphics);
        public abstract bool CheckIsInside(int x1, int y1);
    }
}
