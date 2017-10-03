using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ramboell
{

    public partial class MainPage : ContentPage
	{

		public MainPage()
		{
			InitializeComponent();
		}

	    private void LoginButton_OnClicked(object sender, EventArgs e)
	    {
            var email = UsernameLoginEntry;
            var pass = PasswordLoginEntry;

            var firebaseAuthService = DependencyService.Get<IFireBaseAuthService>();
            firebaseAuthService.SignIn(email.Text, pass.Text);   
	    }
	}
}
