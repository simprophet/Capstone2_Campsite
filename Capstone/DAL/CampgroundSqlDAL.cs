using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class CampgroundSqlDAL
    {
        private string connectionString;

        public CampgroundSqlDAL(string connection)
        {
            this.connectionString = connection;
        }

        public List<Campground> GetCampgrounds(Park p)
        {
            List<Campground> campgrounds = new List<Campground>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd;
                    conn.Open();

                    cmd = new SqlCommand("SELECT * FROM campground WHERE park_id = @p_id;", conn);
                    cmd.Parameters.AddWithValue("@p_id", p.ParkID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        campgrounds.Add(PopulateCampgroundObject(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ParkSqlDal.GetParks() error" + ex.Message);
                throw;
            }
            return campgrounds;

        }
       
        private Campground PopulateCampgroundObject(SqlDataReader reader)
        {
            return new Campground()
            {
                CampgroundId = Convert.ToInt32(reader["campground_id"]),
                ParkId = Convert.ToInt32(reader["park_id"]),
                Name = Convert.ToString(reader["name"]),
                OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]),
                OpenTillMonth = Convert.ToInt32(reader["open_to_mm"]),
                DailyFee = Convert.ToDecimal(reader["daily_fee"])
            };
        }
    }
}
