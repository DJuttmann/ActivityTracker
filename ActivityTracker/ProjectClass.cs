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

  class Project
  {
    private List <User> Users;
    private List <Activity> Activities;
    private List <Tag> Tags; 
    private User ActiveUser;
    DatabaseConnection Database;
    string DatabaseFileName = "ActivityDatabase.sqlite";

    
    Project () {
      Users = new List <User> ();
      Activities = new List <Activity> ();
      ActiveUser = null;
      Database = null;
    }


    bool LoadDatabase ()
    {
      try
      {
        Database = new DatabaseConnection (DatabaseFileName);
        if (!Database.LoadAllTags (Tags))
          return false;
        if (!Database.LoadAllActivities (Activities, Tags))
          return false;
      }
      catch
      {
        return false;
      }
      return true;
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
      if (ActiveUser != null) // cannot register if someone is already logged in
        return false;
      if (!ValidatePassword (password))
        return false;

      // [wip] check if the username already exists!
      
      User newUser = new User (userName, CreateHash (password), type);
      if (Database.AddUser (newUser))
      {
        ActiveUser = newUser; // new user is logged in
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
      tempUser.LoadFromDatabase (Database, userName);
      if (tempUser.PasswordHash == CreateHash (password))
      {
        ActiveUser = tempUser;
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

   
    // Create new activity owned by user.
    public bool CreateActivity (User user, string name, string description)
    {
      if (ActiveUser == null)
        return false;

      Int64 id = Database.NewActivityID ();

      Activity newActivity = new Activity (id, user.ID, name, description);
      if (Database.AddActivity (newActivity))
      {
        Activities.Add (newActivity);
        user.AddActivity (newActivity);
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
  }
}