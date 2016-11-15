using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Configuration;
using System.Data.SqlClient;
using Capstone.Models;
using System.Collections;

namespace Capstone.DAL.Tests
{
    [TestClass()]
    public class SiteSqlDALTests
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

                //create a few campgrounds in the parks
                cmd = new SqlCommand("INSERT INTO campground(park_id, name, open_from_mm, open_to_mm, daily_fee) " +
                    "VALUES (" + id1 + ", 'Campground 1', 1, 12, 20.00), " +
                    "(" + id1 + ", 'Campground 2', 2, 8, 40.00), " +
                    "(" + id2 + ", 'Campground 3', 6, 9, 220.00), " +
                    "(" + id2 + ", 'Campground 4', 5, 11, 1.00);", conn);
                cmd.ExecuteNonQuery();

                List<Campground> clist = new List<Campground>();

                //get the campground IDs for these campgrounds                
                cmd = new SqlCommand("SELECT campground_id, name FROM campground;", conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Campground c = new Campground();
                    c.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                    c.Name = Convert.ToString(reader["name"]);

                    clist.Add(c);
                }
                reader.Close();


                int id3 = 0;
                id3 = clist.Find(c => c.Name == "Campground 1").CampgroundID;
                int id4 = 0;
                id4 = clist.Find(c => c.Name == "Campground 2").CampgroundID;
                // create a few sites in the campgrounds
                cmd = new SqlCommand("INSERT INTO site( campground_id, site_number,max_occupancy, accessible, max_rv_length, utilities) " +
                    "VALUES(" + id3 + ", 20,5,1,23,0)," +
                        "(" + id4 + ", 21,5,1,23,0)," +
                        "(" + id3 + ", 22, 5, 1, 23, 0);", conn);
                cmd.ExecuteNonQuery();

            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }


        [TestMethod()]
        public void GetSitesByCampgroundTest()
        {
            SiteSqlDAL dal = new SiteSqlDAL(connectionString);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();


                //get the site IDs                 
                SqlCommand cmd = new SqlCommand("SELECT * FROM site;", conn);
                List<Site> slist = new List<Site>();
                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Site s = new Site();
                    s.SiteID = Convert.ToInt32(reader["site_id"]);
                    s.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                    s.SiteNumber = Convert.ToInt32(reader["site_number"]);
                    s.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                    s.Accessibile = Convert.ToBoolean(reader["accessible"]);
                    s.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                    s.Utilities = Convert.ToBoolean(reader["utilities"]);


                    slist.Add(s);
                }
                reader.Close();

                int id1 = 0;
                id1 = slist.Find(s => s.SiteNumber == 20).CampgroundID;
                int id2 = 0;
                id2 = slist.Find(s => s.SiteNumber == 21).CampgroundID;

                conn.Close();
                DateTime startDate = new DateTime(2016, 9, 15);
                DateTime endDate = new DateTime(2016, 9, 22);

                List<Site> s1 = dal.GetSitesByCampground(id1, startDate, endDate);


                Assert.AreEqual(2, s1.Count);
                Assert.IsNotNull(s1);




            }
        }
    }
}
