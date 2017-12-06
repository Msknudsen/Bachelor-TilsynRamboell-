using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using Foundation;
using UIKit;

namespace Ramboell.iOS
{
    public partial class ProjectListViewController : UITableViewController
    {
        public List<ProjectInfo> ProjectInfos;

        public ProjectListViewController (IntPtr handle) : base (handle)
        {
            var node = Database.DefaultInstance.GetRootReference().GetChild("pdf");
            nuint handleReference = node.ObserveEvent(DataEventType.Value, (snapshot) => {
               
                ProjectInfos = new List<ProjectInfo>{new ProjectInfo{Name = "Add New Project"}};
                foreach (var element in snapshot.GetValue<NSDictionary>())
                {
                    ProjectInfos.Add(new ProjectInfo
                    {
                        Guid = new Guid(element.Key.Description),
                        Name = element.Value.ValueForKeyPath((NSString) "name").Description,
                        MetaId = new Guid(element.Value.ValueForKeyPath((NSString) "metaId").Description),
                        Created = element.Value.ValueForKeyPath((NSString) "created").Description,
                        Updated = element.Value.ValueForKeyPath((NSString) "updated").Description
                    });
                }
            });
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView = new UITableView(View.Bounds)
            {
                Source = new TableSource(ProjectInfos, this),
                AllowsSelection = true,
                AllowsMultipleSelection = false
            };
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