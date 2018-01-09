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

namespace ActivityTracker
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow: Window
  {
    Project MainProject;

    public MainWindow ()
    {
      InitializeComponent ();

      MainProject = new Project ();
    }


﻿//========================================================================================
// Event handlers

    private void MenuRegisterClick (object sender, RoutedEventArgs e)
    {
      // Launch the registration window.
    }


    private void MenuLoginClick (object sender, RoutedEventArgs e)
    {
      // Launch the login Window
    }


    private void MenuLogoutClick (object sender, RoutedEventArgs e)
    {
      // Log out.
    }

    private void TextBox_TextChanged (object sender, TextChangedEventArgs e)
    {

    }
  }
}
