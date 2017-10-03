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
	public partial class OpretBruger : ContentPage
	{
		public OpretBruger ()
		{
			InitializeComponent ();
		}

	    private void AddUserButton_OnClicked(object sender, EventArgs e)
	    {
	        throw new NotImplementedException();
	    }

	    private void CancelUserButton_Clicked(object sender, EventArgs e)
	    {
	        throw new NotImplementedException();
	    }
	}
}