using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ReservationSqlDAL
    {

        private string connectionString;

        // Single Parameter Constructor
        public ReservationSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public int GetReservations(int siteID, DateTime startDate, DateTime endDate,
            string name)
        {
            //List<Reservation> rList = new List<Reservation>();
            int maxID = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string SqlCommand = "INSERT into reservation" +
                        "(site_id, name, from_date, to_date, create_date)" +
                 "Values(@siteID, @name, @sDate, @eDate, @currentDate); ";

                    connection.Open();
                    SqlCommand command = new SqlCommand(SqlCommand, connection);
                    command.Parameters.AddWithValue("@siteID", siteID);
                    command.Parameters.AddWithValue("@name",name);
                    command.Parameters.AddWithValue("@sDate", startDate);
                    command.Parameters.AddWithValue("@eDate", endDate);
                    command.Parameters.AddWithValue("@currentDate", DateTime.Now);

                    command.ExecuteNonQuery();

                    //reservation added, now need to get back the confirmation number.  

                    command = new SqlCommand("select MAX(reservation_id) as MaxID from reservation", connection);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation r = new Reservation();
                        maxID = Convert.ToInt32(reader["MaxID"]);
                        //r.Name = Convert.ToString(reader["name"]);
                        //r.FromDate = Convert.ToDateTime(reader["from_date"]);
                        //r.ToDate = Convert.ToDateTime(reader["to_date"]);
                        //r.CreateDate = Convert.ToDateTime(reader["create_date"]);

                        //rList.Add(r);
                    }
                }
            }
            catch (SqlException ex)
            {
                //some sort of log 
                Console.WriteLine(ex.ToString());
                throw;
            }
            return maxID;

        }
    }
}
