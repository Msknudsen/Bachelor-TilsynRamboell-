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
    /// <summary>
    /// ViewController used for showing list of different project in Ramboell
    /// </summary>
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class ProjectListViewController : UITableViewController
    { 
        public List<RegistrationDto> ProjectInfos;
        nuint handleReference;
        DatabaseReference _node;
        private readonly string _file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Global.ProjectListFileName);
        public ProjectListViewController (IntPtr handle) : base (handle)
        {
            _node = Database.DefaultInstance.GetRootReference().GetChild(Global.Pdf);
            _node.KeepSynced(true);
        }

        /// <summary>
        /// Adding a default list of data it needs to show if there is no project list to be found 
        /// </summary>
        private void InstaniateJsonFile()
        {
            if (!File.Exists(_file))
            {
                ProjectInfos = new List<RegistrationDto> { new RegistrationDto { Name = Global.AddProjectLabel }, new RegistrationDto { Name = Global.AddUserLabel } };
                File.WriteAllText(_file, JsonConvert.SerializeObject(ProjectInfos));
            }
            else
            {
                var data = File.ReadAllText(_file);
                ProjectInfos = JsonConvert.DeserializeObject<List<RegistrationDto>>(data);
            }
        }

        /// <summary>
        /// Latching onto a FirebaseNode and listen to events on the perticular node in this case "pdf node from firebase if there is any changes on the node, the event would create a updated local file" 
        /// </summary>
        private void SyncWithFirebaseDatabase()
        {
            handleReference = _node.ObserveEvent(DataEventType.Value, (snapshot) =>
            {

                ProjectInfos = new List<RegistrationDto> { new RegistrationDto { Name = Global.AddProjectLabel }, new RegistrationDto { Name = Global.AddUserLabel } };

                foreach (var element in snapshot.GetValue<NSDictionary>())
                {
                    ProjectInfos.Add(new RegistrationDto
                    {
                        Guid = new Guid(element.Key.Description),
                        Name = element.Value.ValueForKeyPath((NSString) nameof(RegistrationDto.Name)).Description,
                        MetaId = new Guid(element.Value.ValueForKeyPath((NSString)nameof(RegistrationDto.MetaId)).Description),
                        Created = element.Value.ValueForKeyPath((NSString)nameof(RegistrationDto.Created)).Description,
                        Updated = element.Value.ValueForKeyPath((NSString)nameof(RegistrationDto.Updated)).Description
                    });
                    if (File.Exists(_file))
                    {   
                        File.WriteAllText(_file, JsonConvert.SerializeObject(ProjectInfos));

                        if (TableView.Source is TableSource source)
                        {
                            source.ProjectInfos = ProjectInfos;
                            TableView.ReloadData();
                        }
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
            InstaniateJsonFile();
            TableView = new UITableView(View.Bounds)
            {
                Source = new TableSource(ProjectInfos, this),
                AllowsSelection = true,
                AllowsMultipleSelection = false
            };
            SyncWithFirebaseDatabase();
        }
    }
}