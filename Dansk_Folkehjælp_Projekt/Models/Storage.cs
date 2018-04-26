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
        public int itemID
        {
            get { return _itemID; }
            set
            {
                _itemID = value;
                OnPropertyChanged("itemID");
            }
        }
        //Storage
        private string _location;
        public string location {
            get { return _location; }
            set
            {

                {
                    _location = value;
                    OnPropertyChanged("location");
                    
                }
            }
        }

        //Bookcase
        private string _bookcaseName;
        public string bookcaseName {
            get { return _bookcaseName; }
            set
            {

                {
                    _bookcaseName = value;
                    OnPropertyChanged("bookcaseName");

                }
            }
        }

        //Box
        private string _boxID;
        public string boxID {
            get { return _boxID; }
            set
            {

                {
                    _boxID = value;
                    OnPropertyChanged("boxID");

                }
            }
        }

        //Item
        private string _itemName;
        public string itemName {
            get { return _itemName; }
            set
            {

                {
                    _itemName = value;
                    OnPropertyChanged("itemName");

                }
            }
        }
        private int _amount;
        public int amount {
            get { return _amount; }
            set
            {

                {
                    _amount = value;
                    OnPropertyChanged("amount");

                }
            }
        }
        private int _minAmount;
        public int minAmount {
            get { return _minAmount; }
            set
            {

                {
                    _minAmount = value;
                    OnPropertyChanged("minAmount");

                }
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
