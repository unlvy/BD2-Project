using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlTypes;
using System.IO;
using System;

namespace APITest
{
    
    
    /// <summary>
    ///This is a test class for PointTest and is intended
    ///to contain all PointTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PointTest
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
        ///A test for Y
        ///</summary>
        [TestMethod()]
        public void YTest()
        {
            Point target = new Point(-3, -3.3);
            double expected = -3.3;
            double actual;
            target.Y = expected;
            actual = target.Y;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for X
        ///</summary>
        [TestMethod()]
        public void XTest()
        {
            Point target = new Point(2.5, 0);
            double expected = 2.5;
            double actual;
            target.X = expected;
            actual = target.X;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Null
        ///</summary>
        [TestMethod()]
        public void NullTest()
        {
            Point actual;
            double x = Double.NaN;
            double y = Double.NaN;
            actual = Point.Null;
            Assert.AreEqual(x, actual.X);
            Assert.AreEqual(y, actual.Y);
        }

        /// <summary>
        ///A test for IsNull
        ///</summary>
        [TestMethod()]
        public void IsNullTest()
        {
            Point target = new Point();
            bool expected = true;
            bool actual;
            actual = target.IsNull;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            Point target = new Point(3, 3);
            string expected = "(3, 3)";
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
            SqlString s = new SqlString("5;5");
            Point expected = new Point(5, 5);
            Point actual;
            actual = Point.Parse(s);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for DistanceTo
        ///</summary>
        [TestMethod()]
        public void DistanceToTest()
        {
            Point target = new Point(1, 1);
            Point p2 = new Point(1, 5);
            SqlDouble expected = new SqlDouble(4);
            SqlDouble actual;
            actual = target.DistanceTo(p2);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Distance
        ///</summary>
        [TestMethod()]
        public void DistanceTest()
        {
            Point p1 = new Point(5, 5);
            Point p2 = new Point(5, 10);
            SqlDouble expected = new SqlDouble(5);
            SqlDouble actual;
            actual = Point.Distance(p1, p2);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Point Constructor
        ///</summary>
        [TestMethod()]
        public void PointConstructorTest1()
        {
            double x = Double.NaN;
            double y = Double.NaN;
            Point target = new Point();
            Assert.AreEqual(x, target.X);
            Assert.AreEqual(y, target.Y);
        }

        /// <summary>
        ///A test for Point Constructor
        ///</summary>
        [TestMethod()]
        public void PointConstructorTest()
        {
            double x = 2;
            double y = 2;
            Point target = new Point(2, 2);
            Assert.AreEqual(x, target.X);
            Assert.AreEqual(y, target.Y);
        }
    }
}
