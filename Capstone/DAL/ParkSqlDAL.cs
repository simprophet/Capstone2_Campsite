using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.Classes;
using System.Data.SqlClient;
using System.Configuration;

namespace Capstone.DAL
{
    public class ParkSqlDAL
    {
        private string connectionString;

        public ParkSqlDAL(string connection)
        {
            this.connectionString = connection;
        }

        public List<Park> GetParks()
        {
            List<Park> parks = new List<Park>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd;
                    conn.Open();

                    cmd = new SqlCommand("SELECT * FROM park;", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        parks.Add(PopulateParkObject(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ParkSqlDal.GetParks() error" + ex.Message);
                throw;
            }
            return parks;
        }

        private Park PopulateParkObject(SqlDataReader reader)
        {
            return new Park()
            {
                ParkID = Convert.ToInt32(reader["park_id"]),
                Name = Convert.ToString(reader["name"]),
                Location = Convert.ToString(reader["location"]),
                EstablishDate = Convert.ToDateTime(reader["establish_date"]),
                Area = Convert.ToInt32(reader["area"]),
                AnnualVisitors = Convert.ToInt32(reader["visitors"]),
                Description = Convert.ToString(reader["description"])
            };
        }
    }
}
