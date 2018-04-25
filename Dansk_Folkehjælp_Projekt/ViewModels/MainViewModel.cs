using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dansk_Folkehjælp_Projekt.Models;

namespace Dansk_Folkehjælp_Projekt.ViewModels
{
    public class MainViewModel
    {
        Models.DatabaseConnection DatabaseConnection;
        public List<Storage> ItemList { get; set; }
        public List<Storage> Tr;
        public Storage Current { get; set; }
        public string FindViewTextBox { get; set; } = "Indsæt søgeord";

        public Storage Test { get; set; }
        public MainViewModel()
        {
            DatabaseConnection = new Models.DatabaseConnection();
            DatabaseConnection.FindByItemName("Bandage");
            ItemList = DatabaseConnection.GetStorages;
            Current = ItemList[0];
        }
        public string _itemName;
        public int _amount;
        public int _minAmount;
        public string _boxID;
        public string _bookcaseName;
        public string _location;
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

        }
        public void UpdateList()
        {
            



            DatabaseConnection.FindByItemName(FindViewTextBox);

            ItemList = DatabaseConnection.GetStorages;
            
           
            Current = DatabaseConnection.GetStorages[0];
        }


    }
}

