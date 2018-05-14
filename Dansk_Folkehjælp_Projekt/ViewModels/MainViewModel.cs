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
        public ObservableCollection<Storage> collection { get; set; }
        public ObservableCollection<Storage> Bagcollection { get; set; }
        public List<string> bookcaseCombo { get; set; }
        public List<string> NotificationMail;
        public ObservableCollection<Storage> itemCollection { get; set; }
        public ObservableCollection<Storage> bagItemInfo { get; set; }
        public Storage specificBagItem { get; set; }
        public Storage Current { get; set; }
        public string FindViewTextBox { get; set; } = "Indsæt søgeord";
        public Storage selectedBag { get; set; }
        public ObservableCollection<Storage> BagType { get; set; }
        public ObservableCollection<Storage> GetItemsInType { get; set; }
        public Storage selectedBagType { get; set; }
        public Storage selectedItemForType { get; set; }
        public Storage selectedItem { get; set; }
        public Storage SelectedItemFromBag { get; set; }
        public ObservableCollection<Storage> HotStuff { get; set; }
      
        public MainViewModel()
        {
            DatabaseConnection = new Models.DatabaseConnection();
            
           
            
            DatabaseConnection.ShowBookcases();
            Bagcollection = DatabaseConnection.GetBags;
            bookcaseCombo = DatabaseConnection.Bookcases;
            selectedBag = Bagcollection[0];
            DatabaseConnection.GetBagItems(selectedBag.ItemID);
            itemCollection = DatabaseConnection.GetItems;
            bagItemInfo = DatabaseConnection.ItemFromBag;
            collection = DatabaseConnection.ItemFromBag;
            Current = collection[0];
            DatabaseConnection.InitBagTypes();
            BagType = DatabaseConnection.BagTypes;
            selectedBagType = BagType[0];
            DatabaseConnection.GetItemRequirementsForTypes(BagType[0].ItemID);
            GetItemsInType = DatabaseConnection.BagTypeRequirements;
            selectedItemForType = GetItemsInType[0];
            selectedItem = bagItemInfo[0];
            HotStuff = DatabaseConnection.ChosenItemFromBag;
            
            

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
            DatabaseConnection.AddItemToRequirements(selectedBagType.ItemID, selectedItem.ItemID, amount);
            DatabaseConnection.UpdateBagsAfterRequirementsChanged(selectedBagType.ItemID);
            ChangeBagTypeRequirements();
        }

        public void ReduceAmount()
        {

        }
        public void ChangeMinimumForRequirements (int minimum)
        {
            
            DatabaseConnection.EditMinimumForType(selectedBagType.ItemID, selectedItemForType.ItemID, minimum);
            ChangeBagTypeRequirements();  

        }

        public void DeleteRequirement()
        {
            DatabaseConnection.DeleteFromTypeRequirements(selectedBagType.ItemID, selectedItemForType.ItemID);
            ChangeBagTypeRequirements();
        }
        public void ChangeBagTypeRequirements()
        {
            DatabaseConnection.GetItemRequirementsForTypes(selectedBagType.ItemID);
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
            selectedItemForType = GetItemsInType[0];
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

            int p = collection.Count;
            for(int i=0; i<p; i++)
            {
                collection.RemoveAt(0);
            }

            int u = DatabaseConnection.GetStorages.Count;
            for(int i=0; i<u;i++)
            { collection.Add(DatabaseConnection.GetStorages[i]); }
            
           
           
            Current = DatabaseConnection.GetStorages[0];

            
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
            DatabaseConnection.GetBagItems(selectedBag.ItemID);
            int p = itemCollection.Count;

            for(int i=0; i<p;i++)
            {
                itemCollection.RemoveAt(0);
            }

            int u = DatabaseConnection.GetItems.Count;
            for(int i =0; i<u;i++)
            {
                itemCollection.Add(DatabaseConnection.GetItems[i]);
            }
        }
        public void SelectItem()
        {
            
            DatabaseConnection.GetItemFromBag(selectedBag.ItemName, SelectedItemFromBag.ItemID);
            SelectedItemFromBag = HotStuff[0];
        }
        public void AddMoreOfItemToBag()
        {
            DatabaseConnection.TakeItemFromStorageToBag(selectedBag.ItemName, SelectedItemFromBag.ItemName, _amount);
        }
        public void RemoveItemFromBag()
        {
            DatabaseConnection.RemoveItemFromBag(selectedBag.ItemName, SelectedItemFromBag.ItemName, _amount);
        }
        
        public void AddBag()
        {
                DatabaseConnection.AddBag(_itemName, selectedBagType.ItemID);
        }
        public void SeekThenAlterCollection()
        {
            DatabaseConnection.FindByItemName(FindViewTextBox);
            int p = bagItemInfo.Count;
            for (int i = 0; i < p; i++)
            {
                bagItemInfo.RemoveAt(0);
            }

            int u = DatabaseConnection.GetStorages.Count;
            for (int i = 0; i < u; i++)
            { bagItemInfo.Add(DatabaseConnection.GetStorages[i]); }



            
        }

    }
}

