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
    /// Interaction logic for ContainerView.xaml
    /// </summary>
    public partial class ContainerView : Window
    {
        private MainViewModel mainView;
        public ContainerView()
        {
            mainView = new MainViewModel();
            DataContext = mainView;
            InitializeComponent();
        }

        private void Container_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
