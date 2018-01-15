//========================================================================================
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

  interface ISelectableUI
  {
    Int64 ID {get;}
    bool Selected {get; set;}
    UIElement Element {get;}
  }


//========================================================================================
// Class UIActivityItem
﻿//========================================================================================


  class UIActivityItem: ISelectableUI
  {
    private MainWindow ParentWindow;
    public Int64 ID {get; private set;}
    private Border Box;
    private Grid Container;
    private Label title;
    private Label creator;
    private Label description;
    private bool selected;

    public string Title
    {
      get {return (string) title.Content;}
      set {title.Content = value;}
    }

    public string Creator
    {
      get {return (string) creator.Content;}
      set {creator.Content = "Created by " + value;}
    }

    public string Description
    {
      get {return (string) description.Content;}
      set {description.Content = value;}
    }

    public UIElement Element // Returns the element that can be added to UI.
    {
      get {return (UIElement) Box;}
    }

    public bool Selected
    {
      get {return selected;}
      set
      {
        selected = value;
        if (selected)
        {
          Color selected = new Color ()
          {
            R = 0xE0, G = 0xE0, B = 0xF0, A = 0xFF
          };
          Container.Background = new SolidColorBrush (selected);
        }
        else
        {
          Color unselected = new Color ()
          {
            R = 0xFF, G = 0xFF, B = 0xFF, A = 0xFF
          };
          Container.Background = new SolidColorBrush (unselected);
        }
      }
    }


    // Constructor
    public UIActivityItem (MainWindow parentWindow, Int64 id, string title,
                           string creator, string description)
    {
      Box = new Border ();
      Container = new Grid ();
      this.title = new Label ();
      this.creator = new Label ();
      this.description = new Label ();

      ParentWindow = parentWindow;
      ID = id;

      Box.Height = 50;
      Box.HorizontalAlignment = HorizontalAlignment.Stretch; 
      Box.Margin = new Thickness (4, 4, 4, 4);
      Color BorderColor = new Color ()
      {
        R = 0, G = 0, B = 0, A = 255
      };
      Box.BorderBrush = new SolidColorBrush (BorderColor);
      Box.BorderThickness = new Thickness (1);
      Box.MouseDown += this.Click;

      Container.VerticalAlignment = VerticalAlignment.Stretch;
      Container.HorizontalAlignment = HorizontalAlignment.Stretch;

      Title = title;
      this.title.VerticalAlignment = VerticalAlignment.Top;
      this.title.HorizontalAlignment = HorizontalAlignment.Left;

      Creator = creator;
      this.creator.VerticalAlignment = VerticalAlignment.Top;
      this.creator.HorizontalAlignment = HorizontalAlignment.Right;

      Description = description;
      this.description.VerticalAlignment = VerticalAlignment.Bottom;
      this.description.HorizontalAlignment = HorizontalAlignment.Left;

      Selected = false;

      Box.Child = Container;
      Container.Children.Add (this.title);
      Container.Children.Add (this.creator);
      Container.Children.Add (this.description);
    }


    /* Identify the UIActivityItem by its border.
    public bool Identify (object b)
    {
      return Box == b;
    }*/


    // Handle click event
    public void Click (object sender, RoutedEventArgs e)
    {
      if (!(Keyboard.IsKeyDown (Key.LeftCtrl) || Keyboard.IsKeyDown (Key.RightCtrl)))
        ParentWindow.ActivitiesSelectNone ();
      Selected = !Selected;
    }

  }


//========================================================================================
// Class ProgressBar
﻿//========================================================================================


  class ProgressBar
  {
    public Grid Bar;
    ColumnDefinition LeftColumn;
    ColumnDefinition RightColumn;
    private Grid BarLeft;
    private Grid BarRight;
    
    public Int64 Percent
    {
      set
      {
        if (value >= 0 && value <= 100)
        {
          LeftColumn.Width = new GridLength (value, GridUnitType.Star);
          RightColumn.Width = new GridLength (100 - value, GridUnitType.Star);
        }
      }
    }


    // Constructor
    public ProgressBar ()
    {
      Bar = new Grid ();
      BarLeft = new Grid ();
      BarRight = new Grid ();
      LeftColumn = new ColumnDefinition ();
      RightColumn = new ColumnDefinition ();

      Bar.HorizontalAlignment = HorizontalAlignment.Stretch;
      Bar.VerticalAlignment = VerticalAlignment.Bottom;
      Bar.Height = 23;

      BarLeft.HorizontalAlignment = HorizontalAlignment.Stretch;
      BarLeft.VerticalAlignment = VerticalAlignment.Stretch;
      BarLeft.SetValue (Grid.ColumnProperty, 0);
      Color completed = new Color ()
      {
        R = 0xA0, G = 0xA0, B = 0xE0, A = 0x80
      };
      BarLeft.Background = new SolidColorBrush (completed);

      BarRight.HorizontalAlignment = HorizontalAlignment.Stretch;
      BarRight.VerticalAlignment = VerticalAlignment.Stretch;
      BarRight.SetValue (Grid.ColumnProperty, 1);
      Color remaining = new Color ()
      {
        R = 0xD0, G = 0xD0, B = 0xF0, A = 0x80
      };
      BarRight.Background = new SolidColorBrush (remaining);

      Percent = 0; // set inital percentage to 0, also sets column definitions.

      Bar.ColumnDefinitions.Add (LeftColumn);
      Bar.ColumnDefinitions.Add (RightColumn);
      Bar.Children.Add (BarLeft);
      Bar.Children.Add (BarRight);
    }
  }


//========================================================================================
// Class UIInstanceItem
﻿//========================================================================================


  class UIInstanceItem: ISelectableUI
  {
    protected MainWindow ParentWindow;
    protected Int64 id;
    protected Border Box;
    protected Grid Container;
    protected Label title;
    protected Label duration;
    protected Label percentage;
    protected ProgressBar Progress;
    protected bool selected;

    public Int64 ID 
    {
      get {return id;}
    }

    public string Title
    {
      get {return (string) title.Content;}
      set {title.Content = value;}
    }

    public Int64 Duration
    {
      set {duration.Content = (value / 60).ToString () + ":" + (value % 60).ToString ();}
    }

    public Int64 Percentage
    {
      set
      {
        percentage.Content = value.ToString () + "%";
        Progress.Percent = value;
      }
    }

    public UIElement Element // Returns the element that can be added to UI.
    {
      get {return (UIElement) Box;}
    }

    public bool Selected
    {
      get {return selected;}
      set
      {
        selected = value;
        if (selected)
        {
          Color selected = new Color ()
          {
            R = 0xE0, G = 0xE0, B = 0xF0, A = 0xFF
          };
          Container.Background = new SolidColorBrush (selected);
        }
        else
        {
          Color unselected = new Color ()
          {
            R = 0xFF, G = 0xFF, B = 0xFF, A = 0xFF
          };
          Container.Background = new SolidColorBrush (unselected);
        }
      }
    }



    // Constructor
    public UIInstanceItem (MainWindow parentWindow, Int64 id, string title,
                           Int64 duration, Int64 percentage)
    {
      Box = new Border ();
      Container = new Grid ();
      this.title = new Label ();
      this.duration = new Label ();
      this.percentage = new Label ();
      Progress = new ProgressBar ();


      ParentWindow = parentWindow;
      this.id = id;

      Box.Height = 50;
      Box.HorizontalAlignment = HorizontalAlignment.Stretch; 
      Box.Margin = new Thickness (4, 4, 4, 4);
      Color BorderColor = new Color ()
      {
        R = 0, G = 0, B = 0, A = 255
      };
      Box.BorderBrush = new SolidColorBrush (BorderColor);
      Box.BorderThickness = new Thickness (1);
      Box.MouseDown += this.Click;

      Container.VerticalAlignment = VerticalAlignment.Stretch;
      Container.HorizontalAlignment = HorizontalAlignment.Stretch;

      Title = title;
      this.title.VerticalAlignment = VerticalAlignment.Top;
      this.title.HorizontalAlignment = HorizontalAlignment.Left;

      Duration = duration;
      this.duration.VerticalAlignment = VerticalAlignment.Top;
      this.duration.HorizontalAlignment = HorizontalAlignment.Right;

      Percentage = percentage;
      this.percentage.VerticalAlignment = VerticalAlignment.Bottom;
      this.percentage.HorizontalAlignment = HorizontalAlignment.Center;

      Selected = false;

      Box.Child = Container;
      Container.Children.Add (this.title);
      Container.Children.Add (this.duration);
      Container.Children.Add (Progress.Bar);
      Container.Children.Add (this.percentage);
    }


    // Handle click event
    public virtual void Click (object sender, RoutedEventArgs e)
    {
      if (!(Keyboard.IsKeyDown (Key.LeftCtrl) || Keyboard.IsKeyDown (Key.RightCtrl)))
        ParentWindow.InstancesSelectNone ();
      Selected = !Selected;
    }

  }


//========================================================================================
// Class UISessionItem
﻿//========================================================================================

 
  class UISessionItem: UIInstanceItem, ISelectableUI
  {

    public UISessionItem (MainWindow parentWindow, Int64 id, string title,
                          Int64 duration, Int64 percentage):
        base (parentWindow, id, title, duration, percentage) {}


    // Handle click event
    public override void Click (object sender, RoutedEventArgs e)
    {
      if (!(Keyboard.IsKeyDown (Key.LeftCtrl) || Keyboard.IsKeyDown (Key.RightCtrl)))
        ParentWindow.SessionsSelectNone ();
      Selected = !Selected;
    }

  }

}