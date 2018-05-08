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
            BindingExpression amountOfItemAdded = AmountAddedToBag_Box.GetBindingExpression(TextBox.TextProperty);
            amountOfItemAdded.UpdateSource();
            mainView.AddMoreOfItemToBag();
            MessageBox.Show("Gjort");
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {

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
