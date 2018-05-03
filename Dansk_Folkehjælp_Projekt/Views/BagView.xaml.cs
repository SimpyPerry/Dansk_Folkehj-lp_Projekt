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
    /// Interaction logic for BagView.xaml
    /// </summary>
    public partial class BagView : Window
    {
        private MainViewModel MainView;
        public BagView()
        {
            MainView = new MainViewModel();
            DataContext = MainView;

            InitializeComponent();
        }

        private void Caretaker_Button_Click(object sender, RoutedEventArgs e)
        {
            MainView._BagID = 2;
            MainView.ChooseBagType();
        }
    }
}
