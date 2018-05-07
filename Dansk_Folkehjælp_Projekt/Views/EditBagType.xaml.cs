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
    /// Interaction logic for EditBagType.xaml
    /// </summary>
    public partial class EditBagType : Window
    {
        private MainViewModel main;
        public EditBagType()
        {
            main = new MainViewModel();
            DataContext = main;
            InitializeComponent();
        }

       

        private void TypeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            main.ChangeBagTypeRequirements();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            
                int m = Int32.Parse(Amount.Text);
            main.AddNewRequirement(m);
            
        }

        private void ChangeMinimum_Click(object sender, RoutedEventArgs e)
        {
            int a = Int32.Parse(ItemMinimum.Text);
            main.ChangeMinimumForRequirements(a);
        }
    }
}
