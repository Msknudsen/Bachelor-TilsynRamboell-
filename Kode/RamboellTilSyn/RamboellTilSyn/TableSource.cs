using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

namespace Ramboell.iOS
{
    public class TableSource : UITableViewSource
    {
        public List<RegistrationDto> ProjectInfos { get; }
    
        //string[] TableItems;

        string CellIdentifier = "TableCell";

        private ProjectListViewController ProjectListViewController { get; }

 
        public TableSource(List<RegistrationDto> projectInfos, ProjectListViewController projectListViewController)
        {
            ProjectInfos = projectInfos;
            ProjectListViewController = projectListViewController;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return ProjectInfos.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            string item = ProjectInfos[indexPath.Row].Name;

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            { cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

            cell.TextLabel.Text = item;

            return cell;
        }
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
                switch (indexPath.Row)
                {
                    case 0:
                        if (ProjectListViewController.Storyboard.InstantiateViewController("CreateProjectViewController") is CreateProjectViewController createProjectVC)
                        {

                            ProjectListViewController.NavigationController.PushViewController(createProjectVC, true);
                        }
                        break;

                    case 1:
                        if (ProjectListViewController.Storyboard.InstantiateViewController("CreateUserViewController") is CreateUserViewController createUserVC)
                        {

                            ProjectListViewController.NavigationController.PushViewController(createUserVC, true);
                        }
                        break;

                    default:
                        if (!(ProjectListViewController.Storyboard.InstantiateViewController("PdfViewController") is PdfViewController controller)) return;
                        controller.PDFInfo = ProjectInfos[indexPath.Row];
                        ProjectListViewController.NavigationController.PushViewController(controller, true);
                        break;
                }         
        }
    }
}