using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

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
    class GISPoint : GISSpatial
    {
        public GISPoint(GISVertex oneVertex)
        {
            centroId = oneVertex;
            extent = new GISExtent(oneVertex, oneVertex);
        }
        public override void Draw(Graphics graphics, GISView view)
        {
            Point screenPoint = view.ToScreenPoint(centroId);
            graphics.FillEllipse(new SolidBrush(Color.Red),
                new Rectangle(screenPoint.X - 3, screenPoint.Y - 3, 6, 6));
        }
        public double Distance(GISVertex anotherVertex)
        {
            return centroId.Distance(anotherVertex);
        }
    }
    /*
     * 线实体
     */
    class GISLine : GISSpatial
    {
        List<GISVertex> AllVertexs;
        public override void Draw(Graphics graphics, GISView view)
        {
            throw new System.NotImplementedException();
        }
    }
    /**
     * 面实体
     */
    class GISPolygon : GISSpatial
    {
        List<GISVertex> AllVertexs;
        public override void Draw(Graphics graphics, GISView view)
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
        public void Draw(Graphics graphics, GISView view, bool DrawAttributeOrNot, int index)
        {
            spatialPart.Draw(graphics,view);
            if (DrawAttributeOrNot)
                attributePart.Draw(graphics, view, spatialPart.centroId, index);
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
        public void Draw(Graphics graphics, GISView view, GISVertex location, int index)
        {
            Point screenPoint = view.ToScreenPoint(location);
            graphics.DrawString(values[index].ToString(), 
                new Font("宋体", 20),
                new SolidBrush(Color.Green), 
                new PointF(screenPoint.X, screenPoint.Y));
        }
    }

    abstract class GISSpatial
    {
        public GISVertex centroId;
        public GISExtent extent;
        public abstract void Draw(Graphics graphics, GISView view);
    }
    /*
     * GISExtent
     */
    public class GISExtent
    {
        public GISVertex bottomLeft;
        public GISVertex upRight;
        double ZoomingFactor = 2;//缩放倍数
        double MovingFactor = 0.25;//移动倍数

        public GISExtent(GISVertex bottomLeft, GISVertex upRight)
        {
            this.bottomLeft = bottomLeft;
            this.upRight = upRight;
        }
        //该构造函数更好，保证角点的有效性，不必担心输入的坐标值谁大谁小。
        public GISExtent(double x1, double x2, double y1, double y2)
        {
            upRight = new GISVertex(Math.Max(x1, x2), Math.Max(y1, y2));
            bottomLeft = new GISVertex(Math.Min(x1, x2), Math.Min(y1, y2));
        }
        //修改地图显示范围
        public void ChangeExtent(GISMapActions action)
        {
            double newminx = bottomLeft.x, newminy = bottomLeft.y,
                newmaxx = upRight.x, newmaxy = upRight.y;
            switch(action)
            {
                case GISMapActions.zoomin:
                    newminx = ((GetMinX() + GetMaxX()) - GetWidth() / ZoomingFactor) / 2;
                    newminy = ((GetMinY() + GetMaxY()) - GetHeight() / ZoomingFactor) / 2;
                    newmaxx = ((GetMinX() + GetMaxX()) + GetWidth() / ZoomingFactor) / 2;
                    newmaxy = ((GetMinY() + GetMaxY()) + GetHeight() / ZoomingFactor) / 2;
                    break;
                case GISMapActions.zoomout:
                    newminx = ((GetMinX() + GetMaxX()) - GetWidth() * ZoomingFactor) / 2;
                    newminy = ((GetMinY() + GetMaxY()) - GetHeight() * ZoomingFactor) / 2;
                    newmaxx = ((GetMinX() + GetMaxX()) + GetWidth() * ZoomingFactor) / 2;
                    newmaxy = ((GetMinY() + GetMaxY()) + GetHeight() * ZoomingFactor) / 2;
                    break;
                case GISMapActions.moveup:
                    newminy = GetMinY() - GetHeight() * MovingFactor;
                    newmaxy = GetMaxY() - GetHeight() * MovingFactor;
                    break;
                case GISMapActions.movedown:
                    newminy = GetMinY() + GetHeight() * MovingFactor;
                    newmaxy = GetMaxY() + GetHeight() * MovingFactor;
                    break;
                case GISMapActions.moveleft:
                    newminx = GetMinX() + GetWidth() * MovingFactor;
                    newmaxx = GetMaxX() + GetWidth() * MovingFactor;
                    break;
                case GISMapActions.moveright:
                    newminx = GetMinX() - GetWidth() * MovingFactor;
                    newmaxx = GetMaxX() - GetWidth() * MovingFactor;
                    break;
            }
            upRight.x = newmaxx;
            upRight.y = newmaxy;
            bottomLeft.x = newminx;
            bottomLeft.y = newminy;
        }

        public double GetMinX()
        {
            return bottomLeft.x;
        }
        public double GetMaxX()
        {
            return upRight.x;
        }
        public double GetMinY()
        {
            return bottomLeft.y;
        }
        public double GetMaxY()
        {
            return upRight.y;
        }
        public double GetWidth()
        {
            return upRight.x - bottomLeft.x;
        }
        public double GetHeight()
        {
            return upRight.y - bottomLeft.y;
        }
    }
    /*
     * GISView
     */
    public class GISView
    {
        GISExtent CurrentMapExent;//记录当前绘图窗口中的地图范围
        Rectangle MapWindowSize;//记录绘图窗口的大小，窗口的左上角必须事坐标原点。
        double MapMinX, MapMinY;//当前屏幕显示地图范围的最小横纵坐标
        int WinW, WinH;//绘图窗口的宽度、高度
        double MapW, MapH;//地图横坐标长度，纵坐标长度
        double ScaleX, ScaleY;//横、纵坐标比例尺，即一个绘图窗口中的像素分别表示多少个横、纵地图坐标单位

        public GISView(GISExtent currentMapExent, Rectangle mapWindowSize)
        {
            Update(currentMapExent, mapWindowSize);
        }
        public void Update(GISExtent _extent, Rectangle _rectangle)
        {
            CurrentMapExent = _extent;
            MapWindowSize = _rectangle;
            MapMinX = CurrentMapExent.GetMinX();
            MapMinY = CurrentMapExent.GetMinY();
            WinW = MapWindowSize.Width;
            WinH = MapWindowSize.Height;
            MapW = CurrentMapExent.GetWidth();
            MapH = CurrentMapExent.GetHeight();
            ScaleX = MapW / WinW;
            ScaleY = MapH / WinH;
        }

        //以下两个函数用于地图坐标和屏幕坐标之间的转换
        public Point ToScreenPoint(GISVertex oneVertex)
        {
            double ScreenX = (oneVertex.x - MapMinX) / ScaleX;
            double ScreenY = WinH - (oneVertex.y - MapMinY) / ScaleY;
            return new Point((int)ScreenX, (int)ScreenY);
        }
        public GISVertex ToMapVertex(Point point)
        {
            double MapX = ScaleX * point.X + MapMinX;
            double MapY = ScaleY * (WinH - point.Y) + MapMinY;
            return new GISVertex(MapX, MapY);
        }

        public void ChangeView(GISMapActions action)
        {
            CurrentMapExent.ChangeExtent(action);
            Update(CurrentMapExent, MapWindowSize);
        }

    }
    public enum GISMapActions
    {
        zoomin, zoomout,
        moveup, movedown, moveleft, moveright
    };
    class GISShapefile
    {
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct ShapefileHeader
        {
            public int Unused1, Unused2, Unused3, Unused4;
            public int Unused5, Unused6, Unused7, Unused8;
            public int ShapeType;
            public double Xmin;
            public double Ymin;
            public double Xmax;
            public double Ymax;
            public double Unused9, Unused10, Unused11, Unused12;
        };
        ShapefileHeader ReadFileHeader(BinaryReader br)
        {
            byte[] buff = br.ReadBytes(Marshal.SizeOf(typeof(ShapefileHeader)));
            GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);
            ShapefileHeader header = (ShapefileHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(ShapefileHeader));
            handle.Free();
            return header;
        }
        public GISLayer ReadShapefile(string shapefilename)
        {
            FileStream fsr = new FileStream(shapefilename, FileMode.Open);//打开shp磁盘文件
            BinaryReader br = new BinaryReader(fsr);//
            ShapefileHeader sfh = ReadFileHeader(br);
            SHAPETYPE ShapeType = (SHAPETYPE)Enum.Parse(typeof(SHAPETYPE), sfh.ShapeType.ToString());
            GISExtent extent = new GISExtent(sfh.Xmax, sfh.Xmin, sfh.Ymax, sfh.Ymin);
            GISLayer layer = new GISLayer(shapefilename, extent,ShapeType);
            while (br.PeekChar() != -1)
            {
                RecordHeader rh = ReadRecordHeader(br);
                int RecordLength = FromBigToLittle(rh.RecordLength) * 2 - 4;
                byte[] RecordContent = br.ReadBytes(RecordLength);
                if(ShapeType == SHAPETYPE.point)
                {
                    GISPoint onepoint = ReadPoint(RecordContent);
                    GISFeature onefeature = new GISFeature(onepoint, new GISAttribute());
                    layer.AddFeature(onefeature);
                }
            }
            br.Close();
            fsr.Close();
            return layer;
        }
        //读取shp中一个点实体记录
        GISPoint ReadPoint(byte[] RecordContent)
        {
            double x = BitConverter.ToDouble(RecordContent, 0);
            double y = BitConverter.ToDouble(RecordContent, 8);
            return new GISPoint(new GISVertex(x, y));
        }
        [StructLayout(LayoutKind.Sequential, Pack =4)]
        struct RecordHeader
        {
            public int RecordNumber;
            public int RecordLength;
            public int ShapeType;
        }
        //读取记录头
        RecordHeader ReadRecordHeader(BinaryReader br)
        {
            byte[] buff = br.ReadBytes(Marshal.SizeOf(typeof(RecordHeader)));
            GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);
            RecordHeader header = (RecordHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(RecordHeader));
            handle.Free();
            return header;
        }
        //通用Big Integer 转 Little Integer
        int FromBigToLittle(int bigValue)
        {
            byte[] bigbytes = new byte[4];
            GCHandle handle = GCHandle.Alloc(bigbytes, GCHandleType.Pinned);
            Marshal.StructureToPtr(bigValue, handle.AddrOfPinnedObject(), false);
            handle.Free();
            //改变字节顺序
            byte b2 = bigbytes[2];
            byte b3 = bigbytes[3];
            bigbytes[3] = bigbytes[0];
            bigbytes[2] = bigbytes[1];
            bigbytes[1] = b2;
            bigbytes[0] = b3;
            return BitConverter.ToInt32(bigbytes, 0);
        }
    }
    //定义一个枚举存储shapefile常量
    enum SHAPETYPE
    {
        point = 1,
        line = 3,
        polygon = 5
    };
    //图层,相同类型的空间实体的集合
    class GISLayer
    {
        public string Name;//图层名称
        public GISExtent Extent;//地图范围
        public bool DrawAttributeOrNot;//是否需要标注属性信息
        public int LabelIndex;//需要表主的属性序列号
        public SHAPETYPE ShapeType;//空间对象类型
        List<GISFeature> Features = new List<GISFeature>();//包含所有的空间对象实体

        public GISLayer(string name, GISExtent extent, SHAPETYPE shapeType)
        {
            Name = name;
            Extent = extent;
            ShapeType = shapeType;
        }
        public void Draw(Graphics graphics, GISView view)
        {
            for(int i = 0; i < Features.Count; i++)
            {
                Features[i].Draw(graphics, view, DrawAttributeOrNot, LabelIndex);
            }
        }
        public void AddFeature(GISFeature feature)
        {
            Features.Add(feature);
        }
        public int FeatureCount()
        {
            return Features.Count;
        }
    }
}