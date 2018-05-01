using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dansk_Folkehjælp_Projekt.Models
{
    public class DatabaseConnection
    {
        
            //Forbinder til databasen
            public ObservableCollection <Storage> GetStorages { get; set; }
            private static string connectionString = "Server=EALSQL1.eal.local; Database= DB2017_A21; User ID = USER_A21; Password=SesamLukOp_21;";
            public List<string> bookcases { get; set; }

        public List<string> MailList { get; set; }

        public DatabaseConnection()
            {
                GetStorages = new ObservableCollection<Storage>();

            }

            //Metode til at søge efter alle genstande med navn der minder om ItemName
            public void FindByItemName(string itemName)
            {
            GetStorages = new ObservableCollection<Storage>();

            int DB_minAmount;
            string DB_box;
            string DB_bookcaseID;
            string checkIfExists = "SELECT COUNT(*) FROM STORAGE WHERE ItemName LIKE'%" + itemName + "%'";
                string query = "SELECT ItemID, ItemName, Amount, MinAmount, BoxID, BookcaseName, Location "
                    + "FROM STORAGE WHERE ItemName LIKE'%" + itemName + "%'";
            
            
                using (SqlConnection Connect = new SqlConnection(connectionString))
                {
             SqlCommand CheckFirst = new SqlCommand(checkIfExists, Connect);
                    Connect.Open();

                int records = (int)CheckFirst.ExecuteScalar();

             if(records!=0)
                {
                    SqlCommand GetByName = new SqlCommand(query, Connect);

                    using (SqlDataReader reader = GetByName.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            int DB_ItemID = reader.GetInt32(0);
                            string DB_name = reader.GetString(1);

                            int DB_amount = reader.GetInt32(2);
                            if (reader[3] != DBNull.Value)
                            {

                                DB_minAmount = reader.GetInt32(3);
                            }
                            else { DB_minAmount = 0; }

                            if (reader[4] != DBNull.Value)
                            {
                                DB_box = reader.GetString(4);
                            }
                            else
                            {
                                DB_box = "";
                            }
                            if (reader[5] != DBNull.Value)
                            {
                                DB_bookcaseID = reader.GetString(5);
                            }
                            else
                            {
                                DB_bookcaseID = "";

                            }
                            string DB_location = reader.GetString(6);

                            GetStorages.Add(new Storage() { itemID = DB_ItemID, itemName = DB_name, amount = DB_amount, minAmount = DB_minAmount, boxID = DB_box, bookcaseName = DB_bookcaseID, location = DB_location });
                        }

                    }
                }

             else
                {
                    GetStorages.Add(new Storage() {  itemName = "Eksisterer ikke" });
                }

                
                }
            }
        //Opretter ny genstand
        public void AddNewItem(string itemName, int amount, int minAmount, string boxID, string bookcaseName, string location)
        {
            string query = "INSERT into STORAGE(ItemName, Amount, MinAmount, BoxID, BookcaseName, Location) VALUES ( '" + itemName + "','" + amount + "','" + minAmount + "','" + boxID + "','" + bookcaseName + "','" + location + "')";
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                Connect.Open();
                SqlCommand AddItemToTable = new SqlCommand(query, Connect);

                AddItemToTable.ExecuteNonQuery();

                Connect.Close();
                
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
        
        //Redigere genstand info
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
        public void ShowBookcases()
        {
            bookcases = new List<string>();
            string query = "SELECT BookcaseName FROM Bookcase";

            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                SqlCommand showCase = new SqlCommand(query, connect);
                connect.Open();
                using (SqlDataReader reader = showCase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string BookcaseName = reader.GetString(0);
                        bookcases.Add(BookcaseName);
                    }
                }
                
            }

        }
        public void AddNewBookcase(string bookcaseName)
        {
            string query = "INSERT into Bookcase(BookcaseName) VALUES ('" + bookcaseName + "')";
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                Connect.Open();
                SqlCommand AddBookcaseToTable = new SqlCommand(query, Connect);

                AddBookcaseToTable.ExecuteNonQuery();

                Connect.Close();

            }
        }
        public bool SeeIfExsists(string bookcaseName)
        {
            string query = "SELECT BookcaseName FROM Bookcase WHERE BookcaseName = '"+bookcaseName+"'";
            using (SqlConnection Conncet = new SqlConnection(connectionString))
            {
                Conncet.Open();
                SqlCommand Exsits = new SqlCommand(query, Conncet);
                string empty = null;
                empty = (string)Exsits.ExecuteScalar();

                if(empty == null)
                {
                    return true;
                }else
                {
                    return false;
                }

            }
        }

        public void AddNotificationMail(string mail)
        {
            //AddNotificationMail()
            string query = String.Format("INSERT INTO MailAddresses(MailAddress)VALUES('{0}'); ", mail);
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                Connect.Open();
                SqlCommand newMail = new SqlCommand(query, Connect);

                newMail.ExecuteNonQuery();

                Connect.Close();

            }
        }
        public void GetMailAddresses()
        {
            MailList = new List<string>();
            string query = "SELECT MailAddress FROM MailAddresses";

            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                SqlCommand GetAddress = new SqlCommand(query, connect);
                connect.Open();
                using (SqlDataReader reader = GetAddress.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string mail = reader.GetString(0);
                        MailList.Add(mail);
                    }
                }

                connect.Close();

            }
        }

        public void DeleteMailAddress()
        {

        }
    }
}

