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
    private Project MainProject;


    // Constructor.
    public MainWindow ()
    {
      InitializeComponent ();

      MainProject = new Project ();
      MainProject.LoadDatabase ("ActivityDatabase.sqlite");
    }


    // Loads the window for user after logging in.
    private void LoginSetup ()
    {
      ShowInstanceView ();
    }


    // Show the activities view.
    private void ShowActivityView ()
    {
      ActivityView.Visibility = Visibility.Visible;
      FindActivity.Visibility = Visibility.Visible;

      InstanceView.Visibility = Visibility.Hidden;
      SelectUser.Visibility = Visibility.Hidden;

      SessionView.Visibility = Visibility.Hidden;
      NewSession.Visibility = Visibility.Hidden;

      ViewTitle.Content = "Activity List";
    }


    // Show the instances view.
    private void ShowInstanceView ()
    {
      ActivityView.Visibility = Visibility.Hidden;
      FindActivity.Visibility = Visibility.Hidden;

      InstanceView.Visibility = Visibility.Visible;
      SelectUser.Visibility = Visibility.Visible;

      SessionView.Visibility = Visibility.Hidden;
      NewSession.Visibility = Visibility.Hidden;

      ViewTitle.Content = "User Activities";
      InstancesText.Content = "Activities for " + MainProject.SelectedUserName;
    }


    // Show the session view.
    private void ShowSessionView ()
    {
      ActivityView.Visibility = Visibility.Hidden;
      FindActivity.Visibility = Visibility.Hidden;

      InstanceView.Visibility = Visibility.Hidden;
      SelectUser.Visibility = Visibility.Hidden;

      SessionView.Visibility = Visibility.Visible;
      NewSession.Visibility = Visibility.Visible;

      ViewTitle.Content = "Session Log";
      SessionsText.Content = MainProject.SelectedActivityName + " - " + 
                             MainProject.SelectedUserName +
                             "'s sessions";
    }

﻿//========================================================================================
// Event handlers

    private void MenuRegisterClick (object sender, RoutedEventArgs e)
    {
      // Launch the registration window.
      var register = new RegisterWindow ();
      register.MyProject = MainProject;
      bool? success = register.ShowDialog ();
      if (success is bool s)
        if (s)
          LoginSetup ();
    }


    private void MenuLoginClick (object sender, RoutedEventArgs e)
    {
      // Launch the login Window
      var login = new LoginWindow ();
      login.MyProject = MainProject;
      login.ShowDialog ();
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
