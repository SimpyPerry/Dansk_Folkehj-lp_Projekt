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
        public MainWindow()
        {
            InitializeComponent();
            MainView = new MainViewModel();
            DataContext = MainView;
        }

        private void bt_Find_Click(object sender, RoutedEventArgs e)
        {
            find = new FindView();
            find.ShowDialog();
        }
    }
}
