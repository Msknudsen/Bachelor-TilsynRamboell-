using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Firebase.Database;
using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UIKit;

namespace Ramboell.iOS
{
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class ProjectListViewController : UITableViewController
    { 
        public List<ProjectInfo> ProjectInfos;
        nuint handleReference;
        DatabaseReference _node;

        private readonly string _file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "list.json");
        public ProjectListViewController (IntPtr handle) : base (handle)
        {
            _node = Database.DefaultInstance.GetRootReference().GetChild("pdf");
            _node.KeepSynced(true);

            InstaniateJsonFile();
            SyncWithFirebaseDatabase();
        }

        private void InstaniateJsonFile()
        {
            
            if (!File.Exists(_file))
            {
                ProjectInfos = new List<ProjectInfo> { new ProjectInfo { Name = "Add New Project" } };
                File.WriteAllText(_file, JsonConvert.SerializeObject(ProjectInfos));
            }
            else
            {
                var data = File.ReadAllText(_file);
                ProjectInfos = JsonConvert.DeserializeObject<List<ProjectInfo>>(data);
            }
        }

        private void SyncWithFirebaseDatabase()
        {
            // var s = _node.GetQueryOrderedByKey();
            handleReference = _node.ObserveEvent(DataEventType.Value, (snapshot) =>
            {
                ProjectInfos = new List<ProjectInfo> { new ProjectInfo { Name = "Add New Project" } };
                foreach (var element in snapshot.GetValue<NSDictionary>())
                {
                    ProjectInfos.Add(new ProjectInfo
                    {
                        Guid = new Guid(element.Key.Description),
                        Name = element.Value.ValueForKeyPath((NSString)"name").Description,
                        MetaId = new Guid(element.Value.ValueForKeyPath((NSString)"metaId").Description),
                        Created = element.Value.ValueForKeyPath((NSString)"created").Description,
                        Updated = element.Value.ValueForKeyPath((NSString)"updated").Description
                    });
                    if (File.Exists(_file))
                    {
                        File.WriteAllText(_file, JsonConvert.SerializeObject(ProjectInfos));
                        TableView.ReloadData();
                    }
                    else
                        Console.WriteLine("No Json file containing a list of projectInfo found");
                }
            });
        }

        public override void ViewDidUnload()
        {
            base.ViewDidLoad();
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