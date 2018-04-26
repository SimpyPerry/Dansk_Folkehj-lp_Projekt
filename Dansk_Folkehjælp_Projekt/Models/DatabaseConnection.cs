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
            
                string query = "SELECT ItemID, ItemName, Amount, MinAmount, BoxID, BookcaseName, Location "
                    + "FROM STORAGE WHERE ItemName LIKE'%" + itemName + "%'";
            

                using (SqlConnection Connect = new SqlConnection(connectionString))
                {
                    Connect.Open();
                    SqlCommand GetByName = new SqlCommand(query, Connect);

                    using (SqlDataReader reader = GetByName.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                        int DB_ItemID = reader.GetInt32(0);
                            string DB_name = reader.GetString(1);
                            int DB_amount = reader.GetInt32(2);
                            int DB_minAmount = reader.GetInt32(3);
                            string DB_box = reader.GetString(4);
                            string DB_bookcaseID = reader.GetString(5);
                            string DB_location = reader.GetString(6);

                            GetStorages.Add(new Storage() { itemID=DB_ItemID, itemName = DB_name, amount = DB_amount, minAmount = DB_amount, boxID = DB_box, bookcaseName = DB_bookcaseID, location = DB_location });
                        }

                    }
                }
            }
        public void AddNewItem(string itemName, int amount, int minAmount, string boxID, string bookcaseName, string location)
        {
            string query = "INSERT into STORAGE(ItemName, Amount, MinAmount, BoxID, BookcaseName, Location) VALUES ( '" + itemName + "','" + amount + "','" + minAmount + "','" + boxID + "','" + location + "')";
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                Connect.Open();
                SqlCommand AddItemToTable = new SqlCommand(query, Connect);
            }
        }
        //Metode til at vælge hvilket lager man vil se genstande fra
        public void ShowStorage(string location)
        {
            if (location == "Container")
            {
                string query = "SELECT ItemName, Amount, MinAmount"
                        + "FROM STORAGE WHERE Storage =" + "'" + location + "'";

                using (SqlConnection Connect = new SqlConnection(connectionString))
                {
                    Connect.Open();
                    SqlCommand GetByStorage = new SqlCommand(query, Connect);

                    using (SqlDataReader reader = GetByStorage.ExecuteReader())
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
                        + "FROM STORAGE WHERE Storeage =" + "'" + location + "'";

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
        
        public void EditItem(int itemID, string itemName, int amount, int minAmount, string boxID, string bookcase, string location)
        {
            string query = string.Format("UPDATE STORAGE SET ItemName='{0}', Amount='{1}', MinAmount='{2}', BoxID='{3}', BookcaseName='{4}', Location='{5}' WHERE ItemID='{6}'", itemName, amount, minAmount, boxID, bookcase, location, itemID);
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                SqlCommand edit = new SqlCommand(query, Connect);
                Connect.Open();
                edit.ExecuteNonQuery();
                Connect.Close();
            }
        }
    }
}

