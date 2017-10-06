using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ramboell
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Overview : ContentPage
	{
		public Overview ()
		{
			InitializeComponent ();

            ProjectsListView.ItemsSource = new List<ProjectInfo>
            {
                new ProjectInfo
                {
                    Name = "Bro Ansvej",
                    Adress = "Ansvej",
                    City = "Silkeborg",
                    ZipCode = 8600
                },

                new ProjectInfo
                {
                    Name = "Tunnel Finlandsgade",
                    Adress = "Finlandsgade",
                    City = "Aarhus N",
                    ZipCode = 8200
                },

                new ProjectInfo
                {
                    Name = "Bro Olaf Palmes Alle",
                    Adress = "Olaf Palmes Alle",
                    City = "Aarhus N",
                    ZipCode = 8200
                },

            };
		}
	}
}