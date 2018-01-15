﻿//========================================================================================
// ActivityTracker by Daan Juttmann
// Created: 2017-12-19
// License: GNU General Public License 3.0 (https://www.gnu.org/licenses/gpl-3.0.en.html).
//========================================================================================

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
  public enum View
  {
    None,
    Activities,
    Instances,
    Sessions
  }


  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow: Window
  {
    private Project MainProject;
    private List <ISelectableUI> ActivityItems;
    private List <ISelectableUI> InstanceItems;
    private List <ISelectableUI> SessionItems;
    private View ActiveView;

    private readonly SolidColorBrush InvalidInput = 
      new SolidColorBrush (Color.FromArgb (0xFF, 0xFF, 0xA0, 0xA0));
    private readonly SolidColorBrush ValidInput = 
      new SolidColorBrush (Color.FromArgb (0xFF, 0xFF, 0xFF, 0xFF));

    // Constructor.
    public MainWindow ()
    {
      InitializeComponent ();

      MainProject = new Project ();
      MainProject.LoadDatabase ("ActivityDatabase.sqlite");
      ActivityItems = new List <ISelectableUI> ();
      InstanceItems = new List <ISelectableUI> ();
      SessionItems = new List <ISelectableUI> ();
      ShowNoView ();

      // [wip] Login admin -- for testing purposes, remove later.
      MainProject.LoginUser ("Admin", "12345678");
      LoginSetup ();
    }


    // Loads the window for user after logging in.
    private void LoginSetup ()
    {
      ShowInstanceView ();
    }

﻿//========================================================================================
// Manage visibility of UI elements


    // Show no view.
    private void ShowNoView ()
    {
      ActivityView.Visibility = Visibility.Hidden;
      FindActivity.Visibility = Visibility.Hidden;
      NewActivity.Visibility = Visibility.Hidden;
      StartButton.Visibility = Visibility.Visible;

      InstanceView.Visibility = Visibility.Hidden;
      SelectUser.Visibility = Visibility.Hidden;

      SessionView.Visibility = Visibility.Hidden;
      NewSession.Visibility = Visibility.Hidden;

      ViewTitle.Content = "";
      ActiveView = View.None;
    }


    // Show the activities view.
    private void ShowActivityView ()
    {
      ActivityView.Visibility = Visibility.Visible;
      FindActivity.Visibility = Visibility.Visible;
      NewActivity.Visibility = Visibility.Hidden;
      EditActivity.Visibility = Visibility.Hidden;
      StartButton.Visibility = Visibility.Visible;

      InstanceView.Visibility = Visibility.Hidden;
      SelectUser.Visibility = Visibility.Hidden;

      SessionView.Visibility = Visibility.Hidden;
      NewSession.Visibility = Visibility.Hidden;
      EditSession.Visibility = Visibility.Hidden;

      ViewTitle.Content = "Activity List";
      LoadActivityList ();
      ShowActivityList ();
      ActiveView = View.Activities;
    }


    // Show the list of availabe activities.
    private void ShowActivityList ()
    {
      ActivityList.Children.Clear ();
      for (int i = 0; i < ActivityItems.Count; i++)
        ActivityList.Children.Add (ActivityItems [i].Element);
    }


    // Show the instances view.
    private void ShowInstanceView ()
    {
      ActivityView.Visibility = Visibility.Hidden;
      FindActivity.Visibility = Visibility.Hidden;
      NewActivity.Visibility = Visibility.Hidden;
      EditActivity.Visibility = Visibility.Hidden;
      StartButton.Visibility = Visibility.Hidden;

      InstanceView.Visibility = Visibility.Visible;
      SelectUser.Visibility = Visibility.Visible;

      SessionView.Visibility = Visibility.Hidden;
      NewSession.Visibility = Visibility.Hidden;
      EditSession.Visibility = Visibility.Hidden;

      ViewTitle.Content = "User Data";
      InstancesText.Content = "Activities for " + MainProject.SelectedUserName;
      LoadInstanceList ();
      ShowInstanceList ();
      ActiveView = View.Instances;
    }


    // Show the list of activity instances for the selected user.
    private void ShowInstanceList ()
    {
      InstanceList.Children.Clear ();
      for (int i = 0; i < InstanceItems.Count; i++)
        InstanceList.Children.Add (InstanceItems [i].Element);
    }


    // Show the session view.
    private void ShowSessionView ()
    {
      ActivityView.Visibility = Visibility.Hidden;
      FindActivity.Visibility = Visibility.Hidden;
      NewActivity.Visibility = Visibility.Hidden;
      EditActivity.Visibility = Visibility.Hidden;
      StartButton.Visibility = Visibility.Hidden;

      InstanceView.Visibility = Visibility.Hidden;
      SelectUser.Visibility = Visibility.Hidden;

      SessionView.Visibility = Visibility.Visible;
      NewSession.Visibility = Visibility.Hidden;
      EditSession.Visibility = Visibility.Hidden;

      ViewTitle.Content = "Session Log";
      SessionsText.Content = MainProject.SelectedActivityName + " - " + 
                             MainProject.SelectedUserName +
                             "'s sessions";
      LoadSessionList ();
      ShowSessionList ();
      ActiveView = View.Sessions;
    }


    // Show the list of sessions for the selected instance.
    private void ShowSessionList ()
    {
      SessionList.Children.Clear ();
      for (int i = 0; i < SessionItems.Count; i++)
        SessionList.Children.Add (SessionItems [i].Element);
    }


    private void ShowNewActivity ()
    {
      if (ActiveView == View.Activities)
      {
        FindActivity.Visibility = Visibility.Hidden;
        NewActivity.Visibility = Visibility.Visible;
        EditActivity.Visibility = Visibility.Hidden;

        NewActivityName.Text = "";
        NewActivityDescription.Text = "";
        NewActivityTagInput.Text = "";
        NewActivityTags.Items.Clear ();
        ClearNewActivityValidation ();
      }
    }


    private void HideNewActivity ()
    {
      if (ActiveView == View.Activities)
      {
        FindActivity.Visibility = Visibility.Visible;
        NewActivity.Visibility = Visibility.Hidden;
      }
    }


    private void ShowEditActivity ()
    {
      if (ActiveView == View.Activities)
      {
        FindActivity.Visibility = Visibility.Hidden;
        NewActivity.Visibility = Visibility.Hidden;
        EditActivity.Visibility = Visibility.Visible;

        EditActivityName.Text = MainProject.SelectedActivityName;
        EditActivityDescription.Text = MainProject.SelectedActivityDescription;
        EditActivityTagInput.Text = "";
        EditActivityTags.Items.Clear ();
        foreach (string s in MainProject.SelectedActivityTags)
          EditActivityTags.Items.Add (s);
        ClearEditActivityValidation ();
      }
    }


    private void HideEditActivity ()
    {
      if (ActiveView == View.Activities)
      {
        FindActivity.Visibility = Visibility.Visible;
        EditActivity.Visibility = Visibility.Hidden;
      }
    }


    private void ShowNewSession ()
    {
      if (ActiveView == View.Sessions)
      {
        NewSession.Visibility = Visibility.Visible;
        EditSession.Visibility = Visibility.Hidden;

        NewSessionYear.Text = "";
        NewSessionMonth.Text = "";
        NewSessionDay.Text = "";
        NewSessionTimeSpent.Text = "";
        NewSessionPercentFinished.Text = "";
        ClearNewSessionValidation ();
      }
    }


    private void HideNewSession ()
    {
      if (ActiveView == View.Sessions)
      {
        NewSession.Visibility = Visibility.Hidden;
      }
    }


    private void ShowEditSession ()
    {
      if (ActiveView == View.Sessions)
      {
        NewSession.Visibility = Visibility.Hidden;
        EditSession.Visibility = Visibility.Visible;

        EditSessionYear.Text = MainProject.SelectedSessionDate.Year.ToString ();
        EditSessionMonth.Text = MainProject.SelectedSessionDate.Month.ToString ();
        EditSessionDay.Text = MainProject.SelectedSessionDate.Day.ToString ();
        EditSessionTimeSpent.Text = MainProject.SelectedSessionTimeSpent;
        EditSessionPercentFinished.Text = MainProject.SelectedSessionPercentFinished;
        ClearEditSessionValidation ();
      }
    }


    private void HideEditSession ()
    {
      if (ActiveView == View.Sessions)
      {
        EditSession.Visibility = Visibility.Hidden;
      }
    }


﻿//========================================================================================
// General Functionality

// Activity view


    // Load the list of activities.
    private void LoadActivityList ()
    {
      ActivityItems.Clear ();
      int i = 0;
      Activity activity = MainProject.GetActivity (i);
      while (activity != null)
      {
        var item = new UIActivityItem (this, activity.ID, activity.Name,
                                       "Unknown", activity.Description);
        ActivityItems.Add (item);
        i++;
        activity = MainProject.GetActivity (i);
      }
    }


    // Reloads the activities in the list.
    private void UpdateActivityList ()
    {
      for (int i = 0; i < ActivityItems.Count; i++)
      {
        Activity activity = MainProject.GetActivity (i);
        if (activity == null)
          break;
        ((UIActivityItem) ActivityItems [i]).Title = activity.Name;
        ((UIActivityItem) ActivityItems [i]).Creator = "Unknown";
        ((UIActivityItem) ActivityItems [i]).Description = activity.Description;
      }
    }


    // Select none of the activities.
    public void ActivitiesSelectNone ()
    {
      foreach (UIActivityItem activity in ActivityItems)
        activity.Selected = false;
    }


    // Remove new activity validation colours.
    public void ClearNewActivityValidation ()
    {
      NewActivityName.Background = ValidInput;
    }


    // Validate new activity input.
    public bool ValidateNewActivity ()
    {
      ClearNewActivityValidation ();
      bool success = true;
      if (!Validation.IsName (NewActivityName.Text))
      {
        NewActivityName.Background = InvalidInput;
        success = false;
      }
      return success;
    }


    // Remove edit activity validation colours.
    public void ClearEditActivityValidation ()
    {
      EditActivityName.Background = ValidInput;
    }


    // Validate edit activity input.
    public bool ValidateEditActivity ()
    {
      ClearEditActivityValidation ();
      bool success = true;
      if (!Validation.IsName (EditActivityName.Text))
      {
        EditActivityName.Background = InvalidInput;
        success = false;
      }
      return success;
    }


﻿//----------------------------------------------------------------------------------------
// Instance view


    // Load the list of instances for the selected user and activity.
    private void LoadInstanceList ()
    {
      InstanceItems.Clear ();
      int i = 0;
      Instance instance = MainProject.GetInstance (i);
      while (instance != null)
      {
        var item = new UIInstanceItem (this, instance.ID, instance.Name,
                                       instance.TimeSpent, instance.PercentFinished);
        InstanceItems.Add (item);
        i++;
        instance = MainProject.GetInstance (i);
      }
    }


    // Reloads the instances in the list.
    private void UpdateInstanceList ()
    {
      for (int i = 0; i < InstanceItems.Count; i++)
      {
        Instance instance = MainProject.GetInstance (i);
        if (instance == null)
          break;
        ((UIInstanceItem) InstanceItems [i]).Title = instance.Name;
        ((UIInstanceItem) InstanceItems [i]).Duration = instance.TimeSpent;
        ((UIInstanceItem) InstanceItems [i]).Percentage = instance.PercentFinished;
      }
    }


    // Select none of the instances.
    public void InstancesSelectNone ()
    {
      foreach (UIInstanceItem instance in InstanceItems)
        instance.Selected = false;
    }


﻿//----------------------------------------------------------------------------------------
// Session view


    // Load the list of sessions for the selected instance.
    private void LoadSessionList ()
    {
      SessionItems.Clear ();
      int i = 0;
      Session session = MainProject.GetSession (i);
      while (session != null)
      {
        var item = new UISessionItem (this, session.ID, session.Date.ToShortDateString (),
                                      session.TimeSpent, session.PercentFinished);
        SessionItems.Add (item);
        i++;
        session = MainProject.GetSession (i);
      }
    }


    // Reloads the sessions in the list.
    private void UpdateSessionList ()
    {
      for (int i = 0; i < SessionItems.Count; i++)
      {
        Session session = MainProject.GetSession (i);
        if (session == null)
          break;
        ((UIInstanceItem) SessionItems [i]).Title = session.Date.ToShortDateString ();
        ((UIInstanceItem) SessionItems [i]).Duration = session.TimeSpent;
        ((UIInstanceItem) SessionItems [i]).Percentage = session.PercentFinished;
      }
    }


    // Select none of the sessions.
    public void SessionsSelectNone ()
    {
      foreach (UISessionItem instance in SessionItems)
        instance.Selected = false;
    }


    // Remove new session validation colours.
    private void ClearNewSessionValidation ()
    {
        NewSessionYear.Background = ValidInput;
        NewSessionMonth.Background = ValidInput;
        NewSessionDay.Background = ValidInput;
        NewSessionTimeSpent.Background = ValidInput;
        NewSessionPercentFinished.Background = ValidInput;
    }


    // Validate new session input
    private bool ValidateNewSession ()
    {
      ClearNewSessionValidation ();
      bool success = true;
      if (!Validation.IsYear (NewSessionYear.Text))
      {
        NewSessionYear.Background = InvalidInput;
        success = false;
      }
      if (!Validation.IsMonth (NewSessionMonth.Text))
      {
        NewSessionMonth.Background = InvalidInput;
        success = false;
      }
      if (!Validation.IsDay (NewSessionDay.Text))
      {
        NewSessionDay.Background = InvalidInput;
        success = false;
      }
      if (!Validation.IsTime (NewSessionTimeSpent.Text))
      {
        NewSessionTimeSpent.Background = InvalidInput;
        success = false;
      }
      if (!Validation.IsPercent (NewSessionPercentFinished.Text))
      {
        NewSessionPercentFinished.Background = InvalidInput;
        success = false;
      }
      return success;
    }


    // Remove edit session validation colours.
    private void ClearEditSessionValidation ()
    {
        EditSessionYear.Background = ValidInput;
        EditSessionMonth.Background = ValidInput;
        EditSessionDay.Background = ValidInput;
        EditSessionTimeSpent.Background = ValidInput;
        EditSessionPercentFinished.Background = ValidInput;
    }


    // Validate edit session input
    private bool ValidateEditSession ()
    {
      ClearEditSessionValidation ();
      bool success = true;
      if (!Validation.IsYear (EditSessionYear.Text))
      {
        EditSessionYear.Background = InvalidInput;
        success = false;
      }
      if (!Validation.IsMonth (EditSessionMonth.Text))
      {
        EditSessionMonth.Background = InvalidInput;
        success = false;
      }
      if (!Validation.IsDay (EditSessionDay.Text))
      {
        EditSessionDay.Background = InvalidInput;
        success = false;
      }
      if (!Validation.IsTime (EditSessionTimeSpent.Text))
      {
        EditSessionTimeSpent.Background = InvalidInput;
        success = false;
      }
      if (!Validation.IsPercent (EditSessionPercentFinished.Text))
      {
        EditSessionPercentFinished.Background = InvalidInput;
        success = false;
      }
      return success;
    }



﻿//----------------------------------------------------------------------------------------
// Misc


    // Returns a list of indices of the selected items in a selectable UI element list.
    List <int> GetSelectedElements (List <ISelectableUI> elements)
    {
      var selected = new List <int> ();
      for (int i = 0; i < elements.Count; i++)
      {
        if (elements [i].Selected)
          selected.Add (i);
      }
      return selected;
    }
    

﻿//========================================================================================
// Event handlers


    private void MenuRegisterClick (object sender, RoutedEventArgs e)
    {
      // Launch the registration window.
      var register = new RegisterWindow ();
      register.MyProject = MainProject;
      bool? success = register.ShowDialog ();
      if (success is bool s && s)
        LoginSetup ();
    }


    private void MenuLoginClick (object sender, RoutedEventArgs e)
    {
      // Launch the login Window
      var login = new LoginWindow ();
      login.MyProject = MainProject;
      bool? success = login.ShowDialog ();
      if (success is bool s && s)
        LoginSetup ();
    }


    private void MenuLogoutClick (object sender, RoutedEventArgs e)
    {
      // [wip] Log out.
    }


    private void TextBox_TextChanged (object sender, TextChangedEventArgs e)
    {
      // [wip] Can this be removed?
    }


    private void MenuInstancesClick (object sender, RoutedEventArgs e)
    {
      ShowInstanceView ();
    }


    private void MenuActivitiesClick (object sender, RoutedEventArgs e)
    {
      ShowActivityView ();
    }


    private void AddButton_Click (object sender, RoutedEventArgs e)
    {
      switch (ActiveView)
      {
      case View.Activities:
        ShowNewActivity ();
        break;
      case View.Instances:
        ShowActivityView ();
        break;
      case View.Sessions:
        ShowNewSession ();
        break;
      default:
        break;
      }
    }


    private void EditButton_Click (object sender, RoutedEventArgs e)
    {
      List <int> selected;
      switch (ActiveView)
      {
      case View.Activities:
        selected = GetSelectedElements ((List <ISelectableUI>) ActivityItems);
        if (selected.Count == 1 && 
            MainProject.SelectActivity (ActivityItems [selected [0]].ID))
          ShowEditActivity ();
        break;
      case View.Instances:
        selected = GetSelectedElements ((List <ISelectableUI>) InstanceItems);
        if (selected.Count == 1 && 
            MainProject.SelectInstance (InstanceItems [selected [0]].ID))
          ShowSessionView ();
        break;
      case View.Sessions:
        selected = GetSelectedElements ((List <ISelectableUI>) SessionItems);
        if (selected.Count == 1 && 
            MainProject.SelectSession (SessionItems [selected [0]].ID))
          ShowEditSession ();
        break;
      default:
        break;
      }
    }


    private bool ConfirmDelete ()
    {
      return MessageBox.Show ("Delete selected items?", "Warning", 
                              MessageBoxButton.YesNo, MessageBoxImage.Warning) == 
                              MessageBoxResult.Yes;
    }


    private void DeleteButton_Click (object sender, RoutedEventArgs e)
    {
      if (!ConfirmDelete ())
        return;

      switch (ActiveView)
      {
      case View.Activities:
        foreach (var item in ActivityItems)
          if (item.Selected)
          {
            MainProject.SelectActivity (item.ID);
            MainProject.DeleteActivity ();
          }
        LoadActivityList ();
        ShowActivityList ();
        break;

      case View.Instances:
        foreach (var item in InstanceItems)
          if (item.Selected)
          {
            MainProject.SelectInstance (item.ID);
            MainProject.DeleteInstance ();
          }
        LoadInstanceList ();
        ShowInstanceList ();
        break;

      case View.Sessions:
        foreach (var item in SessionItems)
          if (item.Selected)
          {
            MainProject.SelectSession (item.ID);
            MainProject.DeleteSession ();
          }
        LoadSessionList ();
        ShowSessionList ();
        break;

      default:
        break;
      }
    }


    private void StartButtonClick (object sender, RoutedEventArgs e)
    {
      bool started = false;
      foreach (UIActivityItem item in ActivityItems)
      {
        if (item.Selected && MainProject.CreateInstance (item.ID))
          started = true;
      }
      if (started)
        ShowInstanceView ();
    }


    private void ButtonCeateActivity_Click (object sender, RoutedEventArgs e)
    {
      if (!ValidateNewActivity ())
        return;
      List <string> tagList = new List <string> ();
      foreach (object obj in NewActivityTags.Items)
        tagList.Add ((string) obj);
      MainProject.CreateActivity (NewActivityName.Text,
                                  NewActivityDescription.Text, tagList);
      LoadActivityList ();
      ShowActivityList ();
      HideNewActivity ();
    }


    private void ButtonCancelActivity_Click (object sender, RoutedEventArgs e)
    {
      HideNewActivity ();
    }


    private void ButtonSaveEditActivity_Click (object sender, RoutedEventArgs e)
    {
      if (!ValidateEditActivity ())
        return;
      List <string> tagList = new List <string> ();
      foreach (object obj in EditActivityTags.Items)
        tagList.Add ((string) obj);
      MainProject.EditActivity (EditActivityName.Text,
                                EditActivityDescription.Text, tagList);
      UpdateActivityList ();
      ShowActivityList ();
      HideEditActivity ();
    }


    private void ButtonCancelEditActivity_Click (object sender, RoutedEventArgs e)
    {
      HideEditActivity ();
    }


    private void ButtonCeateSession_Click (object sender, RoutedEventArgs e)
    {
      if (!ValidateNewSession ())
        return;
      int Year = Convert.ToInt32 (NewSessionYear.Text);
      int Month = Convert.ToInt32 (NewSessionMonth.Text);
      int Day = Convert.ToInt32 (NewSessionDay.Text);
      DateTime date = new DateTime (Year, Month, Day);
      Int64 timeSpent = Validation.TimeToInt (NewSessionTimeSpent.Text);
      Int64 percentFinished = Convert.ToInt64 (NewSessionPercentFinished.Text);
      MainProject.CreateSession (date, timeSpent, percentFinished);
      LoadSessionList ();
      ShowSessionList ();
      HideNewSession ();
    }


    private void ButtonCancelSession_Click (object sender, RoutedEventArgs e)
    {
      HideNewSession ();
    }


    private void ButtonEditSaveSession_Click (object sender, RoutedEventArgs e)
    {
      if (!ValidateEditSession ())
        return;
      int Year = Convert.ToInt32 (EditSessionYear.Text);
      int Month = Convert.ToInt32 (EditSessionMonth.Text);
      int Day = Convert.ToInt32 (EditSessionDay.Text);
      DateTime date = new DateTime (Year, Month, Day);
      Int64 timeSpent = Validation.TimeToInt (EditSessionTimeSpent.Text);
      Int64 percentFinished = Convert.ToInt64 (EditSessionPercentFinished.Text);
      MainProject.EditSession (date, timeSpent, percentFinished);
      UpdateSessionList ();
      ShowSessionList ();
      HideEditSession ();
    }


    private void ButtonEditCancelSession_Click (object sender, RoutedEventArgs e)
    {
      HideEditSession ();
    }


    private void NewActivityAddTag_Click (object sender, RoutedEventArgs e)
    {
      if (Validation.IsName (NewActivityTagInput.Text))
      {
        foreach (object obj in NewActivityTags.Items)
        {
          if ((string) obj == NewActivityTagInput.Text)
            return;
        }
        NewActivityTags.Items.Add (NewActivityTagInput.Text);
        NewActivityTagInput.Text = string.Empty;
      }
    }


    private void NewActivityTagInput_KeyDown (object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
        NewActivityAddTag_Click (null, null);
    }


    private void NewActivityTags_KeyDown (object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete || e.Key == Key.Back)
      {
        NewActivityTags.Items.Remove (NewActivityTags.SelectedItem);
      }
    }


    private void EditActivityAddTag_Click (object sender, RoutedEventArgs e)
    {
      if (Validation.IsName (EditActivityTagInput.Text))
      {
        foreach (object obj in EditActivityTags.Items)
        {
          if ((string) obj == EditActivityTagInput.Text)
            return;
        }
        EditActivityTags.Items.Add (EditActivityTagInput.Text);
        EditActivityTagInput.Text = string.Empty;
      }
    }


    private void EditActivityTagInput_KeyDown (object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
        EditActivityAddTag_Click (null, null);
    }


    private void EditActivityTags_KeyDown (object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete || e.Key == Key.Back)
      {
        EditActivityTags.Items.Remove (EditActivityTags.SelectedItem);
      }
    }

  }
}
