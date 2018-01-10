//========================================================================================
// ActivityTracker by Daan Juttmann
// Created: 2017-12-19
// License: GNU General Public License 3.0 (https://www.gnu.org/licenses/gpl-3.0.en.html).
//========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.IO;


namespace ActivityTracker
{

  public enum UserType
  {
    Student,
    Teacher
  }


//========================================================================================
// Class User
﻿//========================================================================================


  public class User
  {
    public Int64 ID {get; private set;}
    public string Name {get; private set;}
    public string PasswordHash {get; private set;}
    public UserType Type {get; private set;}
    private List <Tag> Tags;
    private List <Instance> Instances;
    private List <Activity> Activities;


    // Constructor
    public User ()
    {
      ID = 0;
      Name = "";
      PasswordHash = "";
    }


    // Constructor from name and password
    public User (string name, string password, UserType type)
    {
      if (true) // [wip] check if username exists
      {
        Name = name;

        // [wip] get new id from database.

        // [wip] calculate hash for password.
      }
    }


    // Add a new tag to the user.
    public bool HasTag (Tag newTag)
    {
      return Tags.Contains (newTag);
    }


    // Add a new tag to the user.
    public void AddTag (Tag newTag)
    {
      if (!Tags.Contains (newTag))
        Tags.Add (newTag);
    }


    // Remove a tag from the user.
    public void RemoveTag (Tag deleteTag)
    {
      Tags.Remove (deleteTag);
    }


    // Add a new activity to the user.
    public void AddActivity (Activity newActivity)
    {
      if (!Activities.Contains (newActivity))
        Activities.Add (newActivity);
    }


    // Remove an activity from the user.
    public void RemoveActivity (Activity deleteActivity)
    {
      Activities.Remove (deleteActivity);
    }


    // Add an activity instance to the user.
    public void AddInstance (Instance instance)
    {
      if (!Instances.Contains (instance))
        Instances.Add (instance);
    }


    // Delete a user's activity instance by index.
    public void DeleteInstance (int index)
    {
      if (index < Instances.Count)
      {
        Instances.RemoveAt (index);
      }
    }


    // Delete a user's activity instance by ID.
    public void DeleteInstanceByID (int ID)
    {
      for (int i = 0; i < Instances.Count; i++)
        if (Instances [i].ID == ID)
        {
          Instances.RemoveAt (i);
          i--;
        }
    }


    public bool LoadFromDatabase (DatabaseConnection database, string userName)
    {
      Int64 newID = 0;
      string newPasswordHash = null;
      UserType newType = UserType.Student; 
      if (database.LoadUser (userName, ref newID, ref newPasswordHash, ref newType))
      {
        ID = newID;
        PasswordHash = newPasswordHash;
        Type = newType;
        return true;
      }
      return false;
    }

  }


//========================================================================================
// Class Activity
﻿//========================================================================================


  public class Activity
  {
    public Int64 ID {get; private set;}
    public Int64 CreatorID {get; private set;}
    public string Name {get; private set;}
    public string Description {get; private set;}
    private List <Tag> Tags;


    // Constructor.
    public Activity (Int64 id, Int64 creatorID, string name, string description)
    {
      ID = id;
      CreatorID = creatorID;
      Name = name;
      Description = description;
      Tags = new List <Tag> ();
    }


    // Check if activity has tag
    public bool HasTag (Tag tag)
    {
      return Tags.Any (x => x.Equals (tag));
    }


    // Add tag to the Activity.
    public void AddTag (Tag newTag)
    {
      if (!Tags.Any (x => x.Equals (newTag)))
        Tags.Add (newTag);
    }


    // Add tag to the Activity based on ID and list of all tags.
    public void AddTag (Int64 tagID, List <Tag> allTags)
    {
      Tag newTag = allTags.Find (x => x.ID == tagID);
      if (newTag != null)
        AddTag (newTag);
    }


    // Add tag to the Activity based on ID and list of all tags.
    public void RemoveTag (Tag deleteTag)
    {
      for (int i = 0; i < Tags.Count;)
      {
        if (Tags [i].Equals (deleteTag))
          Tags.RemoveAt (i);
        else
          i++;
      }
    }

  }


//========================================================================================
// Class Instance
﻿//========================================================================================


  public class Instance
  {
    public Int64 ID {get; private set;}
    private Activity MyActivity;
    public Int64 ActivityID {get {return MyActivity.ID;}}
    private List <Session> Sessions;

    public Int64 TimeSpent // Time in minutes;
    {
      get
      {
        Int64 time = 0;
        foreach (Session s in Sessions)
        {
          time += s.TimeSpent;
        }
        return time;
      }
    }

    public Int64 PercentFinished
    {
      get
      {
        return Sessions [Sessions.Count - 1].PercentFinished;
      }
    }

//----------------------------------------------------------------------------------------

    public Instance (Int64 id, Activity act)
    {
      ID = id;
      MyActivity = act;
    }


    public void AddSession (Session newSession)
    {
      if (!Sessions.Contains (newSession))
      {
        Sessions.Add (newSession);
        Sessions.Sort (CompareDates); // sort by date
      }
    }


    private static int CompareDates (Session a, Session b)
    {
      if (a.Date < b.Date)
        return -1;
      if (a.Date > b.Date)
        return 1;
      return 0;
    }
  }


//========================================================================================
// Class Session
﻿//========================================================================================

  public class Session
  {
    public Int64 ID {get; private set;}
    public DateTime Date {get; private set;}
    public Int64 TimeSpent {get; private set;}
    public Int64 PercentFinished {get; private set;}


    public Session (Int64 id, DateTime date, Int64 timeSpent, Int64 percentFinished)
    {
      ID = id;
      Date = date;
      TimeSpent = timeSpent;
      PercentFinished = percentFinished;
    }


    public void SetDateTime (int year, int month, int day, int hour, int minute, int second)
    {
      Date = new DateTime (year, month, day, hour, minute, second);
    }


    public void SetTimeSpent (int timeSpent)
    {
      TimeSpent = timeSpent;
    }


    public void SetPercentFinished (int percentFinished)
    {
      PercentFinished = percentFinished;
    }

  }


//========================================================================================
// Class Tag
﻿//========================================================================================


  public class Tag
  {
    public Int64 ID {get; private set;}
    public string Name {get; private set;}


    public Tag (Int64 id, string name)
    {
      ID = id;
      Name = name;
    }


    public override bool Equals (object obj)
    {
      if (obj is Tag t)
      {
        return t.ID == ID;
      }
      return false;
    }

  }


//========================================================================================
// END
﻿//========================================================================================

}