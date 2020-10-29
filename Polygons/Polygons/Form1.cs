using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Polygons
{
    public partial class Form1 : Form
    {
        enum PShape { C, S, T }
        PShape pointShape = PShape.C;
        bool isNotInside;
        List<Cosmic_body> Points = new List<Cosmic_body>();
        public Form1()
        {
            DoubleBuffered = true; // с ним отрисовывается красиво, без лагов и мерцаний
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var i in Points)
            {
                i.Draw(e.Graphics);
            }
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            isNotInside = true;
            for (int j = Points.Count - 1; j >= 0; j--)
            {
                if (Points[j].CheckIsInside(e.X, e.Y))
                {
                    isNotInside = false;
                    if ((e.Button & MouseButtons.Right) == 0)
                    {
                        Points[j].Drag = true;
                        Points[j].Deltax = Points[j].X - e.X;
                        Points[j].Deltay = Points[j].Y - e.Y;
                    }
                    else
                    {
                        Points.RemoveAt(j);
                        break;
                    }
                }
            }
            if ((e.Button & MouseButtons.Right) == 0 && isNotInside)
            {
                if (pointShape == PShape.C) Points.Add(new Circle(e.X, e.Y));
                if (pointShape == PShape.S) Points.Add(new Square(e.X, e.Y));
                if (pointShape == PShape.T) Points.Add(new Triangle(e.X, e.Y));
            }
            this.Invalidate();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (var i in Points)
            {
                if (i.Drag)
                {
                    i.X = e.X + i.Deltax;
                    i.Y = e.Y + i.Deltay;
                }
            }
            this.Invalidate();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (var i in Points)
            {
                i.Drag = false;
                i.Deltax = 0;
                i.Deltay = 0;
            }
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pointShape = PShape.C;
            circleToolStripMenuItem.Checked = true;
            squareToolStripMenuItem.Checked = false;
            triangleToolStripMenuItem.Checked = false;
        }

        private void squareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pointShape = PShape.S;
            circleToolStripMenuItem.Checked = false;
            squareToolStripMenuItem.Checked = true;
            triangleToolStripMenuItem.Checked = false;
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pointShape = PShape.T;
            circleToolStripMenuItem.Checked = false;
            squareToolStripMenuItem.Checked = false;
            triangleToolStripMenuItem.Checked = true;
        }
    }
}
