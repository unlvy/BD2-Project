using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined, IsByteOrdered = true, MaxByteSize = -1)]
public class Polygon : INullable, IBinarySerialize
{
    // vertexes
    private List<Point> m_points;

    // constructors
    public Polygon() 
    {
        m_points = new List<Point>();
    }

    public Polygon(List<Point> points)
    {
        m_points = points;
    }    

    // IsNull
    public bool IsNull
    {
        get
        {
            return false;
        }
    }

    // ToString
    public override string ToString()
    {
        if (m_points.Count < 3)
        {
            return "";
        }

        string result = "";
        foreach (Point p in m_points)
        {
            result += p.ToString() + " - ";
        }
        return result + m_points[0];
    }

    // Null
    public static Polygon Null
    {
        get
        {
            return new Polygon();
        }
    }

    // Parser
    public static Polygon Parse(SqlString s)
    {
        if (s.IsNull)
        {
            return new Polygon();
        }


        string[] args = s.Value.Split(";".ToCharArray());

        if (args.Length < 6)
        {
            throw new ArgumentException("Wrong number of arguments!");
        }

        double x;
        double y;

        List<Point> points = new List<Point>();
        

        for (int i = 0; i < args.Length; i += 2)
        {
            Console.WriteLine(args[i]);
            Console.WriteLine(args[i + 1]);
            try
            {
                x = double.Parse(args[i]);
                y = double.Parse(args[i + 1]);
                points.Add(new Point(x, y));
            }
            catch
            {
                throw new ArgumentException("Wrong arguments type!");
            }
        }
        
        return new Polygon(points);
    }

    // property adding Point to vertices
    public Point AddPoint
    {
        set { m_points.Add(new Point(value.X, value.Y)); }
    }

    // function checking if point p2 lies on segment p1 - p3
    private static bool onSegment(Point p1, Point p2, Point p3)
    {
        if (p2.X <= Math.Max(p1.X, p3.X) && p2.X >= Math.Min(p1.X, p3.X)
            && p2.Y <= Math.Max(p1.Y, p3.Y) && p2.Y >= Math.Min(p1.Y, p3.Y))
        {
            return true;
        }
        return false;
    }

    // function finding orientation of input Points triplet
    private static int orientation(Point p1, Point p2, Point p3)
    {
        double value = (p2.Y - p1.Y) * (p3.X - p2.X)
                - (p2.X - p1.X) * (p3.Y - p2.Y);
        if (-1.0E-12 < value && value < 1.0E-12)
        {
            return 0;
        }
        return (value > 0) ? 1 : 2;
    }

    // function checking if two edges intersect
    private static bool edgesIntersect(Point p1, Point p2, Point p3, Point p4)
    {
        int o1 = orientation(p1, p2, p3);
        int o2 = orientation(p1, p2, p4);
        int o3 = orientation(p3, p4, p1);
        int o4 = orientation(p3, p4, p2);

        if (o1 != o2 && o3 != o4)
            return true;

        if (o1 == 0 && onSegment(p1, p3, p2)) return true;
        if (o2 == 0 && onSegment(p1, p4, p2)) return true;
        if (o3 == 0 && onSegment(p3, p1, p4)) return true;
        if (o4 == 0 && onSegment(p3, p2, p4)) return true;

        return false;
    }

    // Property checking if Polygon is simple
    public bool IsSimple()
    {

        int numVertices = m_points.Count;

        // Check if input is a polygon
        if (numVertices < 3)
        {
            return false;
        }

        // Check if input is a simple polygon
        // Using brute force - just check if any edges intersect
        for (int i = 0; i < numVertices - 1; i++)
        {
            for (int j = i + 2; j < numVertices; j++)
            {
                // skip already checked checked / not valid edges
                if ((i == 0) && (j == (numVertices - 1)))
                {
                    continue;
                }
                if (edgesIntersect(m_points[i], m_points[i + 1], m_points[j], m_points[(j + 1) % numVertices]))
                {
                    return false;
                }
            }
        }
        return true;
    }

    // calculates area of a polygon
    public SqlDouble Area()
    {
        int numVertices = m_points.Count;
        if (!IsSimple())
        {
            return -1.0;
        }

        // Calculate area using shoelace formula
        double result = 0.0;
        for (int i = 0; i < numVertices - 1; i++)
        {
            result += m_points[i].X * m_points[i + 1].Y - m_points[i + 1].X * m_points[i].Y;
        }
        result = Math.Abs(result + m_points[numVertices - 1].X * m_points[0].Y - m_points[0].X * m_points[numVertices - 1].Y) / 2.0;
        return result;
    }

    // check position of point to line
    private double CheckLeft(Point p1, Point p2, Point p3)
    {
        return (((p2.Y - p1.Y) * (p3.X - p1.X) 
            - (p3.Y - p1.Y) * (p2.X - p1.X)));
    }

    // checks if Point is inside polygon
    public bool IsInside(Point p)
    {
        List<Point> tempPoints = m_points;
        tempPoints.Add(m_points[0]);
        int wn = 0;
        for (int i = 0; i < tempPoints.Count - 1; i++)
        {
            if (tempPoints[i].X <= p.X)
            {
                if (tempPoints[i + 1].X > p.X)
                {
                    if (CheckLeft(tempPoints[i], tempPoints[i + 1], p) > 0)
                    {
                        wn++;
                    }
                }
            }
            else
            {
                if (tempPoints[i + 1].X <= p.X)
                {
                    if (CheckLeft(tempPoints[i], tempPoints[i + 1], p) < 0)
                    {
                        wn--;
                    }
                }
                    
            }
        }
        return !(wn == 0);
    }

    // serialization
    #region IBinarySerialize Members

    public void Read(System.IO.BinaryReader r)
    {
        int j = r.ReadInt32();
        for (int i = 0; i < j; i++)
        {
            Point tmp = new Point();
            tmp.Read(r);
            m_points.Add(tmp);
        }
    }

    public void Write(System.IO.BinaryWriter writer)
    {
        writer.Write(m_points.Count);
        foreach (Point p in m_points)
        {
            p.Write(writer);
        }
    }
    #endregion

}


