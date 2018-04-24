using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dansk_Folkehjælp_Projekt.Models
{
    public class DatabaseConnection
    {
        
            //Forbinder til databasen
            public List<Storage> GetStorages { get; set; }
            private static string connectionString = "Server=EALSQL1.eal.local; Database= DB2017_A21; User ID = USER_A21; Password=SesamLukOp_21;";

            public DatabaseConnection()
            {
                GetStorages = new List<Storage>();

            }

            //Metode til at søge efter genstande med navn der minder om ItemName
            public void FindByItemName(string itemName)
            {
            GetStorages = new List<Storage>();
            itemName = "Klud";
                string query = "SELECT ItemName, Amount, MinAmount, BoxID, BookcaseName, Location "
                    + "FROM STORAGE WHERE ItemName LIKE'%" + itemName + "%'";
            GetStorages = new List<Storage>();

                using (SqlConnection Connect = new SqlConnection(connectionString))
                {
                    Connect.Open();
                    SqlCommand GetByName = new SqlCommand(query, Connect);

                    using (SqlDataReader reader = GetByName.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string DB_name = reader.GetString(0);
                            int DB_amount = reader.GetInt32(1);
                            int DB_minAmount = reader.GetInt32(2);
                            string DB_box = reader.GetString(3);
                            string DB_bookcaseID = reader.GetString(4);
                            string DB_location = reader.GetString(5);

                            GetStorages.Add(new Storage() { itemName = DB_name, amount = DB_amount, minAmount = DB_amount, boxID = DB_box, bookcaseName = DB_bookcaseID, location = DB_location });
                        }

                    }
                }
            }
        public void ShowStorage(string storage)
        {
            if (storage == "Container")
            {
                string query = "SELECT ItemName, Amount, MinAmount"
                        + "FROM STORAGE WHERE Storeage =" + storage;

                using (SqlConnection Connect = new SqlConnection(connectionString))
                {
                    Connect.Open();
                    SqlCommand GetByName = new SqlCommand(query, Connect);

                    using (SqlDataReader reader = GetByName.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string DB_name = reader.GetString(0);
                            int DB_amount = reader.GetInt32(1);
                            int DB_minAmount = reader.GetInt32(2);

                            GetStorages.Add(new Storage() { itemName = DB_name, amount = DB_amount, minAmount = DB_amount });
                        }

                    }
                }
            }
            else
            {
                string query = "SELECT ItemName, Amount, MinAmount, BoxID, BookcaseName"
                        + "FROM STORAGE WHERE Storeage =" + storage;

                using (SqlConnection Connect = new SqlConnection(connectionString))
                {
                    Connect.Open();
                    SqlCommand GetByName = new SqlCommand(query, Connect);

                    using (SqlDataReader reader = GetByName.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string DB_name = reader.GetString(0);
                            int DB_amount = reader.GetInt32(1);
                            int DB_minAmount = reader.GetInt32(2);
                            string DB_box = reader.GetString(3);
                            string DB_bookcaseID = reader.GetString(4);

                            GetStorages.Add(new Storage() { itemName = DB_name, amount = DB_amount, minAmount = DB_amount, boxID = DB_box, bookcaseName = DB_bookcaseID });
                        }

                    }
                }
            }
        }
        
        
    }
}

