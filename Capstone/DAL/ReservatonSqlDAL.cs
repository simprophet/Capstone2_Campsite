using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class ReservatonSqlDAL
    {
        private string connectionString;

        public ReservatonSqlDAL(string connection)
        {
            this.connectionString = connection;
        }

        public List<Site> CheckReservations(List<Site> sites, DateTime userArrival, DateTime userDepart)
        {
            List<Site> availableSites = new List<Site>();

            for (int i = 0; i < sites.Count; i++)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd;
                        conn.Open();

                        cmd = new SqlCommand("SELECT * FROM reservation WHERE ((@arrivalDate BETWEEN from_date AND to_date) " +
                            "OR (@departDate BETWEEN from_date AND to_date)) AND @site_id = site_id;", conn);
                        cmd.Parameters.AddWithValue("@arrivalDate", userArrival.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@departDate", userDepart.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@site_id",sites[i].SiteID);

                        SqlDataReader reader = cmd.ExecuteReader();
                        if(!reader.HasRows)
                        {
                            availableSites.Add(sites[i]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ReservationSqlDal.CheckReservations() error" + ex.Message);
                    throw;
                }

            }
            return availableSites;
        }

        public int ConfirmReservation(Site selectedSite, string reservationName, DateTime userArrival, DateTime userDepart)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd;
                    conn.Open();

                    cmd = new SqlCommand("INSERT INTO reservation (site_id, name, from_date, to_date, create_date) " +
                        "VALUES (@site_id, @name, @from_date, @to_date, @create_date); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                    cmd.Parameters.AddWithValue("@site_id", selectedSite.SiteID);
                    cmd.Parameters.AddWithValue("@name", reservationName);
                    cmd.Parameters.AddWithValue("@from_date", userArrival.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@to_date", userDepart.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@create_date", DateTime.Now.ToString("yyyy-MM-dd"));

                    int newId = (int)cmd.ExecuteScalar();
                    return newId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReservationSqlDAL.ConfirmReservation() error" + ex.Message);
                throw;
            }
        }

        public int GetConfirmationId(string searchReservationName)
        {
            int confirmationId;
            Reservation newReservation = new Reservation();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd;
                    conn.Open();

                    cmd = new SqlCommand("SELECT * FROM reservation WHERE name = @searchName", conn);
                    cmd.Parameters.AddWithValue("@searchName", searchReservationName);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                       newReservation = PopulateReservationObject(reader);
                    }
                    confirmationId = newReservation.ReservationID;
                }
            }
            catch (Exception ex)
            {
                 Console.WriteLine("ReservationSqlDAL.GetConfirmationId() error" + ex.Message);
                throw;
            }
            return confirmationId;
        }
       
        private Reservation PopulateReservationObject(SqlDataReader reader)
        {
            return new Reservation()
            {
                ReservationID = Convert.ToInt32(reader["reservation_id"]),
                SiteID = Convert.ToInt32(reader["site_id"]),
                Name = Convert.ToString(reader["name"]),
                FromDate = Convert.ToDateTime(reader["from_date"]),
                ToDate = Convert.ToDateTime(reader["to_date"]),
                CreateDate = Convert.ToDateTime(reader["create_date"])
            };
        }
    }
}
