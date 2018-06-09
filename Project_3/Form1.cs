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

namespace Project_3
{
    public partial class Form1 : Form
    {
        GISView view = null;
        List<GISFeature> features = new List<GISFeature>();
        public Form1()
        {            
            InitializeComponent();
            view = new GISView(new GISExtent(new GISVertex(0, 0), new GISVertex(100, 100)), ClientRectangle);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //获取空间信息
            double x = Convert.ToDouble(textBox1.Text);
            double y = Convert.ToDouble(textBox2.Text);
            GISVertex oneVertex = new GISVertex(x, y);
            GISPoint onePoint = new GISPoint(oneVertex);
            //获取属性信息
            string attribute = textBox3.Text;
            GISAttribute oneAttribute = new GISAttribute();
            oneAttribute.AddValue(attribute);
            //新建一个GISFeature，并添加到数组“features”中
            GISFeature oneFeature = new GISFeature(onePoint, oneAttribute);
            features.Add(oneFeature);
            //把新的GISFeature画出来
            Graphics graphics = this.CreateGraphics();
            oneFeature.Draw(graphics, view, true, 0);
        }
        /*
         * 鼠标点击出现点属性
         */
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //计算点击位置的地理坐标信息
            GISVertex mouseLocation = view.ToMapVertex(new Point(e.X, e.Y));
            double minDistance = Double.MaxValue;
            int id = -1;
            //计算点击位置与features数组中的哪个元素的中心点最近
            for (int i = 0; i < features.Count; i++)
            {
                double distance = features[i].spatialPart.centroId.Distance(mouseLocation);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    id = i;
                }
            }

            //判断是否存在空间对象
            if(id == -1){
                MessageBox.Show("没有任何空间对象");
                return;
            }
            Point nearestPoint = view.ToScreenPoint(features[id].spatialPart.centroId);
            //本来应该用平方根求距离。但距离本来小，计算成本高。直接差值求和，虽不精确，但效率高
            int screenDistance = Math.Abs(nearestPoint.X - e.X) + Math.Abs(nearestPoint.Y - e.Y);
            if(screenDistance>5)
            {
                MessageBox.Show("请靠近空间对象点击");
                return;
            }
            MessageBox.Show("该空间对象属性是：" + features[id].GetAttribute(0));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //从文本框获取新的地图范围
            double minx = Double.Parse(textBox4.Text);
            double miny = Double.Parse(textBox5.Text);
            double maxx = Double.Parse(textBox6.Text);
            double maxy = Double.Parse(textBox7.Text);
            //更新view
            view.Update(new GISExtent(minx, maxx, miny, maxy), ClientRectangle);
            Graphics graphics = CreateGraphics();
            //用黑色填充整个窗口
            graphics.FillRectangle(new SolidBrush(Color.Black), ClientRectangle);
            //根据新的view在绘图窗口中画上数组中的每一个空间对象
            for(int i = 0; i < features.Count; i++)
            {
                features[i].Draw(graphics, view, true, 0);
            }
        }
    }
}
