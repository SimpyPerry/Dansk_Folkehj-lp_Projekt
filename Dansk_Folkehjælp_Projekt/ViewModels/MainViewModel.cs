﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Storage> collection { get; set; }

        public List<string> bookcaseCombo { get; set; }


        public Storage Current { get; set; }
        public string FindViewTextBox { get; set; } = "Indsæt søgeord";

        public Storage Test { get; set; }
        public MainViewModel()
        {
            DatabaseConnection = new Models.DatabaseConnection();
            DatabaseConnection.FindByItemName("Bandage");
            collection = DatabaseConnection.GetStorages;
            Current = collection[0];

            bookcaseCombo = new List<string> { "a", "b" };
        }
        public string _itemName { get; set; }
        public int _amount { get; set; }
        public int _minAmount { get; set; }
        public string _boxID { get; set; }
        public string _bookcaseName { get; set; }
        public string _location { get; set; }
       
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


    }
}

