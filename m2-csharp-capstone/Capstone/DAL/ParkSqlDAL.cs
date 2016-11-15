using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ParkSqlDAL
    {
        private string connectionString;


        // Single Parameter Constructor
        public ParkSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Park> GetParks()
        {
            List<Park> pList = new List<Park>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("Select * from park ORDER BY name asc", connection);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Park p = new Park();
                        p.ParkID = Convert.ToInt32(reader["park_id"]);
                        p.Name = Convert.ToString(reader["name"]);
                        p.Location = Convert.ToString(reader["location"]);
                        p.EstablishedDate = Convert.ToDateTime(reader["establish_date"]);
                        p.Area = Convert.ToInt32(reader["area"]);
                        p.Visitors = Convert.ToInt32(reader["visitors"]);
                        p.Description = Convert.ToString(reader["description"]);

                        pList.Add(p);
                    }
                }
            }
            catch (SqlException ex)
            {
                //some sort of log 
                Console.WriteLine(ex.ToString());
                throw;
            }

            return pList;

        }
    }
}