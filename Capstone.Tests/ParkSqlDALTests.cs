using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Data.SqlClient;
using Capstone;
using System.Configuration;

namespace Capstone.Tests
{
    [TestClass]
    public class ParkSqlDALTests
    {
        private TransactionScope tran;
        private string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                conn.Open();
                cmd = new SqlCommand("INSERT INTO park Values('Jellystone National Park', 'Ohio', '1958-01-31', 8808, 4200001, 'This park is cartoonishly delightful!');", conn);
                cmd.ExecuteNonQuery();


            }


        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }




        [TestMethod]
        public void TestMethod1()
        {
            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{



            //}

        }
    }
}
