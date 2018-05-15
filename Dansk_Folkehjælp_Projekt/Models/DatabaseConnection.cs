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
            public List<string> Bookcases { get; set; }
            public ObservableCollection<Storage> GetBags { get; set; }
            public ObservableCollection<Storage> GetItems { get; set; }

        public ObservableCollection<Storage> BagTypes { get; set; }
        public ObservableCollection<Storage> BagTypeRequirements { get; set; }
        public ObservableCollection<Storage> ItemFromDatabase { get; set; }
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
            ItemFromDatabase = new ObservableCollection<Storage>();
            //string query = "select ItemID, ItemName from Item";
            string query = "Select Item.ItemID, Item.ItemName, Item.Amount, Item.MinAmount, " +
      "Item.BoxID, Bookcase.BookcaseName, Item.Location From Item inner join Bookcase on " +
      "Item.Bookcase = Bookcase.BookcaseID";
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                Connect.Open();
                SqlCommand GetAllItems = new SqlCommand(query, Connect);
                using (SqlDataReader reader = GetAllItems.ExecuteReader())
                {
                    int amount = 0;
                    int minAmount = 0;
                    string boxID = "";
                    string bookCaseName = "";

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        if(reader[2]!=DBNull.Value)
                        {
                            amount = reader.GetInt32(2);
                        }

                        if (reader[3] != DBNull.Value)
                        {
                            minAmount = reader.GetInt32(3);
                        }
                        if(reader[4]!=DBNull.Value)
                        {
                          boxID  = reader.GetString(4);
                        }
                        
                         if(reader[5]!=DBNull.Value)
                        {
                         bookCaseName   = reader.GetString(5);
                        }
                       
                        string location = reader.GetString(6);
                        ItemFromDatabase.Add(new Storage() { ItemID = id, ItemName = name, Amount=amount, MinAmount=minAmount, BoxID= boxID, BookcaseName=bookCaseName, Location=location });
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
                      

                        BagTypes.Add(new Storage() {ItemID=ID, ItemName=typeName });
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
                        BagTypeRequirements.Add(new Storage() { ItemID = id, ItemName = name, MinAmount = minimum });
                    }

                }
                Connect.Close();
            }
        }
        public void UpdateBagsAfterRequirementsChanged(int bagType)
        {
            GetItemRequirementsForTypes(bagType);
            SelectBag(bagType);
            int itemsForThisType = BagTypeRequirements.Count;
            int bagsForThisType = GetBags.Count;
            
            for(int i=0; i<bagsForThisType;i++)
            {
                int bagID = GetBags[i].ItemID;
                //Se om tasken har denne ting i Bag_item
                for (int p=0;p<itemsForThisType;p++)
                {
                    int itemID = BagTypeRequirements[p].ItemID;
                  int o=  CheckIfBagHasItem(bagID, itemID);
                    if (o==0)
                    {
                        string query = String.Format("Insert into bag_item values({0},{1},0)", bagID, itemID);
                        using (SqlConnection Connect = new SqlConnection(connectionString))
                        {
                            SqlCommand AlterBag_Item = new SqlCommand(query, Connect);
                            Connect.Open();
                            AlterBag_Item.ExecuteNonQuery();
                            Connect.Close();

                        }
                    }
                    
                }
            }
        }
        public int CheckIfBagHasItem(int bagID, int itemID)
        {
            string checkIfExists = String.Format("SELECT COUNT(*) FROM BAG_ITEM WHERE Bag={0} And Item={1}", bagID, itemID);
            int matches;
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                SqlCommand Check = new SqlCommand(checkIfExists, Connect);
                Connect.Open();
                matches = (int)Check.ExecuteScalar();
            }

            return matches;
        }
        public void SelectBag(int type)
        {
            GetBags = new ObservableCollection<Storage>();
            string query = String.Format("SELECT ID, BagName FROM Bag WHERE Type ={0}", type);

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

                        GetBags.Add(new Storage() { ItemID = bagsID, ItemName = bagsName });
                    }
                }
            }
        }

        //Metode til at søge efter alle genstande med navn der minder om ItemName
        public void FindByItemName(string itemName)
            {
            GetStorages = new ObservableCollection<Storage>();

            int db_MinAmount;
            string db_Box;
            string db_BookcaseID;
            string checkIfExists = "SELECT COUNT(*) FROM ITEM WHERE ItemName LIKE'%" + itemName + "%'";
               
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
                    {   while (reader.Read()){
                      int db_ItemID = reader.GetInt32(0);
                            string db_Name = reader.GetString(1);

                            int db_Amount = reader.GetInt32(2);
                            if (reader[3] != DBNull.Value)
                            {   db_MinAmount = reader.GetInt32(3);
                            }
                            else { db_MinAmount = 0; }

                            if (reader[4] != DBNull.Value)
                            {
                                db_Box = reader.GetString(4);
                            }
                            else
                            {
                                db_Box = "";
                            }
                            if (reader[5] != DBNull.Value)
                            {
                                db_BookcaseID = reader.GetString(5);
                            }
                            else
                            {
                                db_BookcaseID = "";
                            }
                            string db_Location = reader.GetString(6);
                            GetStorages.Add(new Storage() { ItemID = db_ItemID, ItemName = db_Name, Amount = db_Amount,
                                MinAmount = db_MinAmount, BoxID = db_Box, BookcaseName = db_BookcaseID, Location = db_Location }); }}
                } else
                {
                    GetStorages.Add(new Storage() {  ItemName = "Eksisterer ikke" });
                }  }
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
                            string db_Name = reader.GetString(0);
                            int db_Amount = reader.GetInt32(1);
                            int db_MinAmount = reader.GetInt32(2);

                            GetStorages.Add(new Storage() { ItemName = db_Name, Amount = db_Amount, MinAmount = db_MinAmount });
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
                            string db_Name = reader.GetString(0);
                            int db_Amount = reader.GetInt32(1);
                            int db_MinAmount = reader.GetInt32(2);
                            string db_Box = reader.GetString(3);
                            string db_BookcaseID = reader.GetString(4);

                            GetStorages.Add(new Storage() { ItemName = db_Name, Amount = db_Amount, MinAmount = db_MinAmount, BoxID = db_Box, BookcaseName = db_BookcaseID });
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

                SelectBag(typeID);
                int bagsOfType = GetBags.Count;
                for(int i=0; i<bagsOfType;i++)
                {
                    string query2 = String.Format("Delete from Bag_Item Where bag={0} AND Item={1}", GetBags[i].ItemID, itemID);
                    SqlCommand deleteFromBag_Item = new SqlCommand(query2, Connect);
                    deleteFromBag_Item.ExecuteNonQuery();
                }
               
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
                        RequirementsForType.Add(new Storage() { ItemID = ID });
                    }
                }
                InitBagData();

                Storage newlyAddedBag = GetBags[GetBags.Count - 1];
                
                int items = RequirementsForType.Count;
                for(int i=0;i<items;i++)
                {
                    string bag_Item = String.Format("Insert into Bag_Item Values ({0},{1},0)", newlyAddedBag.ItemID, RequirementsForType[i].ItemID);
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
            string query = string.Format("UPDATE ITEM SET ItemName='{0}', Amount='{1}', MinAmount='{2}', BoxID='{3}', Location='{4}' WHERE ItemID='{5}'", itemName, amount, minAmount, boxID, location, itemID);
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
            Bookcases = new List<string>();
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
                        Bookcases.Add(BookcaseName);
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
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                Connect.Open();

                SqlCommand Exsits = new SqlCommand(query, Connect);
                string empty = null;
                empty = (string)Exsits.ExecuteScalar();

                if(empty == null)
                {
                    return true;
                }

                else
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
        
        public void InitBagData()
        {
            GetBags = new ObservableCollection<Storage>();
            string query = String.Format("SELECT ID, BagName FROM Bag");
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                SqlCommand getBags = new SqlCommand(query, Connect);
                Connect.Open();
                using (SqlDataReader reader = getBags.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int bagsID = reader.GetInt32(0);
                        string bagsName = reader.GetString(1);

                        GetBags.Add(new Storage() { ItemID = bagsID, ItemName = bagsName });
                    }
                }

                Connect.Close();
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
                        int min = reader.GetInt32(2);
                        if(reader[3] != DBNull.Value)
                        {
                            number = reader.GetInt32(3);
                        }
                        else
                        {
                            number = 0;
                        }
                        int itemNr = reader.GetInt32(4);

                        GetItems.Add(new Storage(){ItemName=name, MinAmount=min, Amount=number, ItemID = itemNr  });
                        
                    }
                }
            }
        }
        public void GetItemFromBag(string bagName, int itemID)
        {
            ItemFromDatabase = new ObservableCollection<Storage>();
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
                        int amountNumber = 0;
                        int storageAmount = 0;
                        
                        string name = reader.GetString(0);
                        int min = reader.GetInt32(1);
                        if(reader[2] != DBNull.Value)
                        {
                            amountNumber = reader.GetInt32(2);
                        }
                        else
                        {
                            amountNumber = 0;
                        }
                        string loca = reader.GetString(3);
                        string box = reader.GetString(4);
                        string bCase = reader.GetString(5);
                        if(reader[6] != DBNull.Value)
                        {
                            storageAmount = reader.GetInt32(6);
                        }
                        else
                        {
                            storageAmount = 0;
                        }


                   
                        ChosenItemFromBag.Add(new Storage { ItemName = name,  MinAmount = min, Amount = amountNumber, Location = loca, BoxID = box, BookcaseName = bCase, ItemID = storageAmount });

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
            string query = string.Format(@"UPDATE Bag_Item
						SET Bag_Item.Amount = Bag_Item.Amount +{0}
						FROM Bag_Item
						INNER JOIN Bag ON Bag_Item.Bag = Bag.ID
						INNER JOIN Item ON Item.ItemID = Bag_Item.Item
						WHERE Item.ItemName ='{2}' AND Bag.BagName = '{1}'


						UPDATE Item
						SET Item.Amount = Item.Amount -{0}
						FROM Item 
						INNER JOIN Bag_Item ON Bag_Item.Item = Item.ItemID
						INNER JOIN Bag ON Bag.ID = Bag_Item.Bag
						WHERE Bag.BagName ='{1}' AND Item.ItemName= '{2}'",amountRemoved, bagName, itemName);
            using(SqlConnection Connect = new SqlConnection(connectionString))
            {
                Connect.Open();
                SqlCommand RemoveFromStorageAddToBag = new SqlCommand(query, Connect);

                RemoveFromStorageAddToBag.ExecuteNonQuery();

                Connect.Close();
            }
        }
        public void RemoveItemFromBag(string bagName, string itemName, int amountRemoved)
        {
            string query = string.Format(@"UPDATE Bag_Item
						SET Bag_Item.Amount = Bag_Item.Amount - {0}
						FROM Bag_Item
						INNER JOIN Bag ON Bag_Item.Bag = Bag.ID
						INNER JOIN Item ON Item.ItemID = Bag_Item.Item
						WHERE Item.ItemName ='{2}' AND Bag.BagName = '{1}'", amountRemoved, bagName, itemName);
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                Connect.Open();
                SqlCommand RemoveFromBag = new SqlCommand(query, Connect);

                RemoveFromBag.ExecuteNonQuery();

                Connect.Close();
            }
        }

        public void DeleteItem(int itemID)
        {
            string query = string.Format("DELETE FROM ITEM WHERE ItemID={0}", itemID);
            using (SqlConnection Connect = new SqlConnection(connectionString))
            {
                Connect.Open();
                SqlCommand Delete = new SqlCommand(query, Connect);
                Delete.ExecuteNonQuery();
                Connect.Close();
            }
        }
    }
}

