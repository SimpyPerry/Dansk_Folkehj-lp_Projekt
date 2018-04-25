﻿using System;
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


        }
    }
}
