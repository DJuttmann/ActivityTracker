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
    None,
    Student,
    Teacher
  }


//========================================================================================
// Class User
﻿//========================================================================================


  public class User
  {
    public Int64 ID;
    public string Name {get; private set;}
    public string PasswordHash {get; private set;}
    public UserType Type {get; private set;}
    private List <Tag> Tags;
    private List <Instance> Instances;
    private List <Activity> Activities;
    public bool DataLoaded {get; private set;}

    public List <string> TagNames
    {
      get
      {
        var tagList = new List <string> ();
        foreach (Tag t in Tags)
          tagList.Add (t.Name);
        return tagList;
      }
    }


    // Constructor
    public User ()
    {
      ID = 0;
      Name = "";
      PasswordHash = "";
      Type = UserType.Student;

      Tags = new List <Tag> ();
      Instances = new List <Instance> ();
      Activities = new List <Activity> ();

      DataLoaded = false;
    }


    // Constructor from name and password
    public User (Int64 id, string name, string password, UserType type)
    {
      this.ID = id;
      Name = name;
      PasswordHash = password;
      Type = type;

      Tags = new List <Tag> ();
      Instances = new List <Instance> ();
      Activities = new List <Activity> ();
    }


    // Add a new tag to the user.
    public bool HasTag (Tag newTag)
    {
      return Tags.Contains (newTag);
    }


    // Chect if activity has a tag containing string s.
    public bool HasTagWithText (string s)
    {
      if (s == String.Empty)
        return true;
      foreach (string tagName in TagNames)
        if (tagName.ToLower ().Contains (s))
          return true;
      return false;
    }


    // Add a new tag to the user.
    public void AddTag (Tag newTag)
    {
      if (!Tags.Contains (newTag))
        Tags.Add (newTag);
    }


    // Add tag to the Activity based on ID and list of all tags.
    public void AddTag (Int64 tagID, List <Tag> allTags)
    {
      Tag newTag = allTags.Find (x => x.ID == tagID);
      if (newTag != null)
        AddTag (newTag);
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
    public void DeleteActivity (Activity deleteActivity)
    {
      Activities.Remove (deleteActivity);
    }


    // Return instance with given index.
    public Instance GetInstance (int index)
    {
      if (index < 0 || index >= Instances.Count)
        return null;
      return Instances [index];
    }


    // Return instance with given ID.
    public Instance GetInstance (Int64 id)
    {
      return Instances.Find (x => x.ID == id);
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


    // Delete a user's activity instance by id.
    public void DeleteInstance (Int64 id)
    {
      Instances.RemoveAll (x => x.ID == id);
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


    // Load user info from database based on user name.
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


    // Load user data (instances and sessions) from database.
    public bool LoadDataFromDatabase (DatabaseConnection database,
                                      List <Activity> allActivities)
    {
      DataLoaded = false;
      if (!database.LoadUserInstances (ID, Instances, allActivities))
        return false;
      foreach (Instance instance in Instances)
      {
        if (!instance.LoadDataFromDatabase (database))
          return false;
      }
      DataLoaded = true;
      return true;
    }

  }


//========================================================================================
// Class Activity
﻿//========================================================================================


  public class Activity
  {
    public Int64 ID;
    public Int64 CreatorID {get; private set;}
    public string Name {get; set;}
    public string Description {get; set;}
    private List <Tag> tags;
    public string CreatorName {get; private set;}

    public List <string> TagNames
    {
      get
      {
        var tagList = new List <string> ();
        foreach (Tag t in tags)
          tagList.Add (t.Name);
        return tagList;
      }
    }


    // Constructor.
    public Activity (Int64 id, Int64 creatorID, string name, string description, string creatorName)
    {
      this.ID = id;
      CreatorID = creatorID;
      Name = name;
      Description = description;
      tags = new List <Tag> ();
      CreatorName = creatorName;
    }


    // Check if activity has tag
    public bool HasTag (Tag tag)
    {
      return tags.Any (x => x.Equals (tag));
    }


    // Chect if activity has a tag containing string s.
    public bool HasTagWithText (string s)
    {
      if (s == String.Empty)
        return true;
      foreach (string tagName in TagNames)
        if (tagName.ToLower ().Contains (s))
          return true;
      return false;
    }


    // Add tag to the Activity.
    public void AddTag (Tag newTag)
    {
      if (!tags.Any (x => x.Equals (newTag)))
        tags.Add (newTag);
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
      for (int i = 0; i < tags.Count;)
      {
        if (tags [i].Equals (deleteTag))
          tags.RemoveAt (i);
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
    public Int64 ID;
    private Activity MyActivity;
    public Int64 ActivityID {get {return MyActivity.ID;}}
    private List <Session> Sessions;
    public bool DataLoaded {get; private set;}

    public string Name
    {
      get
      {
        return MyActivity.Name;
      }
    }

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
        if (Sessions.Count == 0)
          return 0;
        return Sessions [Sessions.Count - 1].PercentFinished;
      }
    }

//----------------------------------------------------------------------------------------

    public Instance (Int64 id, Activity act)
    {
      this.ID = id;
      MyActivity = act;
      Sessions = new List <Session> ();
      DataLoaded = false;
    }


    // Return session with given index.
    public Session GetSession (int index)
    {
      if (index < 0 || index >= Sessions.Count)
        return null;
      return Sessions [index];
    }


    // Return session with given ID.
    public Session GetSession (Int64 id)
    {
      return Sessions.Find (x => x.ID == id);
    }


    public void AddSession (Session newSession)
    {
      if (!Sessions.Contains (newSession))
      {
        Sessions.Add (newSession);
        Sessions.Sort (CompareDates); // sort by date
      }
    }


    public void DeleteSession (Int64 id)
    {
      Sessions.RemoveAll (x => x.ID == id);
    }


    private static int CompareDates (Session a, Session b)
    {
      if (a.Date < b.Date)
        return -1;
      if (a.Date > b.Date)
        return 1;
      return 0;
    }


    public bool LoadDataFromDatabase (DatabaseConnection database)
    {
      DataLoaded = database.LoadInstanceSessions (ID, Sessions);
      return DataLoaded;
    }
  }


//========================================================================================
// Class Session
﻿//========================================================================================

  public class Session
  {
    public Int64 ID;
    public DateTime Date {get; set;}
    public Int64 TimeSpent {get; set;}
    public Int64 PercentFinished {get; set;}


    public Session (Int64 id, DateTime date, Int64 timeSpent, Int64 percentFinished)
    {
      this.ID = id;
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
    public Int64 ID;
    public string Name {get; private set;}


    public Tag (Int64 id, string name)
    {
      this.ID = id;
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