using Foundation;
using System;
using UIKit;

namespace RamboellView
{
    public partial class ProjectListViewController : UITableViewController
    {
        public ProjectListViewController (IntPtr handle) : base (handle)
        {
        }
        string[] tableItems;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView = new UITableView(View.Bounds);
            tableItems = new[] { "Add new Project","Sample", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };
            TableView.Source = new TableSource(tableItems);
            Add(TableView);
        }
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UIAlertController okAlertController = UIAlertController.Create("Row Selected", tableItems[indexPath.Row], UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            //action
            tableView.DeselectRow(indexPath, true);
        }
    }
}