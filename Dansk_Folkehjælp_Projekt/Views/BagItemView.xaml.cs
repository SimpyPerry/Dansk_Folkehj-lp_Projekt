using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Dansk_Folkehjælp_Projekt.ViewModels;

namespace Dansk_Folkehjælp_Projekt.Views
{
    /// <summary>
    /// Interaction logic for BagItemView.xaml
    /// </summary>
    public partial class BagItemView : Window
    {
        MainViewModel mainView;
        public BagItemView(MainViewModel main)
        {

            mainView = main;
            DataContext = mainView;
            InitializeComponent();
           
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int amountToBag = Int32.Parse(AmountAddedToBag_Box.Text);
            int amountStorage = Int32.Parse(StorageAmount_box.Text);
            if (amountToBag > amountStorage)
            {
                MessageBox.Show("Du kan ikke tilføje mere af genstanden end der er på lager");
                AmountAddedToBag_Box.Text = AmountAddedToBag_Box.Text.Remove(AmountAddedToBag_Box.Text.Length - 1);
            }
            else
            {
                BindingExpression amountOfItemAdded = AmountAddedToBag_Box.GetBindingExpression(TextBox.TextProperty);
                amountOfItemAdded.UpdateSource();

                mainView.AddMoreOfItemToBag();
                MessageBox.Show("Gjort");
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            int amountInBag = Int32.Parse(ItemAmountInBag_box.Text);
            int amountRemoveFromBag = Int32.Parse(AmountAddedToBag_Box.Text);
            if (amountInBag < amountRemoveFromBag)
            {
                MessageBox.Show("Du kan ikke fjerne mere end hvad der er i tasken");
                AmountAddedToBag_Box.Text = AmountAddedToBag_Box.Text.Remove(AmountAddedToBag_Box.Text.Length - 1);
            }
            else
            {


                BindingExpression amountRemovedFromBag = AmountAddedToBag_Box.GetBindingExpression(TextBox.TemplateProperty);
                amountRemovedFromBag.UpdateSource();
                mainView.RemoveItemFromBag();
                MessageBox.Show("Også gjort");
            }
        }

        private void AmountAddedToBag_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(System.Text.RegularExpressions.Regex.IsMatch(AmountAddedToBag_Box.Text, "[^0-9]"))
            {
                MessageBox.Show("Kun Tal tak");
                AmountAddedToBag_Box.Text = AmountAddedToBag_Box.Text.Remove(AmountAddedToBag_Box.Text.Length - 1);
            }
            
        }
    }
}
