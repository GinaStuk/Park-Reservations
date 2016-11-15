using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Configuration;
using System.Data.SqlClient;
using Capstone.DAL;
using Capstone.Models;
using System.Collections.Generic;
using Capstone.Tests;

namespace Capstone.Tests
{
    [TestClass]
    public class CampgroundSqlDALTest
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
                cmd = new SqlCommand("INSERT into park(name, location, establish_date, area, visitors, description) " +
                    "VALUES ('testPark','outside','2014-10-11','57', '12','Cracked parking lot near the Baker Electric Building'), " +
                        "('testPark2','outside','2016-10-12','99999', '1','Some random dude came to the same place over and over playing pokemon go'); ", conn);
                cmd.ExecuteNonQuery();

                //get the park IDs for these parks                
                cmd = new SqlCommand("SELECT park_id, name FROM park;", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                List<Park> plist = new List<Park>();
                while(reader.Read())
                {
                    Park p = new Park();
                    p.ParkID = Convert.ToInt32(reader["park_id"]);
                    p.Name = Convert.ToString(reader["name"]);

                    plist.Add(p);
                }
                reader.Close();//don't forget to close reader or program breaks

                int id1 = 0;
                id1 = plist.Find(p => p.Name == "testPark").ParkID;
                int id2 = 0;
                id2 = plist.Find(p => p.Name == "testPark2").ParkID;

                //create a few campgrounds in the parks
                cmd = new SqlCommand("INSERT INTO campground(park_id, name, open_from_mm, open_to_mm, daily_fee) " + 
                    "VALUES (" + id1 + ", 'Campground 1', 1, 12, 20.00), " +
                    "(" + id1 + ", 'Campground 2', 2, 8, 40.00), " +
                    "(" + id2 + ", 'Campground 3', 6, 9, 220.00), " +
                    "(" + id2 + ", 'Campground 4', 5, 11, 1.00);", conn);
                cmd.ExecuteNonQuery();


            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }



        [TestMethod]
        public void GetCampgroundByParkTest()
        {
            CampgroundSqlDAL dal = new CampgroundSqlDAL(connectionString);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();


                //get the park IDs for these parks                
                SqlCommand cmd = new SqlCommand("SELECT park_id, name FROM park;", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                List<Park> plist = new List<Park>();

                while (reader.Read())
                {
                    Park p = new Park();
                    p.ParkID = Convert.ToInt32(reader["park_id"]);
                    p.Name = Convert.ToString(reader["name"]);

                    plist.Add(p);
                }
                reader.Close();

                int id1 = 0;
                id1 = plist.Find(p => p.Name == "testPark").ParkID;
                int id2 = 0;
                id2 = plist.Find(p => p.Name == "testPark2").ParkID;

                conn.Close();

                List<Campground> cpark1 = dal.GetCampgroundsByPark(id1);
                List<Campground> cpark2 = dal.GetCampgroundsByPark(id2);

                Assert.AreEqual(2, cpark1.Count);
                Assert.IsNotNull(cpark2);
                Assert.AreEqual("Campground 3", cpark2[0].Name);
                Assert.AreEqual(8, cpark1[1].OpenToMonth);


            }
        }

        [TestMethod]
        public void GetAllCampgroundsTest()
        {
            CampgroundSqlDAL dal = new CampgroundSqlDAL(connectionString);

            List<Campground> campgrounds = dal.GetAllCampgrounds();

            Assert.AreEqual(4, campgrounds.Count);
            Assert.IsNotNull(campgrounds);
            Assert.AreEqual("Campground 1", campgrounds[0].Name);
            Assert.AreEqual(11, campgrounds[3].OpenToMonth);

        }
    }
}
