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
    /// Interaction logic for NewItemView.xaml
    /// </summary>
    public partial class NewItemView : Window
    {
        private MainViewModel MainView;
        public NewItemView()
        {
            MainView = new MainViewModel();
            DataContext = MainView;
            InitializeComponent();
        }

        private void Save_button_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression[] bindings = new BindingExpression[5];
            TextBox[] box = new TextBox[] { ItemNameBox, ItemAmountbox, MinAmountBox, LocationBox, BoxID_Box, };

            if (LocationBox.Text == "Container") {
                
                for (int i = 0; i < 4; i++)
                {
                    bindings[i] = box[i].GetBindingExpression(TextBox.TextProperty);
                    bindings[i].UpdateSource();
                    box[i].Text = null;
                }
                MainView.NewItem();
                MessageBox.Show("Genstand tilføjet til containeren");
               

            } else if( LocationBox.Text == "Loft")
            {
                
                for (int i =0; i<5;i++)
                {
                    bindings[i] = box[i].GetBindingExpression(TextBox.TextProperty);
                    bindings[i].UpdateSource();
                    box[i].Text = null;
                }
                MainView.NewItem();
                MessageBox.Show("Genstand tilføjet til loftet");
               
            } else
            {

                MessageBox.Show("Lokation er ugyldig");
            }
        }
    }
}
