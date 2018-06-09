using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyGIS;

namespace Project_1
{
    public partial class Form1 : Form
    {
        List<GISPoint> points = new List<GISPoint>();
        public Form1()
        {            
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double x = Convert.ToDouble(textBox1.Text);
            double y = Convert.ToDouble(textBox2.Text);
            string attribute = textBox3.Text;
            GISVertex oneVertex = new GISVertex(x, y);
            GISPoint onePoint = new GISPoint(oneVertex, attribute);
            Graphics graphics = this.CreateGraphics();
            onePoint.DrawPoint(graphics);
            onePoint.DrawAttribute(graphics);
            points.Add(onePoint);
        }
        /*
         * 鼠标点击出现点属性
         */
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            GISVertex oneVertex = new GISVertex((double)e.X, (double)e.Y);
            double minDistance = Double.MaxValue;
            int findId = -1;
            for (int i = 0; i < points.Count; i++)
            {
                double distance = points[i].Distance(oneVertex);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    findId = i;
                }
            }
            if (minDistance > 5 || findId == -1)
            {
                MessageBox.Show("没有实体点或者鼠标点击位置不准确！");
            }
            else
            {
                MessageBox.Show(points[findId].Attribute);
            }
        }
    }
}
