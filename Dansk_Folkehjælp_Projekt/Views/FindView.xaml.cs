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
    /// Interaction logic for FindView.xaml
    /// </summary>
    public partial class FindView : Window
    {
        private MainViewModel MainView;
        public FindView()
        {
            MainView = new MainViewModel();
           
            
            DataContext = MainView;
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
