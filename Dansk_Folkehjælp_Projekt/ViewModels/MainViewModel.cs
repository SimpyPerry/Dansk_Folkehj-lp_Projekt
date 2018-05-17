using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Dansk_Folkehjælp_Projekt.Models;

namespace Dansk_Folkehjælp_Projekt.ViewModels
{
    public class MainViewModel
    {
        Models.DatabaseConnection DatabaseConnection;
        public List<Storage> ItemList { get; set; }
        public ObservableCollection<Storage> Collection { get; set; }
        public ObservableCollection<Storage> Bagcollection { get; set; }
        public List<string> BookcaseCombo { get; set; }
        public List<string> NotificationMail;
        public ObservableCollection<Storage> ItemCollection { get; set; }
        public ObservableCollection<Storage> BagItemInfo { get; set; }
        public Storage SpecificBagItem { get; set; }
        public Storage Current { get; set; }
        public string FindViewTextBox { get; set; } = "Indsæt søgeord";
        public Storage SelectedBag { get; set; }
        public ObservableCollection<Storage> BagType { get; set; }
        public ObservableCollection<Storage> GetItemsInType { get; set; }
        public Storage SelectedBagType { get; set; }
        public Storage SelectedItemForType { get; set; }
        public Storage SelectedItem { get; set; }
        public Storage SelectedItemFromBag { get; set; }
        public ObservableCollection<Storage> ItemFromBag { get; set; }
      
        public MainViewModel()
        {
            DatabaseConnection = new Models.DatabaseConnection();
            
           
            
            DatabaseConnection.ShowBookcases();
            Bagcollection = DatabaseConnection.GetBags;
            BookcaseCombo = DatabaseConnection.Bookcases;
            SelectedBag = Bagcollection[0];
            DatabaseConnection.GetBagItems(SelectedBag.ItemID);
            ItemCollection = DatabaseConnection.GetItems;
            BagItemInfo = DatabaseConnection.ItemFromDatabase;
            Collection = DatabaseConnection.ItemFromDatabase;
            Current = Collection[0];
            DatabaseConnection.InitBagTypes();
            BagType = DatabaseConnection.BagTypes;
            SelectedBagType = BagType[0];
            DatabaseConnection.GetItemRequirementsForTypes(BagType[0].ItemID);
            GetItemsInType = DatabaseConnection.BagTypeRequirements;
            SelectedItemForType = GetItemsInType[0];
            SelectedItem = BagItemInfo[0];
            ItemFromBag = DatabaseConnection.ChosenItemFromBag;
            
            

        }
        public string _itemName { get; set; }
        public int _amount { get; set; }
        public int _minAmount { get; set; }
        public string _boxID { get; set; }
        public string _bookcaseName { get; set; }
        public string _location { get; set; }
        public int _BagID { get; set; }
       
        public void FindItem()
        {
            DatabaseConnection.FindByItemName(_itemName);
        }
        public void AddNewRequirement(int amount)
        {
            DatabaseConnection.AddItemToRequirements(SelectedBagType.ItemID, SelectedItem.ItemID, amount);
            DatabaseConnection.UpdateBagsAfterRequirementsChanged(SelectedBagType.ItemID);
            ChangeBagTypeRequirements();
        }

        public void ReduceAmount()
        {

        }
        public void ChangeMinimumForRequirements (int minimum)
        {
            
            DatabaseConnection.EditMinimumForType(SelectedBagType.ItemID, SelectedItemForType.ItemID, minimum);
            ChangeBagTypeRequirements();  

        }

        public void DeleteRequirement()
        {
            DatabaseConnection.DeleteFromTypeRequirements(SelectedBagType.ItemID, SelectedItemForType.ItemID);
            ChangeBagTypeRequirements();
        }
        public void ChangeBagTypeRequirements()
        {
            DatabaseConnection.GetItemRequirementsForTypes(SelectedBagType.ItemID);
            int a = GetItemsInType.Count;
            int b = DatabaseConnection.BagTypeRequirements.Count;

            for(int i=0;i<a;i++)
            {
                GetItemsInType.RemoveAt(0);
            }
            for(int i=0;i<b;i++)
            {
                GetItemsInType.Add(DatabaseConnection.BagTypeRequirements[i]);
            }
            SelectedItemForType = GetItemsInType[0];
        }

        public void NewItem()
        {
            DatabaseConnection.AddNewItem(_itemName,_amount,_minAmount,_boxID, _bookcaseName, _location);
        }

        public void ChooseStorage()
        {
           // DatabaseConnection.ShowStorage(_storage);
        }

        public void NewBookcase()
        {
            DatabaseConnection.AddNewBookcase(_bookcaseName);
        }
        public void UpdateList()
        {
              DatabaseConnection.FindByItemName(FindViewTextBox);

            int p = Collection.Count;
            for(int i=0; i<p; i++)
            {
                Collection.RemoveAt(0);
            }
            int u = DatabaseConnection.GetStorages.Count;
            for(int i=0; i<u;i++)
            { Collection.Add(DatabaseConnection.GetStorages[i]); }
 
            Current = Collection[0];
        }

        public void EditData()
        {
            DatabaseConnection.EditItem(Current.ItemID, Current.ItemName, Current.Amount, Current.MinAmount, Current.BoxID, Current.BookcaseName, Current.Location);
        }

        public bool CheckBookcase()
        {
            //DatabaseConnection.SeeIfExsists(_bookcaseName);
            if (DatabaseConnection.SeeIfExsists(_bookcaseName) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SendNotificationMail()
        {
            DatabaseConnection.GetMailAddresses();
            NotificationMail = DatabaseConnection.MailList;
            int mailCount = NotificationMail.Count;

            for(int i =0; i<mailCount;i++)
            {
                string currentMail = NotificationMail[i];
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.live.com", 587);
                mail.From = new MailAddress("lagernotifikation@hotmail.com");
                mail.To.Add(currentMail);
                mail.Subject = "Test ";
                mail.Body = "Ser bare om jeg kan sende";
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential("lagernotifikation@hotmail.com", "lagerDanskFolke");
                smtpClient.EnableSsl = true;
                smtpClient.Send(mail);



            }

        }
        public void ChangeBagName()
        {
            DatabaseConnection.ChangeBagName(SelectedBag.ItemID, SelectedBag.ItemName);
        }

        public void DeleteBag()
        {
            DatabaseConnection.DeleteBag(SelectedBag.ItemID);
            
        }
        public void ChooseBagType()
        {
            DatabaseConnection.SelectBag(_BagID);
            int p = Bagcollection.Count;
            for(int i=0; i<p; i++)
            {
                Bagcollection.RemoveAt(0);
            }

            int u = DatabaseConnection.GetBags.Count;
            for(int i=0; i<u; i++)
            {
                Bagcollection.Add(DatabaseConnection.GetBags[i]);
            }
            
            Bagcollection = DatabaseConnection.GetBags;
        }
        public void ChooseSpecificBag()
        {
            

            DatabaseConnection.GetBagItems(SelectedBag.ItemID);
            int p = ItemCollection.Count;

            for(int i=0; i<p;i++)
            {
                ItemCollection.RemoveAt(0);
            }

            int u = DatabaseConnection.GetItems.Count;
            for(int i =0; i<u;i++)
            {
                ItemCollection.Add(DatabaseConnection.GetItems[i]);
               
            }
        }
        public void SelectItem()
        {
            
            DatabaseConnection.GetItemFromBag(SelectedBag.ItemName, SelectedItemFromBag.ItemID);
            SelectedItemFromBag = ItemFromBag[0];
        }
        public void AddMoreOfItemToBag()
        {
            DatabaseConnection.TakeItemFromStorageToBag(SelectedBag.ItemName, SelectedItemFromBag.ItemName, _amount);
        }
        public void RemoveItemFromBag()
        {
            DatabaseConnection.RemoveItemFromBag(SelectedBag.ItemName, SelectedItemFromBag.ItemName, _amount);
        }
        
        public void AddBag()
        {
                DatabaseConnection.AddBag(_itemName, SelectedBagType.ItemID);
            _itemName = null;
        }
        public void SeekThenAlterCollection()
        {
            DatabaseConnection.FindByItemName(FindViewTextBox);
            int p = BagItemInfo.Count;
            for (int i = 0; i < p; i++)
            {
                BagItemInfo.RemoveAt(0);
            }


            int u = DatabaseConnection.GetStorages.Count;
            for (int i = 0; i < u; i++)
            { BagItemInfo.Add(DatabaseConnection.GetStorages[i]); }



            
        }

        public void DeleteItem()
        {
            DatabaseConnection.DeleteItem(Current.ItemID);
        }

        public void ShowItemsUnderMinimum()
        {
            //foreach( Storage items in Collection)
            //{
            //    if(items.MinAmount<=items.Amount)
            //    {
            //        Collection.Remove(items);
            //    }
            //}

            int a = Collection.Count;
            for(int i=0; i<a;i++)
            {
                Collection.RemoveAt(0);
            }
            DatabaseConnection.GetAllItems();
            int b = DatabaseConnection.ItemFromDatabase.Count;
            for(int i=0;i<b;i++)
            {
                if(DatabaseConnection.ItemFromDatabase[i].Amount>=DatabaseConnection.ItemFromDatabase[i].MinAmount)
                {
                    Collection.Add(DatabaseConnection.ItemFromDatabase[i]);
                }
            }
        }

        public void ChangeDependingOnBookcase()
        {
            DatabaseConnection.GetItemsFromBookcase(_bookcaseName);

            int a = Collection.Count;
            for(int i=0;i<a;i++)
            {
                Collection.RemoveAt(0);
            }

            int b = DatabaseConnection.ItemFromDatabase.Count;
            for(int i =0; i<b;i++)
            {
                Collection.Add(DatabaseConnection.ItemFromDatabase[i]);
            }

           
        }
    }
}

