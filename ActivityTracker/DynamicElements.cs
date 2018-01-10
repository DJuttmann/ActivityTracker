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

  class UIActivityItem
  {
    public Int64 ActivityID {get; private set;}
    private Border Box;
    private Grid Container;
    private Label title;
    private Label creator;
    private Label description;

    public string Title
    {
      get {return (string) title.Content;}
      set {title.Content = value;}
    }
    public string Creator
    {
      get {return (string) creator.Content;}
      set {creator.Content = "Created by" + value;}
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


    // Constructor
    public UIActivityItem (Int64 id, string title, string creator, string description) {
      Box = new Border ();
      Container = new Grid ();
      this.title = new Label ();
      this.creator = new Label ();
      this.description = new Label ();

      ActivityID = id;

      Box.Height = 40;
      Box.HorizontalAlignment = HorizontalAlignment.Stretch; 
      Box.Margin = new Thickness (4, 4, 4, 4);
      Color BorderColor = new Color ()
      {
        R = 0, G = 0, B = 0, A = 255
      };
      Box.BorderBrush = new SolidColorBrush (BorderColor);
      Box.BorderThickness = new Thickness (1);

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

      Box.Child = Container;
      Container.Children.Add (this.title);
      Container.Children.Add (this.creator);
      Container.Children.Add (this.description);
    }

  }
}