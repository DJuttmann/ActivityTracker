//========================================================================================
// ActivityTracker by Daan Juttmann
// Created: 2017-12-19
// License: GNU General Public License 3.0 (https://www.gnu.org/licenses/gpl-3.0.en.html).
//========================================================================================

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.IO;


namespace ActivityTracker
{
  enum UserType
  {
    Student,
    Teacher
  }


//========================================================================================
// Class User
﻿//========================================================================================


  class User
  {
    public int ID {get; private set;}
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
    public User (string name, string password)
    {
      if (true) // [wip] check if username exists
      {
        Name = name;

        // [wip] get new id from database.

        // [wip] calculate hash for password.
      }
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


    // Add an activity instance to the user.
    public void AddInstance (Activity act)
    {
      Instance i = new Instance (act);
      Instances.Add (i);
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


    public void AddToDatabase (DatabaseConnection database)
    {
      database.AddUser (this);
    }


    public void UpdateInDatabase (DatabaseConnection database)
    {
      database.UpdateUser (this);
    }
  }


//========================================================================================
// Class Activity
﻿//========================================================================================


  class Activity
  {
    public int ID {get; private set;}
    public int CreatorID {get; private set;}
    public string Name {get; private set;}
    public string Description {get; private set;}
    private List <Tag> Tags;



    public void AddToDatabase (DatabaseConnection database)
    {
      database.AddActivity (this);
    }


    public void UpdateInDatabase (DatabaseConnection database)
    {
      database.UpdateActivity (this);
    }

  }


//========================================================================================
// Class Instance
﻿//========================================================================================


  class Instance
  {
    public int ID {get; private set;}
    private Activity MyActivity;
    public int ActivityID {get {return MyActivity.ID;}}
    private List <Session> Sessions;

    public int TimeSpent // Time in minutes;
    {
      get
      {
        int time = 0;
        foreach (Session s in Sessions)
        {
          time += s.TimeSpent;
        }
        return time;
      }
    }

    public int PercentFinished
    {
      get
      {
        return Sessions [Sessions.Count - 1].PercentFinished;
      }
    }

//----------------------------------------------------------------------------------------

    public Instance (Activity act)
    {
      // [wip] get new instance id from database.

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


    public void AddToDatabase (DatabaseConnection database, User parentUser)
    {
      database.AddInstance (parentUser.ID, this);
    }


    public void UpdateInDatabase (DatabaseConnection database)
    {
      database.UpdateInstance (this);
    }


    public void DeleteFromDatabase (DatabaseConnection database)
    {
      database.DeleteInstance (this.ID);
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

  class Session
  {
    public int ID {get; private set;}
    public DateTime Date {get; private set;}
    public int TimeSpent {get; private set;}
    public int PercentFinished {get; private set;}


    public Session (int id, DateTime date, int timeSpent, int percentFinished)
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


    public void AddToDatabase (DatabaseConnection database, Instance parentInstance)
    {
      database.AddSession (parentInstance.ID, this);
    }


    public void UpdateInDatabase (DatabaseConnection database)
    {
      database.UpdateSession (this);
    }


    public void DeleteFromDatabase (DatabaseConnection database)
    {
      database.DeleteSession (this.ID);
    }
  }


//========================================================================================
// Class Tag
﻿//========================================================================================


  class Tag
  {
    public int ID {get; private set;}
    public string Name {get; private set;}


    Tag (int id, string name)
    {
      ID = id;
      Name = name;
    }

  }


//========================================================================================
// END
﻿//========================================================================================

}