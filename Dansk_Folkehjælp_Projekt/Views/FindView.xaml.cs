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
            BindingExpression binding = tb_Find.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
            MainView.UpdateList();

            BindingExpression name = Name.GetBindingExpression(TextBlock.TextProperty);
            BindingExpression minimum = Minimum.GetBindingExpression(TextBlock.TextProperty);
            BindingExpression amount = Amount.GetBindingExpression(TextBlock.TextProperty);
            BindingExpression box = Box.GetBindingExpression(TextBlock.TextProperty);
            BindingExpression bookCase = Bookcase.GetBindingExpression(TextBlock.TextProperty);
            BindingExpression location = Location.GetBindingExpression(TextBlock.TextProperty);

            name.UpdateTarget();
            minimum.UpdateTarget();
            amount.UpdateTarget();
            box.UpdateTarget();
            bookCase.UpdateTarget();
            location.UpdateTarget();


            //Kode til watermark
            if (tb_Find.Text == "")
            {
                // Create an ImageBrush.
                ImageBrush textImageBrush = new ImageBrush();
                textImageBrush.ImageSource =
                    new BitmapImage(
                        new Uri("https://d43rqu5kpufo0.cloudfront.net/images/placeholder/440x440.png", UriKind.Relative)
                    );
                textImageBrush.AlignmentX = AlignmentX.Left;
                textImageBrush.Stretch = Stretch.None;
                // Use the brush to paint the button's background.
                tb_Find.Background = textImageBrush;

            }
            else
            {

                tb_Find.Background = null;
            }
        }
    }
}
