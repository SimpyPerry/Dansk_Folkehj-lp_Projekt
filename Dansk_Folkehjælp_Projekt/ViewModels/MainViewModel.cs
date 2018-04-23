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

        public string _itemName;

        public MainViewModel()
        {
            DatabaseConnection = new DatabaseConnection();
            Tr = new List<Storage>()
         {
             new Storage{itemName="gr", amount=4}
         };
            DatabaseConnection.FindByItemName("Klud");
            ItemList = DatabaseConnection.GetStorages;
            
           
        }
        public void FindItem()
        {
            DatabaseConnection.FindByItemName(_itemName);
        }

        public void ReduceAmount()
        {

        }

        public void NewItem()
        {

        }

        public void ChooseStoreage()
        {

        }

        public void NewBookcase()
        {

        }


    }
}

