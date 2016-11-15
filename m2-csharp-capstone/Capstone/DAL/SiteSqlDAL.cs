using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Capstone.DAL
{
     public class SiteSqlDAL
    {

        private string connectionString;

        // Single Parameter Constructor
        public SiteSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Site> GetSitesByCampground(int campgroundID, DateTime startDate, DateTime endDate)
        {
            List<Site> sList = new List<Site>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //this giant SQL statement picks the top 5 campgrounds
                    //that either are in the reservation but NOT currently occupied
                    //or have never been reserved
                    string giantSqlCommand = "SELECT TOP 5 * FROM site WHERE ( " +
                        "campground_id = @cid) " +
                        "AND (((site_id IN " +
                            "(SELECT site_id FROM reservation WHERE ( " +
                                "(from_date > @fdate AND to_date > @fdate) " +
                                    "OR (to_date < @tdate AND from_date < @tdate)" + 
                                "AND (from_date > @fdate AND to_date < @tdate)))))" +
                            "OR (site_id NOT IN (SELECT site_id FROM reservation)));" ;

                    SqlCommand command = new SqlCommand(giantSqlCommand, connection);

                    command.Parameters.AddWithValue("@cid", campgroundID);
                    command.Parameters.AddWithValue("@fdate", startDate);
                    command.Parameters.AddWithValue("@tdate", endDate);

                    SqlDataReader reader = command.ExecuteReader();

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

                        
                        sList.Add(s);
                    }
                }
            }
            catch (SqlException ex)
            {
                //some sort of log 
                Console.WriteLine(ex.ToString());
                throw;
            }
            return sList;
        }
    }
}
