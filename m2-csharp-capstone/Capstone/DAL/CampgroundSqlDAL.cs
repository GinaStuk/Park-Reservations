using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class CampgroundSqlDAL
    {

        private string connectionString;

        // Single Parameter Constructor
        public CampgroundSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Campground> GetCampgroundsByPark(int parkID)
        {
            List<Campground> cList = new List<Campground>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("Select * from campground where park_id = @pid ORDER BY name asc;", connection);

                    command.Parameters.AddWithValue("@pid", parkID);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground c = new Campground();
                        c.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                        c.ParkID = Convert.ToInt32(reader["park_id"]);
                        c.Name = Convert.ToString(reader["name"]);
                        c.OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]);
                        c.OpenToMonth = Convert.ToInt32(reader["open_to_mm"]);
                        c.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

                        cList.Add(c);
                    }
                }
            }
            catch (SqlException ex)
            {
                //some sort of log 
                Console.WriteLine(ex.ToString());
                throw;
            }
            return cList;
        }








        //get all campgrounds may or may not actually be used
        public List<Campground> GetAllCampgrounds()
        {
            List<Campground> cList = new List<Campground>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("Select * from campground order by park_id asc, name asc;", connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground c = new Campground();
                        c.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                        c.ParkID = Convert.ToInt32(reader["park_id"]);
                        c.Name = Convert.ToString(reader["name"]);
                        c.OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]);
                        c.OpenToMonth = Convert.ToInt32(reader["open_to_mm"]);
                        c.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

                        cList.Add(c);
                    }
                }
            }
            catch (SqlException ex)
            {
                //some sort of log 
                Console.WriteLine(ex.ToString());
                throw;
            }
            return cList;
        }
    }
}
