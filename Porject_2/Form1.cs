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

namespace Project_2
{
    public partial class Form1 : Form
    {
        List<GISFeature> features = new List<GISFeature>();
        public Form1()
        {            
            InitializeComponent();
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
            oneFeature.Draw(graphics, true, 0);
        }
        /*
         * 鼠标点击出现点属性
         */
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            GISVertex oneVertex = new GISVertex((double)e.X, (double)e.Y);
            double minDistance = Double.MaxValue;
            int findId = -1;
            //计算点击位置与features数组中的哪个元素的中心点最近
            for (int i = 0; i < features.Count; i++)
            {
                double distance = features[i].spatialPart.centroId.Distance(oneVertex);
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
                MessageBox.Show(features[findId].GetAttribute(0).ToString());
            }
        }
    }
}
