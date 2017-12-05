using System;
using Firebase.Database;
using Foundation;
using UIKit;

namespace Ramboell.iOS
{
    public partial class ProjectListViewController : UITableViewController
    {

        public ProjectListViewController (IntPtr handle) : base (handle)
        {
            //var rootNode = Database.DefaultInstance.GetRootReference();
        }
        string[] tableItems;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView = new UITableView(View.Bounds);
            //rootNode = rootNode.GetChild("pdf");
            tableItems = new[] { "Add new Project","Sample", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };
            TableView.Source = new TableSource(tableItems, this);
            TableView.AllowsSelection = true;
            TableView.AllowsMultipleSelection = false;
        }
        //public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        //{
        //    Console.WriteLine($"Row Selected: {0}", tableItems[indexPath.Row]);

        //    UIAlertController okAlertController = UIAlertController.Create("Row Selected", tableItems[indexPath.Row], UIAlertControllerStyle.Alert);
        //    okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
        //    tableView.DeselectRow(indexPath, true);
        //}
    }
}