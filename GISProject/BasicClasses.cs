using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

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
        public GISVertex(BinaryReader br)
        {
            x = br.ReadDouble();
            y = br.ReadDouble();
        }
        public double Distance(GISVertex anotherVertex)
        {
            return System.Math.Sqrt((x - anotherVertex.x) * (x - anotherVertex.x)
                + (y - anotherVertex.y) * (y - anotherVertex.y));
        }
        public void CopyFrom(GISVertex v)
        {
            x = v.x;
            y = v.y;           
        }
        //输出到二进制文件
        public void WriteVertex(BinaryWriter bw)
        {
            bw.Write(x);
            bw.Write(y);
        }

        public bool isSame(GISVertex vertex)
        {
            return x == vertex.x && y == vertex.y;
        }
    }
    /*
     * 实体点
     */
    public class GISPoint : GISSpatial
    {
        public GISPoint(GISVertex oneVertex)
        {
            centroId = oneVertex;
            extent = new GISExtent(oneVertex, oneVertex);
        }
        public override void Draw(Graphics graphics, GISView view, bool Selected)
        {
            Point screenPoint = view.ToScreenPoint(centroId);
            graphics.FillEllipse(new SolidBrush(Selected? GISConst.SelectedPointColor:GISConst.PointColor),
                new Rectangle(screenPoint.X - GISConst.PointSize, screenPoint.Y - GISConst.PointSize, GISConst.PointSize*2, GISConst.PointSize*2));
        }
        public double Distance(GISVertex anotherVertex)
        {
            return centroId.Distance(anotherVertex);
        }
    }
    /*
     * 线实体
     */
    public class GISLine : GISSpatial
    {
        public List<GISVertex> Vertexs;
        public double Length;

        public GISLine(List<GISVertex> vertexs)
        {
            Vertexs = vertexs;
            centroId = GISTools.CalculateCentroid(vertexs);
            extent = GISTools.CalculateExtent(vertexs);
            Length = GISTools.CalculateLength(vertexs);
        }
        //点到线距离,逐个计算点到线段的距离，直到找到最短的那个距离
        public double Distance(GISVertex vertex)
        {
            double distance = double.MaxValue;
            for (int i = 0; i < Vertexs.Count; i++)
            {
                distance = Math.Min(GISTools.PointToSegment(Vertexs[i], Vertexs[i + 1], vertex), distance);
            }
            return distance;
        }

        public override void Draw(Graphics graphics, GISView view, bool selected)
        {
            Point[] points = GISTools.GetScreenPoints(Vertexs, view);
            graphics.DrawLines(new Pen(selected? GISConst.SelectedLineColor:GISConst.LineColor, GISConst.LineWidth), points);
        }
        //线的起始点
        public GISVertex FromNode()
        {
            return Vertexs[0];
        }
        //线的终止点
        public GISVertex ToNode()
        {
            return Vertexs[Vertexs.Count - 1];
        }
    }
    /**
     * 面实体
     */
    public class GISPolygon : GISSpatial
    {
        public List<GISVertex> Vertexs;
        public double Area;

        public GISPolygon(List<GISVertex> vertexs)
        {
            Vertexs = vertexs;
            centroId = GISTools.CalculateCentroid(vertexs);
            extent = GISTools.CalculateExtent(vertexs);
            Area = GISTools.CalculateArea(vertexs);
        }

        public override void Draw(Graphics graphics, GISView view, bool selected)
        {
            Point[] points = GISTools.GetScreenPoints(Vertexs, view);
            graphics.FillPolygon(new SolidBrush(selected? GISConst.SelectedPolygonFillColor: GISConst.PolygonFillColor), points);
            graphics.DrawPolygon(new Pen(GISConst.PolygonBoundaryColor, GISConst.PolygonBoundaryWidth), points);
        }
        //射线法判断点面位置
        public bool Include(GISVertex vertex)
        {
            int count = 0;
            for (int i = 0; i < Vertexs.Count; i++)
            {
                //点与面的节点重合
                if (Vertexs[i].isSame(vertex)) return false;
                //由序列为i及next的两个节点构成一条线段，一般情况下next为i+1，
                //而针对最后一条线段，i为Vertexs.Count-1,next为0；
                int next = (i + 1) % Vertexs.Count;
                //确定线段的坐标极值
                double minX = Math.Min(Vertexs[i].x, Vertexs[next].x);
                double minY = Math.Min(Vertexs[i].y, Vertexs[next].y);
                double maxX = Math.Max(Vertexs[i].x, Vertexs[next].x);
                double maxY = Math.Max(Vertexs[i].y, Vertexs[next].y);
                //如果线段平行于射线
                if(minY == maxY)
                {
                    //满足射线刚好与边重合且点在边上，返回false
                    if (minY == vertex.y && vertex.x >= minX && vertex.x <= maxX) return false;
                    //在边的延长线上或射线与线段平行无交点
                    else continue;
                }
                //点在线段坐标极值之外，不可能有交点
                if (vertex.x > maxX || vertex.y > maxY || vertex.y < minY) continue;
                //计算交点横坐标，纵坐标无需计算，就是vertex.y
                double X0 = Vertexs[i].x + (vertex.y - Vertexs[i].y) * (Vertexs[next].x - Vertexs[i].x) / (Vertexs[next].y - Vertexs[i].y);
                //交点在射线反方向，按无交点计算
                if (X0 < vertex.x) continue;
                //交点即为vertex,且在线段上，按不包括处理
                if (X0 == vertex.x) continue;
                //射线穿过线段下断电，不计数
                if (vertex.y == minY) continue;
                //其他情况，交点数加一
                count++;
            }
            //根据交点数量确定面是否包括点,奇数表示在面内，偶数表示在面外
            return count % 2 != 0;
        }
    }

    public class GISFeature
    {
        public GISSpatial spatialPart;
        public GISAttribute attributePart;
        public bool Selected = false;
        public int ID;//在同一图层中把多个GISFeature区别开来，在GISLayer的AddFeature函数中进行赋值。

        public GISFeature(GISSpatial spatialPart, GISAttribute attributePart)
        {
            this.spatialPart = spatialPart;
            this.attributePart = attributePart;
        }
        public void Draw(Graphics graphics, GISView view, bool DrawAttributeOrNot, int index)
        {
            spatialPart.Draw(graphics, view, Selected);
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
        public int ValueCount()
        {
            return values.Count;
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

    public abstract class GISSpatial
    {
        public GISVertex centroId;
        public GISExtent extent;
        public abstract void Draw(Graphics graphics, GISView view, bool selected);
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
        public void CopyFrom(GISExtent extent)
        {
            upRight.CopyFrom(extent.upRight);
            bottomLeft.CopyFrom(extent.bottomLeft);
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
        //判断MinSelectExtent与当前空间对象是否相交,排除所有不相交的可能，剩下就是必然相交
        public bool InsertectOrNot(GISExtent extent)
        {
            return !(GetMaxX() < extent.GetMinX() || GetMinX() > extent.GetMaxX()
                || GetMaxY() < extent.GetMinY() || GetMinY() > extent.GetMaxY());
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

        public bool Include(GISExtent extent)
        {
            return (GetMaxX() >= extent.GetMaxX() && GetMaxY() >= extent.GetMaxY()
                && GetMinX() <= extent.GetMinX() && GetMinY() <= extent.GetMinY());
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
        //将当前窗口的范围告诉view,让它及时更新
        public void UpdateRectangle(Rectangle rectangle)
        {
            MapWindowSize = rectangle;
            Update(CurrentMapExent, MapWindowSize);
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
            //令两者相等，确保显示内容完整且不变形。
            ScaleX = Math.Max(ScaleX, ScaleY);
            ScaleY = ScaleX;
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
        public void UpdateExtent(GISExtent extent)
        {
            CurrentMapExent.CopyFrom(extent);
            Update(CurrentMapExent, MapWindowSize);
        }
        //计算屏幕距离
        public double ToScreenDistance(GISVertex v1, GISVertex v2)
        {
            Point p1 = ToScreenPoint(v1);
            Point p2 = ToScreenPoint(v2);
            return Math.Sqrt((double)((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y)));
        }
        public double ToScreenDistance(double distance)
        {
            return ToScreenDistance(new GISVertex(0, 0), new GISVertex(0, distance));
        }

        internal GISExtent RectToExtent(int x1, int x2, int y1, int y2)
        {
            GISVertex v1 = ToMapVertex(new Point(x1, y1));
            GISVertex v2 = ToMapVertex(new Point(x2, y2));
            return new GISExtent(v1.x, v2.x, v1.y, v2.y);
        }

        internal GISExtent GetRealExtent()
        {
            return new GISExtent(MapMinX, MapMinX + MapW, MapMinY, MapMinY + MapH);
        }
    }
    public enum GISMapActions
    {
        zoomin, zoomout,
        moveup, movedown, moveleft, moveright
    };
    public class GISShapefile
    {
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct ShapefileHeader
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
        static ShapefileHeader ReadFileHeader(BinaryReader br)
        {
            return (ShapefileHeader)GISTools.FromBytes(br, typeof(ShapefileHeader));
        }
        public static GISLayer ReadShapefile(string shapefilename)
        {
            FileStream fsr = new FileStream(shapefilename, FileMode.Open);//打开shp磁盘文件
            BinaryReader br = new BinaryReader(fsr);//
            ShapefileHeader sfh = ReadFileHeader(br);
            SHAPETYPE ShapeType = (SHAPETYPE)Enum.Parse(typeof(SHAPETYPE), sfh.ShapeType.ToString());
            GISExtent extent = new GISExtent(sfh.Xmax, sfh.Xmin, sfh.Ymax, sfh.Ymin);
            string dbffilenam = shapefilename.Replace(".shp", ".dbf");
            DataTable table = ReadDBF(dbffilenam);
            GISLayer layer = new GISLayer(shapefilename, extent,ShapeType, ReadFields(table));
            int rowindex = 0;
            while (br.PeekChar() != -1)
            {
                RecordHeader rh = ReadRecordHeader(br);
                int RecordLength = FromBigToLittle(rh.RecordLength) * 2 - 4;
                byte[] RecordContent = br.ReadBytes(RecordLength);
                if(ShapeType == SHAPETYPE.point)
                {
                    GISPoint onepoint = ReadPoint(RecordContent);
                    GISFeature onefeature = new GISFeature(onepoint, ReadAttribute(table, rowindex));
                    layer.AddFeature(onefeature);
                }
                if(ShapeType == SHAPETYPE.line)
                {
                    List<GISLine> lines = ReadLines(RecordContent);
                    for(int i = 0; i < lines.Count; i++)
                    {
                        GISFeature onefeature = new GISFeature(lines[i], ReadAttribute(table,rowindex));
                        layer.AddFeature(onefeature);
                    }
                }
                if (ShapeType == SHAPETYPE.polygon)
                {
                    List<GISPolygon> polygons = ReadPolygons(RecordContent);
                    for(int i = 0; i < polygons.Count; i++)
                    {
                        GISFeature onefeature = new GISFeature(polygons[i], ReadAttribute(table,rowindex));
                        layer.AddFeature(onefeature);
                    }
                }
                rowindex++;
            }
            br.Close();
            fsr.Close();
            return layer;
        }
        //读取shp中一个点实体记录
        public static GISPoint ReadPoint(byte[] RecordContent)
        {
            double x = BitConverter.ToDouble(RecordContent, 0);
            double y = BitConverter.ToDouble(RecordContent, 8);
            return new GISPoint(new GISVertex(x, y));
        }
        [StructLayout(LayoutKind.Sequential, Pack =4)]
        public struct RecordHeader
        {
            public int RecordNumber;
            public int RecordLength;
            public int ShapeType;
        }
        //读取记录头
        public static RecordHeader ReadRecordHeader(BinaryReader br)
        {
            return (RecordHeader)GISTools.FromBytes(br, typeof(RecordHeader));
        }
        //通用Big Integer 转 Little Integer
        public static int FromBigToLittle(int bigValue)
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
        //读取线实体
        public static List<GISLine> ReadLines(byte[] RecordContent)
        {
            int N = BitConverter.ToInt32(RecordContent, 32);
            int M = BitConverter.ToInt32(RecordContent, 36);
            int[] parts = new int[N + 1];

            for (int i = 0; i < N; i++)
            {
                parts[i] = BitConverter.ToInt32(RecordContent, 40 + i * 4);
            }
            parts[N] = M;
            List<GISLine> lines = new List<GISLine>();
            for (int i = 0; i < N; i++)
            {
                List<GISVertex> vertexes = new List<GISVertex>();
                for (int j = parts[i]; j < parts[i+1]; j++)
                {
                    double x = BitConverter.ToDouble(RecordContent, 40 + N * 4 + j * 16);
                    double y = BitConverter.ToDouble(RecordContent, 40 + N * 4 + j * 16 + 8);
                    vertexes.Add(new GISVertex(x,y));
                }
                lines.Add(new GISLine(vertexes));
            }
            return lines;
        }
        //读取面实体
        public static List<GISPolygon> ReadPolygons(byte[] RecordContent)
        {
            int N = BitConverter.ToInt32(RecordContent, 32);
            int M = BitConverter.ToInt32(RecordContent, 36);
            int[] parts = new int[N + 1];
            for(int i = 0; i < N; i++)
            {
                parts[i] = BitConverter.ToInt32(RecordContent, 40 + i * 4);
            }
            parts[N] = M;
            List<GISPolygon> polygons = new List<GISPolygon>();
            for(int i = 0; i < N; i++)
            {
                List<GISVertex> vertexes = new List<GISVertex>();
                for(int j = parts[i]; j < parts[i + 1]; j++)
                {
                    double x = BitConverter.ToDouble(RecordContent, 40 + N * 4 + j * 16);
                    double y = BitConverter.ToDouble(RecordContent, 40 + N * 4 + j * 16 + 8);
                    vertexes.Add(new GISVertex(x, y));
                }
                polygons.Add(new GISPolygon(vertexes));
            }
            return polygons;
        }
        //读取dbf文件
        public static DataTable ReadDBF(string dbfilename)
        {
            System.IO.FileInfo f = new FileInfo(dbfilename);
            DataSet ds = null;
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + f.DirectoryName + ";Extended Properties=DBASE III";
            using (OleDbConnection con = new OleDbConnection(constr))
            {
                var sql = "select * from " + f.Name;
                OleDbCommand cmd = new OleDbCommand(sql, con);
                con.Open();
                ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
            }
            return ds.Tables[0];
        }
        //读取GISField字段结构
        public static List<GISField> ReadFields(DataTable table)
        {
            List<GISField> fields = new List<GISField>();
            foreach(DataColumn column in table.Columns)
            {
                fields.Add(new GISField(column.DataType, column.ColumnName));
            }
            return fields;
        }
        //读取属性
        public static GISAttribute ReadAttribute(DataTable table, int RowIndex)
        {
            GISAttribute attribute = new GISAttribute();
            DataRow row = table.Rows[RowIndex];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                attribute.AddValue(row[i]);
            }
            return attribute;
        }
    }
    //定义一个枚举存储shapefile常量
    public enum SHAPETYPE
    {
        point = 1,
        line = 3,
        polygon = 5
    };
    //图层,相同类型的空间实体的集合
    public class GISLayer
    {
        public string Name;//图层名称
        public GISExtent Extent;//地图范围
        public bool DrawAttributeOrNot = false;//是否需要标注属性信息
        public int LabelIndex;//需要表主的属性序列号
        public SHAPETYPE ShapeType;//空间对象类型
        List<GISFeature> Features = new List<GISFeature>();//包含所有的空间对象实体
        public List<GISField> Fields;
        public List<GISFeature> Selection = new List<GISFeature>();

        public GISLayer(string name, GISExtent extent, SHAPETYPE shapeType, List<GISField> fields) : this(name, extent, shapeType)
        {
            Fields = fields;
        }
        public GISLayer(string name, GISExtent extent, SHAPETYPE shapeType)
        {
            Name = name;
            Extent = extent;
            ShapeType = shapeType;
            Fields = new List<GISField>();
        }
        //对象选择操作
        public SelectResult Select(GISVertex vertex, GISView view)
        {
            GISSelect gs = new GISSelect();
            SelectResult sr = gs.Select(vertex, Features, ShapeType, view);
            if(sr == SelectResult.OK)
            {
                if(ShapeType == SHAPETYPE.polygon)
                {
                    for (int i = 0; i < gs.SelectedFeatures.Count; i++)
                    {
                        if(gs.SelectedFeatures[i].Selected == false)
                        {
                            gs.SelectedFeatures[i].Selected = true;
                            Selection.Add(gs.SelectedFeatures[i]);
                        }
                    }
                }
                else
                {
                    if(gs.SelectedFeature.Selected == false)
                    {
                        gs.SelectedFeature.Selected = true;
                        Selection.Add(gs.SelectedFeature);
                    }
                }
            }
            return sr;
        }
        public SelectResult Select(GISExtent extent)
        {
            GISSelect gs = new GISSelect();
            SelectResult sr = gs.Select(extent, Features);
            if(sr == SelectResult.OK)
            {
                for (int i = 0; i < gs.SelectedFeatures.Count; i++)
                {
                    if(gs.SelectedFeatures[i].Selected == false)
                    {
                        gs.SelectedFeatures[i].Selected = true;
                        Selection.Add(gs.SelectedFeatures[i]);
                    }
                }
            }
            return sr;
        }

        public void ClearSelection()
        {
            for (int i = 0; i < Selection.Count; i++)
            {
                Selection[i].Selected = false;
            }
            Selection.Clear();
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
            if (Features.Count == 0) feature.ID = 0;
            else feature.ID = Features[Features.Count - 1].ID + 1;
            Features.Add(feature);
        }
        public int FeatureCount()
        {
            return Features.Count;
        }
        public GISFeature GetFeature(int i)
        {
            return Features[i];
        }

        internal List<GISFeature> GetAllFeatures()
        {
            return Features;
        }

        internal void AddSelectedFeatByID(int id)
        {
            GISFeature feature = GetFeatureByID(id);
            feature.Selected = true;
            Selection.Add(feature);
        }
        public GISFeature GetFeatureByID(int id)
        {
            foreach (GISFeature feature in Features)
            {
                if (feature.ID == id) return feature;
            }
            return null;
        }
    }
    public class GISTools
    {
        //计算点到线的最短
        public static double PointToSegment(GISVertex a, GISVertex b, GISVertex c)
        {
            double dot1 = Dot3Product(a, b, c);
            if (dot1 > 0) return b.Distance(c);
            double dot2 = Dot3Product(b, a, c);
            if (dot2 > 0) return a.Distance(c);
            double dist = Cross3Product(a, b, c) / a.Distance(b);
            return Math.Abs(dist);
        }

        private static double Cross3Product(GISVertex a, GISVertex b, GISVertex c)
        {
            GISVertex AB = new GISVertex(b.x - a.x, b.y - a.y);
            GISVertex AC = new GISVertex(c.x - a.x, c.y - a.y);
            return VectorProduct(AB, AC);
        }

        private static double Dot3Product(GISVertex a, GISVertex b, GISVertex c)
        {
            GISVertex AB = new GISVertex(b.x - a.x, b.y - a.y);
            GISVertex BC = new GISVertex(c.x - b.x, c.y - b.y);
            return AB.x * BC.x + AB.y * BC.y;
        }

        //计算中心点坐标
        public static GISVertex CalculateCentroid(List<GISVertex> vertexes)
        {
            if (vertexes.Count == 0) return null;
            double x = 0, y = 0;
            for (int i = 0; i < vertexes.Count; i++)
            {
                x += vertexes[i].x;
                y += vertexes[i].y;
            }
            return new GISVertex(x / vertexes.Count, y / vertexes.Count);
        }
        //计算extent
        public static GISExtent CalculateExtent(List<GISVertex> vertexes)
        {
            if (vertexes.Count == 0) return null;
            double minx = Double.MaxValue;
            double miny = Double.MaxValue;
            double maxx = Double.MaxValue; 
            double maxy = Double.MaxValue;
            for (int i = 0; i < vertexes.Count; i++)
            {
                if (vertexes[i].x < minx) minx = vertexes[i].x;
                if (vertexes[i].x > maxx) maxx = vertexes[i].x;
                if (vertexes[i].y < miny) miny = vertexes[i].y;
                if (vertexes[i].y > maxy) maxy = vertexes[i].y;
            }
            return new GISExtent(minx, maxx, miny, maxy);
        }
        //计算长度
        public static double CalculateLength(List<GISVertex> vertexes)
        {
            double length = 0;
            for (int i = 0; i < vertexes.Count; i++)
            {
                length += vertexes[i].Distance(vertexes[i + 1]);
            }
            return length;
        }
        //计算面积
        public static double CalculateArea(List<GISVertex> vertexes)
        {
            double area = 0;
            for (int i = 0; i < vertexes.Count-1; i++)
            {
                area += VectorProduct(vertexes[i], vertexes[i + 1]);
            }
            area += VectorProduct(vertexes[vertexes.Count - 1], vertexes[0]);
            return area / 2;
        }
        //计算矢量积
        private static double VectorProduct(GISVertex v1, GISVertex v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }

        internal static Point[] GetScreenPoints(List<GISVertex> vertexs, GISView view)
        {
            Point[] points = new Point[vertexs.Count];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = view.ToScreenPoint(vertexs[i]);
            }
            return points;
        }
        //将结构体实例转存为字节数组
        public static byte[] ToBytes(object c)
        {
            byte[] bytes = new byte[Marshal.SizeOf(c.GetType())];//定义一个与结构体字节数等长的字节数组
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            Marshal.StructureToPtr(c, handle.AddrOfPinnedObject(), false);//把结构体实例值放入数组
            handle.Free();
            return bytes;
        }
        //从二进制文件读取到某个结构体的实例中
        public static Object FromBytes(BinaryReader br, Type type)
        {
            byte[] buff = br.ReadBytes(Marshal.SizeOf(type));
            GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);
            Object result = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), type);
            handle.Free();
            return result;
        }
        //把字符串写入一个二进制
        public static void WriteString(string s, BinaryWriter bw)
        {
            bw.Write(StringLength(s));//写入字节长度
            byte[] sbytes = Encoding.Default.GetBytes(s);//将字符串转换为字符数组
            bw.Write(sbytes);
        }
        //从文件中读取一个字符串
        public static string ReadString(BinaryReader br)
        {
            int length = br.ReadInt32();
            byte[] sbytes = br.ReadBytes(length);
            return Encoding.Default.GetString(sbytes);
        }
        //WriteString中写入字符长度，一般下字符数等于字节数。若字符串中有中文，会占两个字节，却被当作一个字符，就有问题。
        public static int StringLength(string s)
        {
            int ChineseCount = 0;
            //将字符串转换为ASCII来编码的字节数组
            byte[] bs = new ASCIIEncoding().GetBytes(s);
            foreach (byte b in bs)
            {
                if (b == 0X3F) ChineseCount++;
            }
            return ChineseCount + bs.Length;
        }
        //将给定数据类型转换为整数
        public static int TypeToInt(Type type)
        {
            ALLTYPES onetype = (ALLTYPES)Enum.Parse(typeof(ALLTYPES), type.ToString().Replace(".", "_"));
            return (int)onetype;
        }
        //把读取的整数转成特定的数据类型
        public static Type IntToType(int index)
        {
            string typestring = Enum.GetName(typeof(ALLTYPES), index);
            typestring = typestring.Replace("_", ".");
            return Type.GetType(typestring);
        }
    }
    //属性数据字段类
    public class GISField
    {
        public Type dataType;
        public string name;

        public GISField(Type dataType, string name)
        {
            this.dataType = dataType;
            this.name = name;
        }
    }

    //个人GIS文件格式
    public class GISMyFile
    {
        [StructLayout(LayoutKind.Sequential,Pack =4)]
        struct MyFileHeader//文件头
        {
            public double MinX, MinY, MaxX, MaxY;//最小、最大横纵坐标
            public int FeatureCount, ShapeType, FieldCount;//对象数量、对象类型、属性字段数量
        };
        //将MyFileHeader的实例写入文件
        static void WriteFileHeader(GISLayer layer, BinaryWriter bw)
        {
            MyFileHeader mfh = new MyFileHeader();
            mfh.MinX = layer.Extent.GetMinX();
            mfh.MinY = layer.Extent.GetMinY();
            mfh.MaxX = layer.Extent.GetMaxX();
            mfh.MaxY = layer.Extent.GetMaxY();
            mfh.FeatureCount = layer.FeatureCount();
            mfh.ShapeType = (int)(layer.ShapeType);
            mfh.FieldCount = layer.Fields.Count;
            bw.Write(GISTools.ToBytes(mfh));
        }
        //写文件函数
        public static void WriteFile(GISLayer layer, string filename)
        {
            FileStream fst = new FileStream(filename, FileMode.Create);//根据文件名新建一个文件用于写入
            BinaryWriter bw = new BinaryWriter(fst);//获得其写入工具bw
            WriteFileHeader(layer, bw);//完成对文件头的写入
            GISTools.WriteString(layer.Name, bw);
            WriteFields(layer.Fields, bw);
            WriteFeatures(layer, bw);
            bw.Close();
            fst.Close();
        }
        //读取文件
        public static GISLayer ReadFile(string filename)
        {
            FileStream fsr = new FileStream(filename, FileMode.Open);
            BinaryReader br = new BinaryReader(fsr);
            MyFileHeader mfh = (MyFileHeader)(GISTools.FromBytes(br, typeof(MyFileHeader)));
            SHAPETYPE ShapeType = (SHAPETYPE)Enum.Parse(typeof(SHAPETYPE), mfh.ShapeType.ToString());
            GISExtent Extent = new GISExtent(mfh.MinX, mfh.MaxX, mfh.MinY, mfh.MaxY);
            string layername = GISTools.ReadString(br);
            List<GISField> Fields = ReadFilds(br, mfh.FieldCount);
            GISLayer layer = new GISLayer(layername, Extent, ShapeType, Fields);
            ReadFeatures(layer, br, mfh.FeatureCount);
            br.Close();
            fsr.Close();
            return layer;
        }
        //写入字段信息
        static void WriteFields(List<GISField> fields, BinaryWriter bw)
        {
            for (int fieldindex = 0; fieldindex < fields.Count; fieldindex++)
            {
                GISField field = fields[fieldindex];
                bw.Write(GISTools.TypeToInt(field.dataType));
                GISTools.WriteString(field.name, bw);
            }
        }
        //写入多个GISVertex
        static void WriteMultiVertexes(List<GISVertex> vs, BinaryWriter bw)
        {
            bw.Write(vs.Count);
            for (int i = 0; i < vs.Count; i++)
            {
                vs[i].WriteVertex(bw);
            }
        }
        //读取多个GISVertex
        static List<GISVertex> ReadMultiVertexes(BinaryReader br)
        {
            List<GISVertex> vs = new List<GISVertex>();
            int vcount = br.ReadInt32();
            for (int vc = 0; vc < vcount; vc++)
                vs.Add(new GISVertex(br));
            return vs;
        }
        //写入属性信息
        static void WriteAttributes(GISAttribute attribute, BinaryWriter bw)
        {
            for (int i = 0; i < attribute.ValueCount(); i++)
            {
                Type type = attribute.GetValue(i).GetType();
                if (type.ToString() == "System.Boolean")
                    bw.Write((bool)attribute.GetValue(i));
                else if (type.ToString() == "System.Byte")
                    bw.Write((byte)attribute.GetValue(i));
                else if (type.ToString() == "System.Char")
                    bw.Write((char)attribute.GetValue(i));
                else if (type.ToString() == "System.Decimal")
                    bw.Write((decimal)attribute.GetValue(i));
                else if (type.ToString() == "System.Double")
                    bw.Write((double)attribute.GetValue(i));
                else if (type.ToString() == "System.Single")
                    bw.Write((float)attribute.GetValue(i));
                else if (type.ToString() == "System.Int32")
                    bw.Write((int)attribute.GetValue(i));
                else if (type.ToString() == "System.Int64")
                    bw.Write((long)attribute.GetValue(i));
                else if (type.ToString() == "System.UInt16")
                    bw.Write((ushort)attribute.GetValue(i));
                else if (type.ToString() == "System.UInt32")
                    bw.Write((uint)attribute.GetValue(i));
                else if (type.ToString() == "System.UInt64")
                    bw.Write((ulong)attribute.GetValue(i));
                else if (type.ToString() == "System.SByte")
                    bw.Write((sbyte)attribute.GetValue(i));
                else if (type.ToString() == "System.Int16")
                    bw.Write((short)attribute.GetValue(i));
                else if (type.ToString() == "System.String")
                    GISTools.WriteString((string)attribute.GetValue(i), bw);
            }
        }
        //读取属性数据
        static GISAttribute ReadAttributes(List<GISField> fs, BinaryReader br)
        {
            GISAttribute attribute = new GISAttribute();
            for (int i = 0; i < fs.Count; i++)
            {
                Type type = fs[i].dataType;
                if (type.ToString() == "System.Boolean")
                    attribute.AddValue(br.ReadBoolean());
                else if (type.ToString() == "System.Byte")
                    attribute.AddValue(br.ReadByte());
                else if (type.ToString() == "System.Char")
                    attribute.AddValue(br.ReadChar());
                else if (type.ToString() == "System.Decimal")
                    attribute.AddValue(br.ReadDecimal());
                else if (type.ToString() == "System.Double")
                    attribute.AddValue(br.ReadDouble());
                else if (type.ToString() == "System.Single")
                    attribute.AddValue(br.ReadSingle());
                else if (type.ToString() == "System.Int32")
                    attribute.AddValue(br.ReadInt32());
                else if (type.ToString() == "System.Int64")
                    attribute.AddValue(br.ReadInt64());
                else if (type.ToString() == "System.UInt16")
                    attribute.AddValue(br.ReadUInt16());
                else if (type.ToString() == "System.UInt32")
                    attribute.AddValue(br.ReadUInt32());
                else if (type.ToString() == "System.UInt64")
                    attribute.AddValue(br.ReadUInt64());
                else if (type.ToString() == "System.SByte")
                    attribute.AddValue(br.ReadSByte());
                else if (type.ToString() == "System.Int16")
                    attribute.AddValue(br.ReadInt16());
                else if (type.ToString() == "System.String")
                    attribute.AddValue(GISTools.ReadString(br));
            }
            return attribute;
        }
        //写入图层所有GISFeature
        static void WriteFeatures(GISLayer layer, BinaryWriter bw)
        {
            for (int featureindex = 0; featureindex < layer.FeatureCount(); featureindex++)
            {
                GISFeature feature = layer.GetFeature(featureindex);
                if(layer.ShapeType == SHAPETYPE.point)
                {
                    ((GISPoint)feature.spatialPart).centroId.WriteVertex(bw);
                }
                else if(layer.ShapeType == SHAPETYPE.line)
                {
                    GISLine line = (GISLine)(feature.spatialPart);
                    WriteMultiVertexes(line.Vertexs, bw);
                }
                else if(layer.ShapeType == SHAPETYPE.polygon)
                {
                    GISPolygon polygon = (GISPolygon)(feature.spatialPart);
                    WriteMultiVertexes(polygon.Vertexs, bw);
                }
                WriteAttributes(feature.attributePart, bw);
            }
        }
        //读取所有的GISFeature的空间部分和属性部分
        static void ReadFeatures(GISLayer layer, BinaryReader br, int FeatureCount)
        {
            for (int featureindex = 0; featureindex < FeatureCount; featureindex++)
            {
                GISFeature feature = new GISFeature(null, null);
                if (layer.ShapeType == SHAPETYPE.point)
                    feature.spatialPart = new GISPoint(new GISVertex(br));
                else if (layer.ShapeType == SHAPETYPE.line)
                    feature.spatialPart = new GISLine(ReadMultiVertexes(br));
                else if (layer.ShapeType == SHAPETYPE.polygon)
                    feature.spatialPart = new GISPolygon(ReadMultiVertexes(br));
                feature.attributePart = ReadAttributes(layer.Fields, br);
                layer.AddFeature(feature);
            }
        }
        //从文件读取字段信息
        static List<GISField> ReadFilds(BinaryReader br, int FieldCount)
        {
            List<GISField> fields = new List<GISField>();
            for (int fieldindex = 0; fieldindex < FieldCount; fieldindex++)
            {
                Type fieldtype = GISTools.IntToType(br.ReadInt32());
                string fieldname = GISTools.ReadString(br);
                fields.Add(new GISField(fieldtype, fieldname));
            }
            return fields;
        }
    }
    //数据类型枚举类型
    public enum ALLTYPES
    {
        System_Boolean,
        System_Byte,
        System_Char,
        System_Decimal,
        System_Double,
        System_Single,
        System_Int32,
        System_Int64,
        System_SByte,
        System_Int16,//短整型
        System_String,
        System_UInt32,
        System_UInt64,
        System_UInt16
    };
    //点选结果
    public enum SelectResult
    {
        //正常选择状态：选择到一个结果
        OK,
        //错误选择状态：备选集为空
        EmptySet,
        //错误选择状态：点击选择时距离空间对象太远
        TooFar,
        //错误选择状态：未知空间对象
        UnknownType
    };
    //点选的选择过程
    public class GISSelect
    {
        public GISFeature SelectedFeature = null;//存储点和线的选择结果
        public List<GISFeature> SelectedFeatures = new List<GISFeature>();//存储面的选择结果
        public SelectResult Select(GISVertex vertex, List<GISFeature> features, SHAPETYPE shapetype, GISView view)
        {
            if (features.Count == 0) return SelectResult.EmptySet;
            GISExtent MinSelectExtent = BuildExtent(vertex, view);
            switch (shapetype)
            {
                case SHAPETYPE.point:
                    return SelectPoint(vertex, features, view, MinSelectExtent);
                case SHAPETYPE.line:
                    return SelectLine(vertex, features, view, MinSelectExtent);
                case SHAPETYPE.polygon:
                    return SelectPolygon(vertex, features, view, MinSelectExtent);
            }
            return SelectResult.UnknownType;
        }
        public SelectResult Select(GISExtent extent, List<GISFeature> Features)
        {
            SelectedFeatures.Clear();
            for (int i = 0; i < Features.Count; i++)
            {
                if (extent.Include(Features[i].spatialPart.extent))
                    SelectedFeatures.Add(Features[i]);
            }
            return (SelectedFeatures.Count > 0) ? SelectResult.OK : SelectResult.TooFar;
        }
        //点选面实体
        private SelectResult SelectPolygon(GISVertex vertex, List<GISFeature> features, GISView view, GISExtent minSelectExtent)
        {
            SelectedFeatures.Clear();
            for (int i = 0; i < features.Count; i++)
            {
                if (minSelectExtent.InsertectOrNot(features[i].spatialPart.extent) == false) continue;
                GISPolygon polygon = (GISPolygon)(features[i].spatialPart);
                if (polygon.Include(vertex))
                    SelectedFeatures.Add(features[i]);
            }
            return (SelectedFeatures.Count > 0) ? SelectResult.OK : SelectResult.TooFar;
        }

        //点选线实体
        private SelectResult SelectLine(GISVertex vertex, List<GISFeature> features, GISView view, GISExtent minSelectExtent)
        {
            Double distance = Double.MaxValue;
            int id = -1;
            for(int i = 0; i < features.Count; i++)
            {
                if (minSelectExtent.InsertectOrNot(features[i].spatialPart.extent) == false) continue;
                GISLine line = (GISLine)(features[i].spatialPart);
                double dist = line.Distance(vertex);
                if(dist < distance)
                {
                    distance = dist;
                    id = i;
                }
            }
            if(id == -1)
            {
                SelectedFeature = null;
                return SelectResult.TooFar;
            }
            else
            {
                double screendistance = view.ToScreenDistance(distance);
                if (screendistance <= GISConst.MinScreenDistance)
                {
                    SelectedFeature = features[id];
                    return SelectResult.OK;
                }
                else
                {
                    SelectedFeature = null;
                    return SelectResult.TooFar;
                }
            }
        }

        public GISExtent BuildExtent(GISVertex vertex, GISView view)
        {
            Point p0 = view.ToScreenPoint(vertex);
            Point p1 = new Point(p0.X + (int)GISConst.MinScreenDistance, p0.Y + (int)GISConst.MinScreenDistance);
            Point p2 = new Point(p0.X - (int)GISConst.MinScreenDistance, p0.Y - (int)GISConst.MinScreenDistance);
            GISVertex gp1 = view.ToMapVertex(p1);
            GISVertex gp2 = view.ToMapVertex(p2);
            return new GISExtent(gp1.x, gp2.x, gp1.y, gp2.y);
        }
        //选择点
        public SelectResult SelectPoint(GISVertex vertex, List<GISFeature> features, GISView view,GISExtent MinSelectExtent)
        {
            double distance = Double.MaxValue;
            int id = -1;
            for (int i = 0; i < features.Count; i++)
            {
                if (MinSelectExtent.InsertectOrNot(features[i].spatialPart.extent) == false) continue;
                GISPoint point = (GISPoint)(features[i].spatialPart);
                double dist = point.Distance(vertex);
                if(dist<distance)
                {
                    distance = dist;
                    id = i;
                }
            }
            if (id == -1)
            {
                SelectedFeature = null;
                return SelectResult.TooFar;
            }
            else
            {
                double screendistance = view.ToScreenDistance(vertex, features[id].spatialPart.centroId);
                if(screendistance <= GISConst.MinScreenDistance)
                {
                    SelectedFeature = features[id];
                    return SelectResult.OK;
                }
                else
                {
                    SelectedFeature = null;
                    return SelectResult.TooFar;
                }
            }
        }
    }
    public class GISConst
    {
        //设置鼠标缩放操作时的系数
        public static double ZoomInFactor = 0.8;
        public static double ZoomOutFactor = 0.8;

        public static double MinScreenDistance = 5;
        //点的颜色和半径
        public static Color PointColor = Color.Pink;
        public static int PointSize = 3;
        //线的颜色和宽度
        public static Color LineColor = Color.CadetBlue;
        public static int LineWidth = 2;
        //面的边框颜色、填充颜色及边框宽度
        public static Color PolygonBoundaryColor = Color.White;
        public static Color PolygonFillColor = Color.Gray;
        public static int PolygonBoundaryWidth = 2;
        //被选中点的颜色
        public static Color SelectedPointColor = Color.Red;
        //被选中线的颜色
        public static Color SelectedLineColor = Color.Blue;
        //被选中面的填充颜色
        public static Color SelectedPolygonFillColor = Color.Yellow;
        //绘制选择或缩放范围框式的填充颜色
        public static Color ZoomSelectBoxColor = Color.FromArgb(50, 0, 0, 0);
    }

    //鼠标动作枚举:空操作，选择，放大，缩小，平移。
    public enum MOUSECOMMAND
    {
        Unused, Select, ZoomIn, ZoomOut, Pan
    };
}