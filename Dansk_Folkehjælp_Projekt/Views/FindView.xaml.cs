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
        private MainViewModel mainView;
        public FindView()
        {
            mainView = new MainViewModel();
           
            
            DataContext = mainView;
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression binding = tb_Find.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
            mainView.UpdateList();


        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression[] bindings = new BindingExpression[6];
            TextBox[] boxes = new TextBox[] { Name, Minimum, Amount, Box, Bookcase, Location };

            
            for(int i=0;i<6;i++)
            {
                bindings[i] = boxes[i].GetBindingExpression(TextBox.TextProperty);
                bindings[i].UpdateSource();
            }

            mainView.EditData();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            mainView.DeleteItem();
        }
    }
}
