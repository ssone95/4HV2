//
// _4H_Application.DefaultPage.xaml.cs: Class that represents the default page
//   displayed on the screen before any of the navigation buttons are selected
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages {

    /// <summary>
    /// Class that wraps the center of the device screen
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DefaultPage : ContentPage {

        /// <summary>
        /// Default constructor for the default main page detail
        /// </summary>
        public DefaultPage() {
            InitializeComponent();
        }

    }

}
