using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Configuration;
using System.Data.SqlClient;
using Capstone.DAL;
using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.Tests
{
    [TestClass]
    public class ParkSqlDALTest
    {
        private TransactionScope tran;
        string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //clear all data
                SqlCommand cmd = new SqlCommand("DELETE from reservation; DELETE from site; DELETE from campground; DELETE from park;", conn);
                cmd.ExecuteNonQuery();

                //create a park for testing
                cmd = new SqlCommand("INSERT into park(name, location, establish_date, area, visitors, description)" +
                    " VALUES ('testPark','outside','2014-10-11','57', '12','Cracked parking lot near the Baker Electric Building');", conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }


        [TestMethod]
        public void GetParks()
        {
            ParkSqlDAL dal = new ParkSqlDAL(connectionString);

            List<Park> parks = dal.GetParks();

            Assert.AreEqual(1, parks.Count);
            Assert.IsNotNull(parks);
            Assert.AreEqual("testPark", parks[0].Name);
        }
    }
}
