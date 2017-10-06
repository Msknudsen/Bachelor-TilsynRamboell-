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

            ProjectsListView.ItemsSource = new List<string>
            {
                "Ansvej", "Finlandsgade", "Olaf Palmes Alle"
            };
		}
	}
}