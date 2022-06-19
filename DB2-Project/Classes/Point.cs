using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined, IsByteOrdered = true, MaxByteSize = 64)]
public class Point : INullable, IBinarySerialize
{
    // coordinates
    private double m_x;
    private double m_y;

    // constructors
    public Point()
    {
        m_x = Double.NaN;
        m_y = Double.NaN;
    }

    public Point(double x, double y)
    {
        m_x = x;
        m_y = y;
    }

    // x property
    public double X
    {
        get { return m_x; }
        set { m_x = value; }
    }

    // y property
    public double Y
    {
        get { return m_y; }
        set { m_y = value; }
    }

    // ToString
    public override string ToString()
    {
        return "(" + m_x.ToString() + ", " + m_y.ToString() + ")";
    }

    // Parser
    public static Point Parse(SqlString s)
    {
        if (s.IsNull)
        {
            return new Point();
        }
            

        string[] args = s.Value.Split(";".ToCharArray());

        if (args.Length != 2)
        {
            throw new ArgumentException("Wrong number of arguments!");
        }

        double x;
        double y;

        try
        {
            x = double.Parse(args[0]);
            y = double.Parse(args[1]);
        }
        catch
        {
            throw new ArgumentException("Wrong arguments type!");
        }

        return new Point(x, y);
    }

    // IsNull
    public bool IsNull
    {
        get
        {
            return (Double.IsNaN(m_x) || Double.IsNaN(m_y));
        }
    }

    public static Point Null
    {
        get
        {
            return new Point();
        }
    }

    // returns euclidean distance to another point
    public SqlDouble DistanceTo(Point p2)
    {
        return Math.Sqrt(Math.Pow(m_x - p2.m_x, 2) + Math.Pow(m_y - p2.m_y, 2));
    }

    // returns euclidean distance of two points
    public static SqlDouble Distance(Point p1, Point p2)
    {
        return p1.DistanceTo(p2);
    }


    // serialization
    #region IBinarySerialize Members

    public void Write(System.IO.BinaryWriter writer)
    {
        writer.Write(m_x);
        writer.Write(m_y);
    }

    public void Read(System.IO.BinaryReader reader)
    {
        m_x = reader.ReadDouble();
        m_y = reader.ReadDouble();
    }
    #endregion

}


