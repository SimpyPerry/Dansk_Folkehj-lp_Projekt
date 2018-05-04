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
        public ObservableCollection<Storage> bagcollcetion { get; set; }
        public List<string> bookcaseCombo { get; set; }
        public List<string> NotificationMail;
        public ObservableCollection<Storage> itemCollection { get; set; }
        public ObservableCollection<Storage> bagItemInfo { get; set; }
        public Storage specificBagItem { get; set; }
        public Storage Current { get; set; }
        public string FindViewTextBox { get; set; } = "Indsæt søgeord";
        public Storage selectedBag { get; set; }
      
        public MainViewModel()
        {
            DatabaseConnection = new Models.DatabaseConnection();
            DatabaseConnection.FindByItemName("Bandage");
            collection = DatabaseConnection.GetStorages;
            Current = collection[0];
            DatabaseConnection.ShowBookcases();
            bagcollcetion = DatabaseConnection.GetBags;
            bookcaseCombo = DatabaseConnection.bookcases;
            selectedBag = bagcollcetion[0];
            DatabaseConnection.GetBagItems(selectedBag.itemID);
            itemCollection = DatabaseConnection.GetItems;
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

        public void ReduceAmount()
        {

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
            DatabaseConnection.EditItem(Current.itemID, Current.itemName, Current.amount, Current.minAmount, Current.boxID, Current.bookcaseName, Current.location);
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
            int p = bagcollcetion.Count;
            for(int i=0; i<p; i++)
            {
                bagcollcetion.RemoveAt(0);
            }

            int u = DatabaseConnection.GetBags.Count;
            for(int i=0; i<u; i++)
            {
                bagcollcetion.Add(DatabaseConnection.GetBags[i]);
            }
            
            bagcollcetion = DatabaseConnection.GetBags;
        }
        public void ChooseSpecificBag()
        {
            DatabaseConnection.GetBagItems(selectedBag.itemID);
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
        public void ChooseBagItem()
        {

        }

    }
}

