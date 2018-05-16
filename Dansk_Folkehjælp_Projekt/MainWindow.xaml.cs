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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dansk_Folkehjælp_Projekt.ViewModels;
using Dansk_Folkehjælp_Projekt.Views;

namespace Dansk_Folkehjælp_Projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel MainView ;
        private FindView find;
        private NewItemView newItem;
        private NewBookcaseView newCase;
        private BagView bagView;
        private EditBagType EditBag;
        private AddBag addBag;
        private AtticView attic;

        public MainWindow()
        {
           
            MainView = new MainViewModel();
            DataContext = MainView;
            InitializeComponent();
        }

        private void bt_Find_Click(object sender, RoutedEventArgs e)
        {
            find = new FindView();
            find.ShowDialog();
        }

        private void bt_AddInstructor_Click(object sender, RoutedEventArgs e)
        {
            newItem = new NewItemView();
            newItem.ShowDialog();
        }

        private void bt_Attic_Click(object sender, RoutedEventArgs e)
        {
            attic = new AtticView();
            attic.ShowDialog();
        }

        private void bt_Container_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bt_AllInstructors_Click(object sender, RoutedEventArgs e)
        {
            newCase = new NewBookcaseView();
            newCase.ShowDialog();
        }

        private void Bag_Button_Click(object sender, RoutedEventArgs e)
        {
            bagView = new BagView();
            bagView.ShowDialog();
        }

        private void EditBagType_Click(object sender, RoutedEventArgs e)
        {
            EditBag = new EditBagType();
            EditBag.ShowDialog();
        }

        private void bt_AddBag(object sender, RoutedEventArgs e)
        {
            addBag = new AddBag();
            addBag.ShowDialog();
        }
    }
}
