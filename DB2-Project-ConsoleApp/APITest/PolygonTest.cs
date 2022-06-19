using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;

namespace APITest
{
    
    
    /// <summary>
    ///This is a test class for PolygonTest and is intended
    ///to contain all PolygonTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PolygonTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Null
        ///</summary>
        [TestMethod()]
        public void NullTest()
        {
            Polygon actual;
            actual = Polygon.Null;
            Polygon expected = new Polygon();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for IsNull
        ///</summary>
        [TestMethod()]
        public void IsNullTest()
        {
            Polygon target = new Polygon();
            bool expected = false;
            bool actual;
            actual = target.IsNull;
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for AddPoint
        ///</summary>
        [TestMethod()]
        public void AddPointTest()
        {

            Polygon target = new Polygon();
            Point p = new Point(0, 0);
            target.AddPoint = p;
            List<Point> points = new List<Point>();
            points.Add(p);
            Polygon expected = new Polygon(points);
            string actual = target.ToString();
            Assert.AreEqual(expected.ToString(), actual);
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            Polygon target = new Polygon();
            string expected = string.Empty;
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [TestMethod()]
        public void ParseTest()
        {
            SqlString s = new SqlString("1;1;2;2;3;3");
            List<Point> points = new List<Point>();
            points.Add(new Point(1, 1));
            points.Add(new Point(2, 2));
            points.Add(new Point(3, 3));
            Polygon expected = new Polygon(points);
            Polygon actual;
            actual = Polygon.Parse(s);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for orientation
        ///</summary>
        [TestMethod()]
        [DeploymentItem("SqlClassLibrary.dll")]
        public void orientationTest()
        {
            Point p1 = new Point(1, 1);
            Point p2 = new Point(1, 3);
            Point p3 = new Point(7, 5);
            int expected = 1;
            int actual;
            actual = Polygon_Accessor.orientation(p1, p2, p3);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for onSegment
        ///</summary>
        [TestMethod()]
        [DeploymentItem("SqlClassLibrary.dll")]
        public void onSegmentTest()
        {
            Point p1 = new Point(12, 7);
            Point p2 = new Point(8, 3); // TODO: Initialize to an appropriate value
            Point p3 = new Point(12, 63); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Polygon_Accessor.onSegment(p1, p2, p3);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsSimple
        ///</summary>
        [TestMethod()]
        public void IsSimpleTest()
        {
            Polygon target = Polygon.Parse("-1;-1;1;1;1;-1;-1;1");
            bool expected = false;
            bool actual;
            actual = target.IsSimple();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsInside
        ///</summary>
        [TestMethod()]
        public void IsInsideTest()
        {
            Polygon target = Polygon.Parse("-1;-1;1;-1;1;1");
            Point p = new Point(0.5, 0);
            bool expected = true;
            bool actual;
            actual = target.IsInside(p);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for edgesIntersect
        ///</summary>
        [TestMethod()]
        [DeploymentItem("SqlClassLibrary.dll")]
        public void edgesIntersectTest()
        {
            Point p1 = new Point(-1, -1);
            Point p2 = new Point(1, 1);
            Point p3 = new Point(-1, 1);
            Point p4 = new Point(1, -1);
            bool expected = true;
            bool actual;
            actual = Polygon_Accessor.edgesIntersect(p1, p2, p3, p4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CheckLeft
        ///</summary>
        [TestMethod()]
        [DeploymentItem("SqlClassLibrary.dll")]
        public void CheckLeftTest()
        {
            Polygon_Accessor target = new Polygon_Accessor();
            Point p1 = new Point(5, 5);
            Point p2 = new Point(3, 3);
            Point p3 = new Point(-1, -3);
            double expected = -4;
            double actual;
            actual = target.CheckLeft(p1, p2, p3);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Area
        ///</summary>
        [TestMethod()]
        public void AreaTest()
        {
            Polygon target = Polygon.Parse("-1;-1;1;1;1;-1");
            SqlDouble expected = new SqlDouble(2);
            SqlDouble actual;
            actual = target.Area();
            Assert.AreEqual(expected, actual);
        }
    }
}
