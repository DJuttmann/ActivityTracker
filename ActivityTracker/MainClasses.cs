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
    private int ID;
    private string Name;
    private string PasswordHash;
    private UserType Type;
    private List <Tag> Tags;
    private List <Instance> Instances;
    private List <Activity> Activities;


    public User ()
    {
      ID = 0;
      Name = "";
      PasswordHash = "";
    }


    public User (string name, string password)
    {
      if (true) // [wip] check if username exists
      {
        Name = name;

        // [wip] get new id from database.

        // [wip] calculate hash for password.
      }
    }


    public void AddTag (Tag newTag)
    {
      if (!Tags.Contains (newTag))
        Tags.Add (newTag);
    }


    public void RemoveTag (Tag deleteTag)
    {
      Tags.Remove (deleteTag);
    }


    public void AddInstance (Activity act)
    {
      Instance i = new Instance (act);
      Instances.Add (i);
    }


    public void DeleteInstance (int index)
    {
      if (index < Instances.Count)
      {
        Instances.RemoveAt (index);
      }
    }


    public void DeleteInstanceByID (int ID)
    {
      for (int i = 0; i < Instances.Count; i++)
        if (Instances [i].ID == ID)
        {
          Instances.RemoveAt (i);
          i--;
        }
    }
  }


//========================================================================================
// Class Activity
﻿//========================================================================================

  class Activity
  {
    private int ID;
    private string Name;
    private string Description;
    private List <Tag> Tags;
  }


//========================================================================================
// Class Instance
﻿//========================================================================================


  class Instance
  {
    public int ID {get; private set;}
    private Activity MyActivity;
    private int TimeSpent;
    private int PercentFinished;
    private List <Session> Sessions;


    public Instance (Activity act)
    {
      // [wip] get new instance id from database.

      MyActivity = act;
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


    public Session ()
    {
    }
  }


//========================================================================================
// Class Tag
﻿//========================================================================================


  class Tag
  {
    public int ID {get; private set;}
    public string Name {get; private set;}
  }


//========================================================================================
// END
﻿//========================================================================================

}