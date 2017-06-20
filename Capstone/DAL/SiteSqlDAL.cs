using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class SiteSqlDAL
    {

        private string connectionString;

        public SiteSqlDAL(string connection)
        {
            this.connectionString = connection;
        }

        public List<Site> GetSites(int selectedCampground)
        {
            List<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd;
                    conn.Open();

                    cmd = new SqlCommand("SELECT * FROM site WHERE campground_id = @campground_id;", conn);
                    cmd.Parameters.AddWithValue("@campground_id", selectedCampground);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        sites.Add(PopulateSiteObject(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SiteSqlDal.GetSites() error" + ex.Message);
                throw;
            }
            return sites;
        }

        private Site PopulateSiteObject(SqlDataReader reader)
        {
            return new Site()
            {
                SiteID = Convert.ToInt32(reader["site_id"]),
                CampgroundID = Convert.ToInt32(reader["campground_id"]),
                SiteNumber = Convert.ToInt32(reader["site_number"]),
                MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]),
                IsAccessible = Convert.ToBoolean(reader["accessible"]),
                MaxRVLength = Convert.ToInt32(reader["max_rv_length"]),
                Utilities = Convert.ToBoolean(reader["utilities"])
            };
        }
    }
}
