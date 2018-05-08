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
            public ObservableCollection<Storage> GetBags { get; set; }
            public ObservableCollection<Storage> GetItems { get; set; }

        public ObservableCollection<Storage> BagTypes { get; set; }
        public ObservableCollection<Storage> BagTypeRequirements { get; set; }
        public ObservableCollection<Storage> ItemFromBag { get; set; }
        public ObservableCollection<Storage> ChosenItemFromBag { get; set; }

        public List<string> MailList { get; set; }

        public DatabaseConnection()
            {
            ChosenItemFromBag = new ObservableCollection<Storage>();      
                GetStorages = new ObservableCollection<Storage>();
                InitBagData();
            InitBagTypes();
            GetAllItems();
            
            }

        public void GetAllItems()
        {
            ItemFromBag = new ObservableCollection<Storage>();
            string query = "select ItemID, ItemName from Item";
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                Connect.Open();
                SqlCommand GetAllItems = new SqlCommand(query, Connect);
                using (SqlDataReader reader = GetAllItems.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ID = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        ItemFromBag.Add(new Storage() { itemID = ID, itemName = name });
                    }
                }
                Connect.Close();
            }
        }

        public void InitBagTypes()
        {
            BagTypes = new ObservableCollection<Storage>();
            string query = "select * from BagType";

            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                Connect.Open();
                SqlCommand GetBagTypes = new SqlCommand(query, Connect);

                using (SqlDataReader reader = GetBagTypes.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ID = reader.GetInt32(0);
                        string typeName = reader.GetString(1);
                      

                        BagTypes.Add(new Storage() {itemID=ID, itemName=typeName });
                    }

                }
                Connect.Close();
            }
            

        }

        public void GetItemRequirementsForTypes(int bagTypeId)
        {
            BagTypeRequirements = new ObservableCollection<Storage>();
            string query = string.Format("select Type_Item.Item, Item.ItemName, Type_Item.Minimum from " +
                "Type_Item inner join Item on Type_Item.Item=Item.ItemID where Type={0}",bagTypeId);

            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                Connect.Open();
                SqlCommand GetTypeItems = new SqlCommand(query, Connect);
                using (SqlDataReader reader = GetTypeItems.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        int minimum = reader.GetInt32(2);
                        BagTypeRequirements.Add(new Storage() { itemID = id, itemName = name, minAmount = minimum });
                    }

                }
                Connect.Close();
            }
        }

            //Metode til at søge efter alle genstande med navn der minder om ItemName
            public void FindByItemName(string itemName)
            {
            GetStorages = new ObservableCollection<Storage>();

            int DB_minAmount;
            string DB_box;
            string DB_bookcaseID;
            string checkIfExists = "SELECT COUNT(*) FROM ITEM WHERE ItemName LIKE'%" + itemName + "%'";
                string query2 = "SELECT ItemID, ItemName, Amount, MinAmount, BoxID, BookcaseName, Location "
                    + "FROM STORAGE WHERE ItemName LIKE'%" + itemName + "%'";

            string query = "Select Item.ItemID, Item.ItemName, Item.Amount, Item.MinAmount, " +
      "Item.BoxID, Bookcase.BookcaseName, Item.Location From Item inner join Bookcase on " +
      "Item.Bookcase = Bookcase.BookcaseID" +
" Where ItemName LIKE'%" + itemName + "%'";


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

        public int GetBookcaseID(string name)
        {
            int ID = 0;
            string query = String.Format("select BookcaseID From bookcase where bookcasename = '{0}'", name);
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                SqlCommand FindByName = new SqlCommand(query, Connect);
                Connect.Open();

                using (SqlDataReader reader = FindByName.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ID = reader.GetInt32(0);
                    }
                }
            }
            return ID;
        }
        //Opretter ny genstand
        public void AddNewItem(string itemName, int amount, int minAmount, string boxID, string bookcaseName, string location)
        {
            int bookcase = GetBookcaseID(bookcaseName);

            string query = "INSERT into Item(ItemName, Amount, MinAmount, BoxID, Bookcase, Location) VALUES ( '" + itemName + "','" + amount + "','" + minAmount + "','" + boxID + "','" + bookcase + "','" + location + "')";
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
        public void DeleteFromTypeRequirements(int typeID, int itemID)
        {
            string query = String.Format("Delete from Type_Item where type={0} and item={1}", typeID, itemID);
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                SqlCommand deleteRequirement = new SqlCommand(query, Connect);
                Connect.Open();
                deleteRequirement.ExecuteNonQuery();
                Connect.Close();
            }

        }

        public void AddBag(string name, int type)
        {
            ObservableCollection<Storage> RequirementsForType = new ObservableCollection<Storage>();
            string query = String.Format("Insert into Bag Values('{0}',{1})", name, type);
            string itemRequirements = String.Format("Select Item from Type_Item Where Type={0}", type);
            
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                SqlCommand AddedBag = new SqlCommand(query, Connect);
                SqlCommand FindItemsViaTypes = new SqlCommand(itemRequirements, Connect);
               

                Connect.Open();
                AddedBag.ExecuteNonQuery();


                using (SqlDataReader reader = FindItemsViaTypes.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int ID = reader.GetInt32(0);
                        RequirementsForType.Add(new Storage() { itemID = ID });
                    }
                }
                InitBagData();

                Storage newlyAddedBag = GetBags[GetBags.Count - 1];
                
                int items = RequirementsForType.Count;
                for(int i=0;i<items;i++)
                {
                    string bag_Item = String.Format("Insert into Bag_Item Values ({0},{1},0)", newlyAddedBag.itemID, RequirementsForType[i].itemID);
                    SqlCommand AddBagsRequirements = new SqlCommand(bag_Item, Connect);
                    AddBagsRequirements.ExecuteNonQuery();

                }


                    Connect.Close();
            }
        }
        public void EditMinimumForType(int typeID, int itemID, int minimum)
        {
            string query = String.Format("Update Type_Item set Minimum={0} where Type={1} and Item={2}", minimum, typeID, itemID);
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                SqlCommand editMinimum = new SqlCommand(query, Connect);
                Connect.Open();
                editMinimum.ExecuteNonQuery();
                Connect.Close();
            }

        }
        //Redigere genstand info
        public void EditItem(int itemID, string itemName, int amount, int minAmount, string boxID, string bookcase, string location)
        {
            int bookcaseID = GetBookcaseID(bookcase);
            string query = string.Format("UPDATE STORAGE SET ItemName='{0}', Amount='{1}', MinAmount='{2}', BoxID='{3}', BookcaseName='{4}', Location='{5}' WHERE ItemID='{6}'", itemName, amount, minAmount, boxID, bookcaseID, location, itemID);
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
        public void SelectBag(int type)
        {
            GetBags = new ObservableCollection<Storage>();
            string query = String.Format("SELECT ID, BagName FROM Bag WHERE Type ={0}", type); 

            using(SqlConnection Conncet = new SqlConnection(connectionString))
            {
                SqlCommand getBags = new SqlCommand(query, Conncet);
                Conncet.Open();
                using(SqlDataReader reader = getBags.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int bagsID = reader.GetInt32(0);
                        string bagsName = reader.GetString(1);

                        GetBags.Add(new Storage() { itemID = bagsID, itemName = bagsName });
                    }
                }
            }
        }
        public void InitBagData()
        {
            GetBags = new ObservableCollection<Storage>();
            string query = String.Format("SELECT ID, BagName FROM Bag");
            using (SqlConnection Conncet = new SqlConnection(connectionString))
            {
                SqlCommand getBags = new SqlCommand(query, Conncet);
                Conncet.Open();
                using (SqlDataReader reader = getBags.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int bagsID = reader.GetInt32(0);
                        string bagsName = reader.GetString(1);

                        GetBags.Add(new Storage() { itemID = bagsID, itemName = bagsName });
                    }
                }
            }
        }
        public void GetBagItems(int bagID)
        {
            GetItems = new ObservableCollection<Storage>();
            string query = String.Format("Select Bag.ID, Item.ItemName, Type_Item.Minimum, Bag_Item.Amount, Item.ItemID" 
+" From Bag inner join Type_Item on Bag.Type = Type_Item.Type"
 + " inner join Bag_Item on  Bag.ID = Bag_Item.Bag AND Bag_Item.Item = Type_Item.Item inner join Item on Type_Item.Item = Item.ItemID " +
 "Where Bag.ID = {0}", bagID);
            using(SqlConnection Connect = new SqlConnection(connectionString))
            {
                SqlCommand GetItemsFromBag = new SqlCommand(query, Connect);
                Connect.Open();
                using(SqlDataReader reader = GetItemsFromBag.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int number = 0;
                        string name = reader.GetString(1);
                        int Min = reader.GetInt32(2);
                        if(reader[3] != DBNull.Value)
                        {
                            number = reader.GetInt32(3);
                        }
                        else
                        {
                            number = 0;
                        }
                        int itemNr = reader.GetInt32(4);

                        GetItems.Add(new Storage(){itemName=name, minAmount=Min, amount=number, itemID = itemNr  });
                        
                    }
                }
            }
        }
        public void GetItemFromBag(string bagName, int itemID)
        {
            ItemFromBag = new ObservableCollection<Storage>();
            string query = String.Format (@"Select Item.ItemName, Type_Item.Minimum, Bag_Item.Amount, Item.Location, Item.BoxID, Bookcase.BookcaseName, Item.Amount
                            FROM Bag INNER JOIN Bag_Item ON Bag_Item.Bag = Bag.ID
                            INNER JOIN Item ON Item.ItemID = Bag_Item.Item
                            INNER JOIN Type_Item ON Type_Item.Item = Item.ItemID
                            INNER JOIN Bookcase ON Bookcase.BookcaseID = Item.Bookcase
                            WHERE Item.ItemID ='{0}' AND Bag.BagName = '{1}'", itemID, bagName);
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                SqlCommand SpecificItemFromBag = new SqlCommand(query, Connect);
                Connect.Open();
                using(SqlDataReader reader = SpecificItemFromBag.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int AmountNumber = 0;
                        int StorageAmount = 0;
                        
                        string name = reader.GetString(0);
                        int min = reader.GetInt32(1);
                        if(reader[2] != DBNull.Value)
                        {
                            AmountNumber = reader.GetInt32(2);
                        }
                        else
                        {
                            AmountNumber = 0;
                        }
                        string loca = reader.GetString(3);
                        string box = reader.GetString(4);
                        string bCase = reader.GetString(5);
                        if(reader[6] != DBNull.Value)
                        {
                            StorageAmount = reader.GetInt32(6);
                        }
                        else
                        {
                            StorageAmount = 0;
                        }
                        


                        ChosenItemFromBag.Add(new Storage { itemName = name, minAmount = min, amount = AmountNumber, location = loca, boxID = box, bookcaseName = bCase, itemID = StorageAmount });

                    }
                }
                
            }
        }
        public void AddItemToRequirements(int typeID, int itemID, int minimum)
        {

            string query = String.Format("Insert into Type_Item values ({0},{1},{2})", typeID, itemID, minimum); 
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                Connect.Open();
                SqlCommand NewTypeRequirement = new SqlCommand(query, Connect);

                NewTypeRequirement.ExecuteNonQuery();

                Connect.Close();

            }
        }
        public void TakeItemFromStorageToBag(string bagName, string itemName, int amountRemoved)
        {
            string query = string.Format(@"UPDATE Bag_Item SET Bag_Item.Anount = Bag_Item.Amount + {0} WHERE", amountRemoved);
        }
    }
}

