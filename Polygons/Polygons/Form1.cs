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
            double k;
            double b;
            int upPoints;
            if (Points.Count >= 3)
            {
                foreach (var i in Points)
                {
                    i.IsntInside = false;
                }
                for (int i = 0; i < Points.Count; i++)
                {
                    for (int j = i + 1; j < Points.Count; j++)
                    {
                        upPoints = 0;
                        if (Points[i].X == Points[j].X)
                        {
                            for (int z = 0; z < Points.Count; z++)
                            {
                                if (z != i && z != j)
                                {
                                    if (Points[z].X > Points[i].X) upPoints++;
                                }
                            }
                        }
                        else
                        {

                            k = ((double)Points[i].Y - Points[j].Y) / (Points[i].X - Points[j].X);
                            b = Points[i].Y - k * Points[i].X;
                            for (int z = 0; z < Points.Count; z++)
                            {
                                if (z != i && z != j)
                                {
                                    if (Points[z].Y > k * Points[z].X + b) upPoints++;
                                }
                            }
                        }
                        if (upPoints == 0 || upPoints == Points.Count - 2)
                        {
                            e.Graphics.DrawLine(new Pen(Color.Red), Points[i].X, Points[i].Y, Points[j].X, Points[j].Y);
                            Points[i].IsntInside = true;
                            Points[j].IsntInside = true;
                        }
                    }
                }
            }
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
                        Points[j].IsMoving = true;
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
                bool move = false;
                if (Points.Count >= 3)
                {
                    List<Cosmic_body> Points2 = new List<Cosmic_body>(Points);
                    Points2.Add(new Circle(e.X, e.Y));
                    foreach (var i in Points2)
                    {
                        i.IsntInside = false;
                    }
                    Cosmic_body A = Points2[0];
                    foreach (var i in Points2)
                    {
                        if (i.Y > A.Y)
                            A = i;
                        if (i.Y == A.Y)
                            if (i.X < A.X)
                                A = i;
                    }
                    Cosmic_body F = A;
                    Cosmic_body M = new Circle(A.X - 10, A.Y);
                    double minCos = 1;
                    Cosmic_body P;
                    if (Points2[0] == A)
                    {
                        P = Points2[1];
                    }
                    else
                    {
                        P = Points2[0];
                    }
                    double cos;
                    foreach (var i in Points2)
                    {
                        if (i != A && i != M)
                        {
                            double aX = M.X - A.X;
                            double bX = i.X - A.X;
                            double aY = M.Y - A.Y;
                            double bY = i.Y - A.Y;
                            cos = (aX * bX + aY * bY) / (Math.Sqrt(aX * aX + aY * aY) * Math.Sqrt(bX * bX + bY * bY));
                            if (cos < minCos)
                            {
                                minCos = cos;
                                P = i;
                            }
                        }
                    }
                    M = A;
                    A = P;
                    while (P != F)
                    {
                        minCos = 1;
                        if (Points2[0] == A)
                            P = Points2[1];
                        else
                            P = Points2[0];
                        foreach (var i in Points2)
                        {
                            if (i != A && i != M)
                            {
                                double aX = M.X - A.X;
                                double bX = i.X - A.X;
                                double aY = M.Y - A.Y;
                                double bY = i.Y - A.Y;
                                cos = (aX * bX + aY * bY) / (Math.Sqrt(aX * aX + aY * aY) * Math.Sqrt(bX * bX + bY * bY));
                                if (cos < minCos)
                                {
                                    minCos = cos;
                                    P = i;
                                }
                            }
                        }
                        A.IsntInside = true;
                        P.IsntInside = true;
                        M = A;
                        A = P;
                    }
                    for (int i = 0; i < Points2.Count; ++i)
                    {
                        if (!Points2[i].IsntInside)
                        {
                            Points2.RemoveAt(i);
                            --i;
                        }
                    }
                    move = Points.SequenceEqual(Points2);
                    if (move)
                    {
                        for (int j = 0; j < Points.Count(); ++j)
                        {
                            Points[j].IsMoving = true;
                            Points[j].Deltax = Points[j].X - e.X;
                            Points[j].Deltay = Points[j].Y - e.Y;
                        }
                    }
                }
                if (!move)
                {
                    if (pointShape == PShape.C) Points.Add(new Circle(e.X, e.Y));
                    if (pointShape == PShape.S) Points.Add(new Square(e.X, e.Y));
                    if (pointShape == PShape.T) Points.Add(new Triangle(e.X, e.Y));
                }
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
            for (int i = 0; i < Points.Count; ++i)
            {
                if (!Points[i].IsntInside)
                {
                    Points.RemoveAt(i);
                    --i;
                }
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
