﻿using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

namespace Ramboell.iOS
{
    public class TableSource : UITableViewSource
    {
        public List<ProjectInfo> ProjectInfos { get; }
    
        //string[] TableItems;

        string CellIdentifier = "TableCell";

        public ProjectListViewController ProjectListViewController { get; }

 
        public TableSource(List<ProjectInfo> projectInfos, ProjectListViewController projectListViewController)
        {
            ProjectInfos = projectInfos;
            ProjectListViewController = projectListViewController;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return ProjectInfos.Count + 1;
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
            if (indexPath.Row == 0)
            {
                //TODO add create new project viewcontroller aand logic to it
                return;
            }
            else
            {
                //look for file local, if not there download. 
                if (!(ProjectListViewController.Storyboard.InstantiateViewController("PdfViewController") is PdfViewController controller)) return;
                controller.PDFInfo = ProjectInfos[indexPath.Row];
                ProjectListViewController.NavigationController.PushViewController(controller, true);

            }

        }
    }
}