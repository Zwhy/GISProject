using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace MyGIS
{
    /*
     * 节点
     */
    public class GISVertex
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
     class GISPoint: GISSpatial
    {
        public GISPoint(GISVertex oneVertex)
        {
            centroId = oneVertex;
            extent = new GISExtent(oneVertex, oneVertex);
        }
        public override void Draw(Graphics graphics)
        {
            graphics.FillEllipse(new SolidBrush(Color.Red),
                new Rectangle((int)(centroId.x) - 3, (int)(centroId.y) - 3, 6, 6));
        }
        public double Distance(GISVertex anotherVertex)
        {
            return centroId.Distance(anotherVertex);
        }
    }
    /*
     * 线实体
     */
    class GISLine: GISSpatial
    {
        List<GISVertex> AllVertexs;
        public override void Draw(Graphics graphics)
        {
            throw new System.NotImplementedException();
        }
    }
    /**
     * 面实体
     */
    class GISPolygon: GISSpatial
    {
        List<GISVertex> AllVertexs;
        public override void Draw(Graphics graphics)
        {
            throw new System.NotImplementedException();
        }
    }

    class GISFeature
    {
        public GISSpatial spatialPart;
        public GISAttribute attributePart;

        public GISFeature(GISSpatial spatialPart, GISAttribute attributePart)
        {
            this.spatialPart = spatialPart;
            this.attributePart = attributePart;
        }
        public void Draw(Graphics graphics, bool DrawAttributeOrNot, int index)
        {
            spatialPart.Draw(graphics);
            if (DrawAttributeOrNot)
                attributePart.Draw(graphics, spatialPart.centroId, index);
        }
        public object GetAttribute(int index)
        {
            return attributePart.GetValue(index);
        }
    }

    public class GISAttribute
    {
        public ArrayList values = new ArrayList();
        public void AddValue(object o)
        {
            values.Add(o);
        }
        public object GetValue(int index)
        {
            return values[index];
        }
        public void Draw(Graphics graphics, GISVertex location, int index)
        {
            graphics.DrawString(values[index].ToString(), new Font("宋体", 20),
                new SolidBrush(Color.Green), new PointF((int)(location.x), (int)(location.y)));
        }
    }

    abstract class GISSpatial
    {
        public GISVertex centroId;
        public GISExtent extent;
        public abstract void Draw(Graphics graphics);
    }

    public class GISExtent
    {
        public GISVertex bottomLeft;
        public GISVertex upRight;

        public GISExtent(GISVertex bottomLeft, GISVertex upRight)
        {
            this.bottomLeft = bottomLeft;
            this.upRight = upRight;
        }
    }
}