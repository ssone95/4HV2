//
// _4H_Application.Views.MainPageMenuItem: Object representation of an item in
//   the sidebar navigation menu.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using static _4H_Application.Views.Pages.MainPage;

namespace _4H_Application.Views.Pages {

    /// <summary>
    /// Class that represents a menu item in the application side menu
    /// </summary>
    public class MainPageMenuItem {

        /// <summary>
        /// Default constructor for each item on the side menu
        /// </summary>
        public MainPageMenuItem() {
        }

        // Unique ID for the item
        public int Id { get; set; }

        // The name of the menu item in the menu itself
        public string MenuTitle { get; set; }

        // The target of the button that sets the resulting content pane
        public PageType TargetType { get; set; }

    }

}
