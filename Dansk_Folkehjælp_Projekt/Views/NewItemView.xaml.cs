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
            BindingExpression bindingItem = ItemNameBox.GetBindingExpression(TextBox.TextProperty);
            bindingItem.UpdateSource();
            BindingExpression bindingAmount = ItemAmountbox.GetBindingExpression(TextBox.TextProperty);
            bindingAmount.UpdateSource();
            BindingExpression bindingMinAmount = MinAmountBox.GetBindingExpression(TextBox.TextProperty);
            bindingMinAmount.UpdateSource();
            BindingExpression bindingBoxID = BoxID_Box.GetBindingExpression(TextBox.TextProperty);
            bindingBoxID.UpdateSource();
            BindingExpression bindingBookcaseName = BookcaseName_box.GetBindingExpression(TextBox.TextProperty);
            bindingBookcaseName.UpdateSource();
            BindingExpression bindingLocation = Storage_Dropdown.GetBindingExpression(ComboBoxItem.IsSelectedProperty);
            bindingLocation.UpdateSource();

            MainView.NewItem();

            MessageBox.Show("Genstand tilføjet");
        }
    }
}
