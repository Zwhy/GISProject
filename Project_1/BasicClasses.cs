using System.Collections.Generic;
using System.Drawing;

namespace MyGIS
{
    /*
     * 节点
     */
     class GISVertex
    {
        public double x;
        public double y;

        public GISVertex(double _x, double _y)
        {
            x = _x;
            y = _y;
        }
        public double Distance(GISVertex anotherVertex)
        {
            return System.Math.Sqrt((x - anotherVertex.x) * (x - anotherVertex.x)
                + (y - anotherVertex.y) * (y - anotherVertex.y));
        }
    }
    /*
     * 实体点
     */
     class GISPoint
    {
        public GISVertex Location;
        public string Attribute;

        public GISPoint(GISVertex oneVertex, string oneString)
        {
            Location = oneVertex;
            Attribute = oneString;
        }
        public void DrawPoint(Graphics graphics)
        {
            graphics.FillEllipse(new SolidBrush(Color.Red),
                new Rectangle((int)(Location.x) - 3, (int)(Location.y) - 3, 6, 6));
        }
        public void DrawAttribute(Graphics graphics)
        {
            graphics.DrawString(Attribute, new Font("宋体", 20), new SolidBrush(Color.Green),
                                new PointF((int)(Location.x), (int)(Location.y)));
        }
        public double Distance(GISVertex anotherVertex)
        {
            return Location.Distance(anotherVertex);
        }
    }
    /*
     * 线实体
     */
    class GISLine
    {
        List<GISVertex> AllVertexs;
    }
    /**
     * 面实体
     */
    class GISPolygon
    {
        List<GISVertex> AllVertexs;
    }
}