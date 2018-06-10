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

namespace GISProject
{
    public partial class Form1 : Form
    {
        GISLayer layer = null;
        GISView view = null;
        public Form1()
        {
            InitializeComponent();
            view = new GISView(new GISExtent(new GISVertex(0, 0), new GISVertex(100, 100)), ClientRectangle);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GISShapefile sf = new GISShapefile();
            layer = sf.ReadShapefile(@"D:\data\Burglaries_2009.shp");
            layer.DrawAttributeOrNot = false;
            MessageBox.Show("read " + layer.FeatureCount() + "point objects.");
        }
    }
}
