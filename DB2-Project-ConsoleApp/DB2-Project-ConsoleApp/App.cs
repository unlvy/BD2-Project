using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DB2_Project_ConsoleApp
{
    class App
    {
        private static String sqlConnection = "Data Source=MSSQLSERVER72;Initial Catalog=AdventureWorks2008;Integrated Security=True";

        public void Run()
        {
            int option;
            while (true)
            {
                DisplayMenu();
                option = GetInputInt(0, 9, " Please choose option: ");

                switch (option)
                {
                    case 0:
                        return;
                    case 1:
                        AddPoint();
                        break;
                    case 2:
                        AddPolygon();
                        break;
                    case 3:
                        DeletePoint();
                        break;
                    case 4:
                        DeletePolygon();
                        break;
                    case 5:
                        displayPoints();
                        break;
                    case 6:
                        displayPolygons();
                        break;
                    case 7:
                        calcDistance();
                        break;
                    case 8:
                        calcArea();
                        break;
                    case 9:
                        CheckIfPointInside();
                        break;
                    default:
                        break;
                }

            }
        }

        private void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("-                Main Menu                 -");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("- 1. Add point                             -");
            Console.WriteLine("- 2. Add polygon                           -");
            Console.WriteLine("- 3. Delete point                          -");
            Console.WriteLine("- 4. Delete polygon                        -");
            Console.WriteLine("- 5. Display points                        -");
            Console.WriteLine("- 6. Display polygons                      -");
            Console.WriteLine("- 7. Calculate distance between two points -");
            Console.WriteLine("- 8. Calculate area of polygon             -");
            Console.WriteLine("- 9. Check if point is inside polygon      -");
            Console.WriteLine("-                                          -");
            Console.WriteLine("- 0. Exit                                  -");
            Console.WriteLine("--------------------------------------------");
        }

        private void AddPoint()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("-                Add point                 -");
            Console.WriteLine("--------------------------------------------");
            double x = GetInputDouble(Double.MinValue, Double.MaxValue, " Please enter x: ");
            double y = GetInputDouble(Double.MinValue, Double.MaxValue, " Please enter y: ");

            String q = "INSERT INTO dbo.Points VALUES (@v)";
            SqlConnection c = new SqlConnection(sqlConnection);

            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(q, c);
                cmd.Parameters.Add("@v", SqlDbType.VarChar);
                cmd.Parameters["@v"].Value = x.ToString() + ";" + y.ToString();
                cmd.ExecuteNonQuery();
                Console.WriteLine(" Point added ");
            }
            catch (SqlException e)
            {
                Console.WriteLine(" Error occured: " + e.Message);
            }
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(" Press enter to continue ... ");
            Console.ReadLine();

        }

        private void AddPolygon()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("-               Add polygon                -");
            
            List<double> values = new List<double>(); 

            int option = -1;
            while (option != 2)
            { 
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("- 1. Add next point                        -");
                Console.WriteLine("- 2. Finish                                -");
                Console.WriteLine("-                                          -");
                Console.WriteLine("- 0. Cancel                                -");
                Console.WriteLine("--------------------------------------------");
                option = GetInputInt(0, 2, "Please choose option: ");

                if (option == 1)
                {
                    double x = GetInputDouble(Double.MinValue, Double.MaxValue, " Please enter x: ");
                    double y = GetInputDouble(Double.MinValue, Double.MaxValue, " Please enter y: ");
                    values.Add(x);
                    values.Add(y);
                }
                else if (option == 0)
                {
                    return;
                }
            }

            if (values.Count() < 6)
            {
                Console.WriteLine("");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine(" Error: at least 3 points required! ");
                Console.WriteLine(" Press enter to continue ... ");
                Console.ReadLine();
                return;
            }

            String q = "INSERT INTO dbo.Polygons VALUES (@v)";
            SqlConnection c = new SqlConnection(sqlConnection);

            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(q, c);
                cmd.Parameters.Add("@v", SqlDbType.VarChar);
                String v = values[0].ToString();
                for (int i = 1; i < values.Count; i++)
                {
                    v += ";" + values[i].ToString();
                }
                cmd.Parameters["@v"].Value = v;
                cmd.ExecuteNonQuery();
                Console.WriteLine(" Polygon added ");
            }
            catch (SqlException e)
            {
                Console.WriteLine(" Error occured: " + e.Message);
            }
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(" Press enter to continue ... ");
            Console.ReadLine();
        }

        private void DeletePoint()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("-              Delete point                -");
            Console.WriteLine("--------------------------------------------");
            int id = GetInputInt(Int32.MinValue, Int32.MaxValue, " Please enter point ID: ");

            String q = "DELETE FROM dbo.Points WHERE ID = @v";
            SqlConnection c = new SqlConnection(sqlConnection);

            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(q, c);
                cmd.Parameters.Add("@v", SqlDbType.VarChar);
                cmd.Parameters["@v"].Value = id.ToString();
                cmd.ExecuteNonQuery();
                int r = cmd.ExecuteNonQuery();
                if (r == 1)
                {
                    Console.WriteLine(" Point deleted ");
                }
                else 
                {
                    Console.WriteLine(" Such Point does not exist ");
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(" Error occured: " + e.Message);
            }
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(" Press enter to continue ... ");
            Console.ReadLine();
        }

        private void DeletePolygon() 
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("-             Delete polygon               -");
            Console.WriteLine("--------------------------------------------");
            int id = GetInputInt(Int32.MinValue, Int32.MaxValue, " Please enter polygon ID: ");

            String q = "DELETE FROM dbo.Polygons WHERE ID = @v";
            SqlConnection c = new SqlConnection(sqlConnection);

            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(q, c);
                cmd.Parameters.Add("@v", SqlDbType.VarChar);
                cmd.Parameters["@v"].Value = id.ToString();
                int r = cmd.ExecuteNonQuery();
                if (r == 1)
                {
                    Console.WriteLine(" Polygon deleted ");
                }
                else
                {
                    Console.WriteLine(" Such polygon does not exist ");
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(" Error occured: " + e.Message);
            }
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(" Press enter to continue ... ");
            Console.ReadLine();
        }

        private void displayPoints()
        {
            String q = "SELECT ID, point.ToString() AS point FROM dbo.Points";
            SqlConnection c = new SqlConnection(sqlConnection);

            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(q, c);
                SqlDataReader d = cmd.ExecuteReader();

                Console.Clear();
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("-                 Points                   -");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine(" {0,-3} {1,-30}", "ID", "(x, y)");
                Console.WriteLine("");

                while (d.Read())
                {
                    Console.WriteLine(" {0,-3} {1,30}", d["ID"].ToString(), d["point"].ToString());
                }
            }
            catch (SqlException e) 
            {
                Console.WriteLine(" Error occured: " + e.Message);
            }
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(" Press enter to continue ... ");
            Console.ReadLine();
        }

        private void displayPolygons()
        {
            String q = "SELECT ID, polygon.ToString() AS polygon FROM dbo.Polygons";
            SqlConnection c = new SqlConnection(sqlConnection);

            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(q, c);
                SqlDataReader d = cmd.ExecuteReader();

                Console.Clear();
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("-                Polygons                  -");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine(" {0,-3} {1}", "ID", "points");
                Console.WriteLine("");

                while (d.Read())
                {
                    Console.WriteLine(" {0,-3} {1}", d["ID"].ToString(), d["polygon"].ToString());
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(" Error occured: " + e.Message);
            }
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(" Press enter to continue ... ");
            Console.ReadLine();
        }

        private void calcDistance()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("-                Distance                  -");
            Console.WriteLine("--------------------------------------------");
            int id1 = GetInputInt(Int32.MinValue, Int32.MaxValue, " Please enter ID of first point: ");
            int id2 = GetInputInt(Int32.MinValue, Int32.MaxValue, " Please enter ID of second point: ");

            String q = @"DECLARE @p1 dbo.Point; SELECT @p1 = point FROM Points WHERE ID = @id1;
                         DECLARE @p2 dbo.Point; SELECT @p2 = point FROM Points WHERE ID = @id2;
                         SELECT @p1.ToString() AS p1, @p2.ToString() AS p2, @p1.DistanceTo(@p2) AS dist";
            SqlConnection c = new SqlConnection(sqlConnection);

            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(q, c);
                cmd.Parameters.Add("@id1", SqlDbType.Int);
                cmd.Parameters.Add("@id2", SqlDbType.Int);
                cmd.Parameters["@id1"].Value = id1;
                cmd.Parameters["@id2"].Value = id2;
                SqlDataReader d = cmd.ExecuteReader();
                while (d.Read())
                { 
                    Console.WriteLine(" {0,-20} {1,-20} {2, -10}", "Point 1", "Point 2", "Distance");
                    Console.WriteLine(" {0,-20} {1,-20} {2, -10}", d["p1"].ToString(), d["p2"].ToString(), d["dist"].ToString());
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(" Error occured: " + e.Message);
            }
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(" Press enter to continue ... ");
            Console.ReadLine();
        }

        private void calcArea()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("-                  Area                    -");
            Console.WriteLine("--------------------------------------------");
            int id = GetInputInt(Int32.MinValue, Int32.MaxValue, " Please enter ID of polygon: ");

            String q = "SELECT polygon.ToString() AS p, polygon.Area() AS a FROM Polygons WHERE ID = @id;";
            SqlConnection c = new SqlConnection(sqlConnection);

            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(q, c);
                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = id;
                SqlDataReader d = cmd.ExecuteReader();
                while (d.Read())
                {
                    if (d["a"].ToString() == "-1")
                    {
                        Console.WriteLine("Unable to calculate area - polygon contains intersecting edges!");
                    }
                    else
                    {
                        Console.WriteLine("Polygon: " + d["p"].ToString());
                        Console.WriteLine("Area: " + d["a"].ToString());
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(" Error occured: " + e.Message);
            }
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(" Press enter to continue ... ");
            Console.ReadLine();
        }

        private void CheckIfPointInside()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("-             Point in polygon             -");
            Console.WriteLine("--------------------------------------------");
            int id1 = GetInputInt(Int32.MinValue, Int32.MaxValue, " Please enter ID of point: ");
            int id2 = GetInputInt(Int32.MinValue, Int32.MaxValue, " Please enter ID of polygon: ");

            String q = @"DECLARE @p1 dbo.Point; SELECT @p1 = point FROM Points WHERE ID = @id1;
                         DECLARE @p2 dbo.Polygon; SELECT @p2 = polygon FROM Polygons WHERE ID = @id2;
                         SELECT @p1.ToString() AS p1, @p2.ToString() AS p2, @p2.IsInside(@p1) AS b";
            SqlConnection c = new SqlConnection(sqlConnection);

            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(q, c);
                cmd.Parameters.Add("@id1", SqlDbType.Int);
                cmd.Parameters.Add("@id2", SqlDbType.Int);
                cmd.Parameters["@id1"].Value = id1;
                cmd.Parameters["@id2"].Value = id2;
                SqlDataReader d = cmd.ExecuteReader();
                while (d.Read())
                {
                    Console.WriteLine(" Point: " + d["p1"].ToString());
                    Console.WriteLine(" Polygon: " + d["p2"].ToString());
                    Console.WriteLine(" Is inside: " + d["b"].ToString());
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(" Error occured: " + e.Message);
            }
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(" Press enter to continue ... ");
            Console.ReadLine();
        }

        private int GetInputInt(int min, int max, string info)
        {
            int result = -1;
            bool valid = false;
            String input;
            Console.Write(info);

            while (!valid)
            {
                input = Console.ReadLine();
                try
                {
                    result = Convert.ToInt32(input);
                    if (result >= min && result <= max)
                    {
                        valid = true;
                    }
                    else
                    {
                        Console.Write(" Wrong number. Please try again: ");
                    }
                }
                catch (Exception ignored)
                {
                    Console.Write(" Bad entry. Please try again: ");
                }
            }
            return result;
        }

        private double GetInputDouble(double min, double max, string info)
        {
            double result = -1;
            bool valid = false;
            String input;
            Console.Write(info);

            while (!valid)
            {
                input = Console.ReadLine();
                try
                {
                    result = Convert.ToDouble(input);
                    if (result >= min && result <= max)
                    {
                        valid = true;
                    }
                    else
                    {
                        Console.Write(" Wrong number. Please try again: ");
                    }
                }
                catch (Exception ignored)
                {
                    Console.Write(" Bad entry. Please try again: ");
                }
            }
            return result;
        }

    }
}
