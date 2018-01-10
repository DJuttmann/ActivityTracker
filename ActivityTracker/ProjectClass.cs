using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;


namespace ActivityTracker
{

//========================================================================================
// Class Project
﻿//========================================================================================

  public class Project
  {
    private List <User> Users;
    private List <Activity> Activities;
    private List <Tag> Tags; 
    private User ActiveUser; // The user who is logged in.
    private User SelectedUser; // The user whose data is being viewed.
    private Activity SelectedActivity;
    private Instance SelectedInstance;
    DatabaseConnection Database;
    private string DatabaseFileName;



    public string SelectedUserName
    {
      get
      {
        return SelectedUser != null ? SelectedUser.Name : "";
      }
    }

    public string SelectedActivityName
    {
      get
      {
        return SelectedActivity != null ? SelectedActivity.Name : "";
      }
    }

    
    // Constructor.
    public Project () {
      Users = new List <User> ();
      Activities = new List <Activity> ();
      Tags = new List <Tag> ();
      ActiveUser = null;
      SelectedUser = null;
      SelectedActivity = null;
      SelectedInstance = null;
      Database = null;
      DatabaseFileName = null;
    }


    // Load a database file.
    public bool LoadDatabase (string fileName)
    {
      bool success = true;
      try
      {
        Database = new DatabaseConnection (fileName);
        if (!Database.LoadAllTags (Tags))
          success = false;
        if (!Database.LoadAllActivities (Activities, Tags))
          success = false;
      }
      catch (Exception ex)
      {
        System.Windows.MessageBox.Show ("Failed to open database connection." +
                                        Environment.NewLine +
                                        ex.Message);
        return false;
      }
      DatabaseFileName = fileName;
      if (!success)
        System.Windows.MessageBox.Show ("Failed to load activity and/or tag lists.");
      return success;
    }


//========================================================================================
// Account management

    
    // Check if password is not too short or too long, and if its characters are valid.
    bool ValidatePassword (string password)
    {
      if (password.Length < 8 || password.Length > 32)
        return false;
      for (int i = 0; i < password.Length; i++)
      {
        if (password [i] < ' ' || password [i] > '~')
          return false;
      }
      return true;
    }

    
    // Convert byte to hex string.
    String ByteToHex (byte b)
    {
      const string characters = "0123456789ABCDEF";
      return characters.Substring (b >> 4, 1) + characters.Substring (b & 0b1111, 1);
    }


    // Create a hash from a password string.
    private string CreateHash (string password)
    {
      byte [] passwordArray = new byte [password.Length]; 
      for (int i = 0; i < password.Length; i++)
      {
        passwordArray [i] = Convert.ToByte (password [i]);
      }

      var hash = new SHA512CryptoServiceProvider ();
      byte [] hashArray = hash.ComputeHash (passwordArray);

      StringBuilder hashString = new StringBuilder ();
      for (int i = 0; i < hashArray.Length; i++)
      {
        hashString.Append (ByteToHex (hashArray [i]));
      }
      return hashString.ToString ();
    }
    

    // Register a new user into the system.
    public bool RegisterUser (string userName, string password, UserType type)
    {
      if (Database == null || ActiveUser != null) // cannot register if someone is already logged in
        return false;
      if (!ValidatePassword (password))
        return false;

      // [wip] check if the username already exists!
      
      User newUser = new User (Database.NewUserID (), userName,
                               CreateHash (password), type);
      if (Database.AddUser (newUser))
      {
        ActiveUser = newUser; // new user is logged in
        SelectedUser = ActiveUser;
        return true;
      }
      return false;
    }


    // Try to log in a user given username and password. Returns false on failure.
    public bool LoginUser (string userName, string password)
    {
      if (!ValidatePassword (password))
        return false;

      User tempUser = new User ();
      if (!tempUser.LoadFromDatabase (Database, userName))
      {
        System.Windows.MessageBox.Show ("Username not found.");
        return false; // user not found.
      }
      // [wip] password check disabled for now
      if (true) // tempUser.PasswordHash == CreateHash (password))
      {
        ActiveUser = tempUser;
        SelectedUser = ActiveUser;
        return true;
      }
      return false;
    }


    // Add tag to activity.
    public bool AddUserTag (User user, Tag tag)
    {
      if (user.HasTag (tag))
        return false;
      if (Database.AddUserTag (ActiveUser.ID, tag.ID))
      {
        user.AddTag (tag);
        return true;
      }
      return false;
    }
   
    
    // Remove tag from activity.
    public bool RemoveUserTag (User user, Tag tag)
    {
      if (Database.RemoveUserTag (user.ID, tag.ID))
      {
        user.RemoveTag (tag);
        return true;
      }
      return false;
    }


//========================================================================================
// Activity management

   
    // Create new activity owned by SelectedUser.
    public bool CreateActivity (string name, string description)
    {
      Int64 id = Database.NewActivityID ();

      Activity newActivity = new Activity (id, SelectedUser.ID, name, description);
      if (Database.AddActivity (newActivity))
      {
        Activities.Add (newActivity);
        SelectedUser.AddActivity (newActivity);
        return true;
      }
      return false;
    }


    // Add tag to activity.
    public bool AddActivityTag (Activity activity, Tag tag)
    {
      if (activity.HasTag (tag))
        return false;
      if (Database.AddActivityTag (activity.ID, tag.ID))
      {
        activity.AddTag (tag);
        return true;
      }
      return false;
    }
   
    
    // Remove tag from activity.
    public bool RemoveActivityTag (Activity activity, Tag tag)
    {
      if (Database.RemoveActivityTag (activity.ID, tag.ID))
      {
        activity.RemoveTag (tag);
        return true;
      }
      return false;
    }


    public Activity GetActivity (int index)
    {
      if (index < 0 || index >= Activities.Count)
        return null;
      return Activities [index];
    }


//========================================================================================
// Instance management


    public bool CreateInstance (User user, Activity activity)
    {
      Int64 id = Database.NewInstanceID ();

      Instance newInstance = new Instance (id, activity);
      if (Database.AddInstance (user.ID, newInstance))
      {
        user.AddInstance (newInstance);
        return true;
      }
      return false;
    }

//========================================================================================
// Session management


    public bool CreateSession (Instance instance, DateTime date, Int64 timeSpent, 
                               Int64 percentFinished)
    {
      Int64 id = Database.NewSessionID ();

      Session newSession = new Session (id, date, timeSpent, percentFinished);
      if (Database.AddSession (instance.ID, newSession))
      {
        instance.AddSession (newSession);
        return true;
      }
      return false;
    }


//    public bool DeleteSession (Session session)
//    {
//      if (Database.DeleteSession (session.ID))
//    }

//========================================================================================
// Tag management


    public bool CreateTag (string name)
    {
      Int64 id = Database.NewTagID ();

      Tag newTag = new Tag (id, name);
      if (Database.AddTag (newTag))
      {
        Tags.Add (newTag);
        return true;
      }
      return false;
    }
  }
}