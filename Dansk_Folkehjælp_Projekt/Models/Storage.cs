using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dansk_Folkehjælp_Projekt.Models
{
    public class Storage: INotifyPropertyChanged
    {
        //ItemID
        private int _itemID;
        public int ItemID
        {
            get { return _itemID; }
            set
            {
                _itemID = value;
                OnPropertyChanged("ItemID");
            }
        }
        //Storage
        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged("Location");
            }
        }
        private string _type;
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        //Bookcase
        private string _bookcaseName;
        public string BookcaseName
        {
            get { return _bookcaseName; }
            set
            {
                _bookcaseName = value;
                OnPropertyChanged("BookcaseName");
            }
        }

        //Box
        private string _boxID;
        public string BoxID
        {
            get { return _boxID; }
            set
            {
                _boxID = value;
                OnPropertyChanged("BoxID");
            }
        }

        //Item
        private string _itemName;
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                _itemName = value;
                OnPropertyChanged("ItemName");
            }
        }
        private string _showIfEnoughItems;
     
        public string ShowIfEnoughItems
        {
            get { return _showIfEnoughItems; }
            set
            {
                _showIfEnoughItems = value;
                OnPropertyChanged("ShowIfEnoughItems");
            }
            
        }
        private int _amount;
        public int Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
            }
        }
        private int _minAmount;
        public int MinAmount
        {
            get { return _minAmount; }
            set
            {
                _minAmount = value;
                OnPropertyChanged("MinAmount");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
