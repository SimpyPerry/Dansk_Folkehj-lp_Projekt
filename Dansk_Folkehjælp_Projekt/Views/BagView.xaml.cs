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
    /// Interaction logic for BagView.xaml
    /// </summary>
    public partial class BagView : Window
    {
        private MainViewModel MainView;
        private BagItemView bagItemView;
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

        private void Firsthelp_button_Click(object sender, RoutedEventArgs e)
        {
            MainView._BagID = 1;
            MainView.ChooseBagType();
        }

        private void BagChoosen(object sender, SelectionChangedEventArgs e)
        {
            MainView.ChooseSpecificBag();
        }

        

        private void bt_SaveName_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression bindingExpression = BagName.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
            MainView.ChangeBagName();
        }

        private void bt_Delete_Click(object sender, RoutedEventArgs e)
        {
            MainView.DeleteBag();
        }

        private void EditAmount_Click(object sender, RoutedEventArgs e)
        {
            MainView.SelectItem();
            bagItemView = new BagItemView(MainView);

            bagItemView.ShowDialog();
        }
    }
}
