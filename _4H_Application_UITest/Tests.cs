using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using _4H_Application.Views.Pages;

namespace _4H_Application_UITest
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;
        private enum PageType {
            Profile,
            HorseManager,
            RecordBook,
            Project,
            Pictures,
            Export
        }
        private enum SeekDirection { 
            Up,
            Down
        }

        int deviceWidth;
        int deviceLength;

        List<string> Breeds = new List<string>() {
                "Appaloosa",
                "Arabian",
                "Belgian",
                "Buckskin",
                "Clydesdale",
                "Haflinger",
                "Half-Arabian",
                "Half-Welsh",
                "Miniature Horse",
                "Morgan",
                "Paint",
                "Palomino",
                "Paso Fino",
                "Percheron",
                "Pinto",
                "Pony of America",
                "Quarter Horse",
                "Saddlebred",
                "Shetland Pony",
                "Shire",
                "Standardbred",
                "Tennessee Walking",
                "Thoroughbred",
                "Welsh Pony",
                "Warmblood",
                "Unspecified",
        };
        string[] feedTypes = { "Hay", "Grain", "Salt and Minerals", "Pasture", "Other" };
        string[] beddingTypes = { "Hay", "Hemp", "Moss", "Paper", "Sawdust", "Shavings", "Stall Mats", "Straw", "Wood Pellets" };
        string[] servicesTypes = { "Riding Lessons/Instruction", "Health Care", "Farrier", "Other" };
        Dictionary<PageType, string> navpairs;
        Random rng = new Random(); // for picking things at random
        const int NUMBER_OF_BREEDS = 26;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        /// <summary>
        /// Uses the natively running android or ios application to navigate to a new page
        /// </summary>
        /// <param name="dest"></param>
        public void NavigateTo(string dest)
        {
            if (platform == Platform.Android) {
                app.Invoke("ChangePage", dest);
            } else if (platform == Platform.iOS) {
                // add ios naviagtion backdoor stuff here
            }
        }

        /// <summary>
        /// Clicks the AddButton depending on the test platform
        /// </summary>
        /// <returns></returns>
        private Func<AppQuery, AppQuery> AddButton()
        {
            if (platform == Platform.Android) {
                return new Func<AppQuery, AppQuery>(c => c.Class("ActionMenuView"));
            } else if (platform == Platform.iOS) {
                return null; //add ios code later
            } else {
                throw new Exception("Something went majorly wrong trying to tap the AddHorseButton");
            }
        }

        /// <summary>
        /// Seeks in a direction for an element using a string for a marked item
        /// </summary>
        /// <param name="marked">Item to look for with property "marked"</param>
        /// <param name="dir">Direction to scroll in</param>
        private void SeekToElement(string marked, SeekDirection dir)
        {
            SeekToElement(c => c.Marked(marked), dir);
        }

        /// <summary>
        /// Seeks in a direction for an element using a query
        /// </summary>
        /// <param name="query">Item to look for using the query</param>
        /// <param name="dir">Direction to scroll in</param>
        private void SeekToElement(Func<AppQuery, AppQuery> query, SeekDirection dir)
        {
            if (app.Query(query).Length == 0) {
                switch (dir) { 
                    case SeekDirection.Up:
                        app.ScrollUpTo(query);
                        break;
                    case SeekDirection.Down:
                        app.ScrollDownTo(query);
                        break;
                }
            }
        }

        /// <summary>
        /// Builds a horse that only has a name; mostly used for records tests
        /// </summary>
        /// <param name="name"></param>
        private void QuickHorse (string name)
        {
            app.Invoke("ChangePage", navpairs[PageType.HorseManager]);
            app.Tap(AddButton());
            app.WaitForElement(c => c.Text("Select A Horse Type"));
            app.Tap(c => c.Text("Standard"));
            app.WaitForElement("HorseEditPage");
            app.Tap("HorseNameEntry");
            app.EnterText(name);
            app.DismissKeyboard();
            app.Tap("SaveButton");
        }

        /// <summary>
        /// Fills the items on a record entry's edit screen
        /// </summary>
        /// <param name="fields">List of fields as strings that need to be generated</param>
        /// <param name="type">Feed or bedding type to be added</param>
        private void RecordBuild (List<string> fields, int type = 0)
        {
            // set description
            if (fields.Contains("Description")) {
                SeekToElement(c => c.Class("ButtonRenderer").Text("Save"), SeekDirection.Up);
                SeekToElement(c => c.Text("Description").Sibling("EditorRenderer").Child("FormsEditText"), SeekDirection.Down);
                app.Tap(c => c.Text("Description").Sibling("EditorRenderer").Child("FormsEditText"));
                app.ClearText();
                app.EnterText("ABCDEFGHIJKLMNOPQRSTUVWKYZ1234" +
                    "ABCDEFGHIJKLMNOPQRSTUVWKYZ1234" +
                    "ABCDEFGHIJKLMNOPQRSTUVWKYZ1234" +
                    "ABCDEFGHIJKLMNOPQRSTUVWKYZ1234" +
                    "ABCDEFGHIJKLMNOPQRSTUVWKYZ1234");
                app.DismissKeyboard();
            }
            if (fields.Contains("DescriptionOpt")) {
                SeekToElement(c => c.Class("ButtonRenderer").Text("Save"), SeekDirection.Up);
                SeekToElement(c => c.Text("Description (Optional)").Sibling("EditorRenderer").Child("FormsEditText"), SeekDirection.Down);
                app.Tap(c => c.Text("Description (Optional)").Sibling("EditorRenderer").Child("FormsEditText"));
                app.ClearText();
                app.EnterText("ABCDEFGHIJKLMNOPQRSTUVWKYZ1234" +
                    "ABCDEFGHIJKLMNOPQRSTUVWKYZ1234" +
                    "ABCDEFGHIJKLMNOPQRSTUVWKYZ1234" +
                    "ABCDEFGHIJKLMNOPQRSTUVWKYZ1234" +
                    "ABCDEFGHIJKLMNOPQRSTUVWKYZ1234");
                app.DismissKeyboard();
            }
            // set type for feed, service, or bedding
            if (fields.Contains("TypeFeed")) {
                SeekToElement(c => c.Class("ButtonRenderer").Text("Save"), SeekDirection.Up);
                SeekToElement(c => c.Text("Type").Sibling("PickerRenderer"), SeekDirection.Down);
                app.Tap(c => c.Text("Type").Sibling("PickerRenderer"));
                app.Tap(c => c.Text(feedTypes[type]));
            }
            if (fields.Contains("TypeBed")) {
                SeekToElement(c => c.Class("ButtonRenderer").Text("Save"), SeekDirection.Up);
                SeekToElement(c => c.Text("Type").Sibling("PickerRenderer"), SeekDirection.Down);
                app.Tap(c => c.Text("Type").Sibling("PickerRenderer"));
                app.Tap(c => c.Text(beddingTypes[type]));
            }
            if (fields.Contains("TypeService")) {
                SeekToElement(c => c.Class("ButtonRenderer").Text("Save"), SeekDirection.Up);
                SeekToElement(c => c.Text("Type").Sibling("PickerRenderer"), SeekDirection.Down);
                app.Tap(c => c.Text("Type").Sibling("PickerRenderer"));
                app.Tap(c => c.Text(servicesTypes[type]));
            }
            // set location
            if (fields.Contains("Location")) {
                SeekToElement(c => c.Class("ButtonRenderer").Text("Save"), SeekDirection.Up);
                SeekToElement(c => c.Text("Location").Sibling("EditorRenderer").Child("FormsEditText"), SeekDirection.Down);
                app.Tap(c => c.Text("Location").Sibling("EditorRenderer").Child("FormsEditText"));
                app.ClearText();
                app.EnterText("ABCDEFGHIJKLMNOPQRSTUVWKYZ1234" +
                    "ABCDEFGHIJKLMNOPQRSTUVWKYZ1234" +
                    "ABCDEFGHIJKLMNOPQRSTUVWKYZ1234" +
                    "ABCDEFGHIJKLMNOPQRSTUVWKYZ1234" +
                    "ABCDEFGHIJKLMNOPQRSTUVWKYZ1234");
                app.DismissKeyboard();
            }
            // set start and end times
            if (fields.Contains("Times")) {
                // Set start time to now in AM
                SeekToElement(c => c.Class("ButtonRenderer").Text("Save"), SeekDirection.Up);
                SeekToElement(c => c.Text("Start Time").Sibling("ButtonRenderer"), SeekDirection.Down);
                app.Tap(c => c.Text("Start Time").Sibling("ButtonRenderer"));
                app.Tap(c => c.Text("Start Time").Sibling("TimePickerRenderer"));
                app.Tap(c => c.Id("am_label"));
                app.Tap(c => c.Text("OK"));
                // set end time to now in PM
                SeekToElement(c => c.Class("ButtonRenderer").Text("Save"), SeekDirection.Up);
                SeekToElement(c => c.Text("End Time").Sibling("ButtonRenderer"), SeekDirection.Down);
                app.Tap(c => c.Text("End Time").Sibling("ButtonRenderer"));
                app.Tap(c => c.Text("End Time").Sibling("TimePickerRenderer"));
                app.Tap(c => c.Id("pm_label"));
                app.Tap(c => c.Text("OK"));
            }
            // set amount
            if (fields.Contains("Amount")) {
                SeekToElement(c => c.Class("ButtonRenderer").Text("Save"), SeekDirection.Up);
                SeekToElement(c => c.Text("Amount (lbs.)").Sibling("EntryRenderer").Child("FormsEditText"), SeekDirection.Down);
                app.Tap(c => c.Text("Amount (lbs.)").Sibling("EntryRenderer").Child("FormsEditText"));
                app.ClearText();
                app.EnterText("2.50");
                app.DismissKeyboard();
            }
            // set cost
            if (fields.Contains("Cost")) {
                SeekToElement(c => c.Class("ButtonRenderer").Text("Save"), SeekDirection.Up);
                SeekToElement(c => c.Text("Cost ($)").Sibling("EntryRenderer").Child("FormsEditText"), SeekDirection.Down);
                app.Tap(c => c.Text("Cost ($)").Sibling("EntryRenderer").Child("FormsEditText"));
                app.ClearText();
                app.EnterText("5.00");
                app.DismissKeyboard();
            }

            // save the record
            SeekToElement(c => c.Class("ButtonRenderer").Text("Save"), SeekDirection.Up);
            app.WaitForElement(c => c.Class("ButtonRenderer").Text("Save"));
            app.Tap(c => c.Class("ButtonRenderer").Text("Save"));
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
            deviceWidth = (int)app.Query().FirstOrDefault().Rect.Width;
            deviceLength = (int)app.Query().FirstOrDefault().Rect.Height;

            // Adds key/values for page types
            navpairs = new Dictionary<PageType, string>();
            navpairs.Add (PageType.Profile, "Profile");
            navpairs.Add (PageType.HorseManager, "HorseManager");
            navpairs.Add (PageType.RecordBook, "RecordBook");
            navpairs.Add (PageType.Project, "Project");
            navpairs.Add (PageType.Pictures, "Pictures");
            navpairs.Add (PageType.Export, "Export");
        }

        /// <summary>
        /// not a real test, just used for test development
        /// Launches the Repl tool
        /// </summary>
        [Test]
        public void LaunchRepl()
        {
            app.Repl();
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }

        [Test]
        public void ProfileActions ()
        {
            // change to the profile page
            NavigateTo(navpairs[PageType.Profile]);

            // add test stuff when profile page is done
        }

        [Test]
        public void GenerateHorses()
        {
            // change to the horse manager page
            NavigateTo(navpairs[PageType.HorseManager]);

            int i = 0;
            bool toasted = false;

            while (true)
            {
                // Wait to see the horse manager
                app.WaitForElement("HorseManagerListView");

                // Add a horse
                app.Tap(AddButton());
                
                // Wait to see the horse type selection
                app.WaitForElement(c => c.Text("Select A Horse Type"));

                // Alternate picking a standard and a miniature
                if (i % 2 == 0) {
                    app.Tap(c => c.Text("Standard"));
                } else {
                    app.Tap(c => c.Text("Miniature"));
                }

                // at  this point, look to see if a toast appears. if it doesn't, then continue.
                for (int q = 0; q < 10 && app.Query(c => c.Marked("HorseNameEntry")).Length == 0; q++) {
                    if (app.Query(c => c.Id("message").Text("Cannot create any more horses.")).Length > 0) {
                        toasted = true;
                        break;
                    }
                }

                // if the toast appeared, break.
                // if the toast DOESN'T appear, make sure we haven't hit the horse limit
                if (toasted) {
                    break;
                } else {
                    Assert.AreNotEqual(20, i, "Test is still trying to make horses despite being full");
                }

                // Enter a horse name
                SeekToElement("SaveButton", SeekDirection.Up);
                SeekToElement("HorseNameEntry", SeekDirection.Down);
                app.Tap("HorseNameEntry");
                app.EnterText("Horse" + i);
                app.DismissKeyboard();

                // Enter a horse age
                SeekToElement("SaveButton", SeekDirection.Up);
                SeekToElement("HorseAgeEntry", SeekDirection.Down);
                app.Tap("HorseAgeEntry");
                app.ClearText();
                app.EnterText(String.Format("{0}", rng.Next(1, 51)));
                app.DismissKeyboard();

                // Enter a horse sex
                SeekToElement("SaveButton", SeekDirection.Up);
                SeekToElement("HorseSexPicker", SeekDirection.Down);
                app.Tap("HorseSexPicker");
                switch (i % 4)
                {
                    case 0:
                        app.Tap(c => c.Text("Colt"));
                        break;
                    case 1:
                        app.Tap(c => c.Text("Filly"));
                        break;
                    case 2:
                        app.Tap(c => c.Text("Gelding"));
                        break;
                    case 3:
                        app.Tap(c => c.Text("Mare"));
                        break;
                }

                // Select a horse breed
                SeekToElement("SaveButton", SeekDirection.Up);
                SeekToElement("HorseBreedPicker", SeekDirection.Down);
                app.Tap("HorseBreedPicker");
                string thisKindOfHorse = Breeds[rng.Next(0, Breeds.Count)];
                while (app.Query(c => c.Text(thisKindOfHorse)).Length == 0)
                    app.ScrollDown(c => c.Id("select_dialog_listview"));
                app.Tap(c => c.Text(thisKindOfHorse));

                // Throw in a height
                SeekToElement("SaveButton", SeekDirection.Up);
                SeekToElement("HorseHeightEntry", SeekDirection.Down);
                app.Tap("HorseHeightEntry");
                app.ClearText();
                app.EnterText(String.Format("{0}.{1}", rng.Next(1, 26), rng.Next(0, 99)));
                app.DismissKeyboard();

                // Throw in a weight
                SeekToElement("SaveButton", SeekDirection.Up);
                SeekToElement("HorseWeightEntry", SeekDirection.Down);
                app.Tap("HorseWeightEntry");
                app.ClearText();
                app.EnterText(String.Format("{0}.{1}", rng.Next(1, 501), rng.Next(0, 99)));
                app.DismissKeyboard();

                // Add some color text
                SeekToElement("SaveButton", SeekDirection.Up);
                SeekToElement("HorseColorEditor", SeekDirection.Down);
                app.Tap("HorseColorEditor");
                app.EnterText("Yes this is indeed a color of a horse");
                app.DismissKeyboard();

                // Add some markings text
                SeekToElement("SaveButton", SeekDirection.Up);
                SeekToElement("HorseMarkingsEditor", SeekDirection.Down);
                app.Tap("HorseMarkingsEditor");
                app.EnterText("Indeed this horse has many a marking let me count the ways");
                app.DismissKeyboard();

                // Save the horse
                SeekToElement("SaveButton", SeekDirection.Up);
                app.Tap("SaveButton");

                // increment counter for prosperity's sake
                i++;
            }

            // make sure we made enough horses
            Assert.AreEqual(20, i, "Didn't make enough horses");
        }

        [Test]
        public void MakeRecords ()
        {
            // make a horse real quick
            QuickHorse("Alucard");

            // change to records page
            NavigateTo(navpairs[PageType.RecordBook]);

            // Make two months of records
            for (int i = 0; i < 2; i++)
            {
                // Wait for the calendar to render
                app.WaitForElement(c => c.Text("01"));

                // Get the number of days in this month
                int daysToProcess = app.Query(c => c.Marked("CalendarContent").Child()).Length;

                // make records for every day of the current month
                for (int j = 1; j < daysToProcess + 1; j++)
                {
                    // Tap the next day
                    app.Tap(c => c.Text(j.ToString("00")));

                    #region Activity Records
                    // Spawn some activity records
                    SeekToElement("ActivitiesBox", SeekDirection.Up);
                    app.Tap("ActivitiesBox");

                    for (int k = 0; k < 6; k++) {
                        app.Tap(AddButton());
                        if (app.Query(c => c.Text("Cannot add any more records for the day!")).Length > 0 && k == 5)
                            continue;
                        Assert.AreNotEqual(5, k, "Didn't find the toast for max records made during Activities generation.");
                        for (int l = 0; l < k; l++) app.ScrollDown();
                        app.Tap(c => c.Id(app.Query(d => d.Text("Description")).Last().Id));
                        RecordBuild(new List<string>() { "Description", "Location" });
                    }
                    app.WaitForElement(AddButton());
                    app.Back();

                    // Check if we made the right number of activities
                    SeekToElement("ActivitiesCount", SeekDirection.Down);
                    Assert.AreEqual("5 entries", app.Query("ActivitiesCount").FirstOrDefault().Text, "Didn't make 5 entries for Activity records.");
                    #endregion

                    #region Riding Records
                    // Spawn some riding/driving/training records
                    SeekToElement("RidingCount", SeekDirection.Down);
                    app.Tap("RidingBox");

                    for (int k = 0; k < 6; k++) {
                        app.Tap(AddButton());
                        if (app.Query(c => c.Text("Cannot add any more records for the day!")).Length > 0 && k == 5)
                            continue;
                        Assert.AreNotEqual(5, k, "Didn't find the toast for max records made during Riding generation.");
                        for (int l = 0; l < k; l++) app.ScrollDown();
                        app.Tap(c => c.Id(app.Query(d => d.Text("Description")).Last().Id));
                        RecordBuild(new List<string>() { "Description", "Times" });
                    }
                    app.WaitForElement(AddButton());
                    app.Back();

                    // Check if we made the right number of riding/driving/training records
                    Assert.AreEqual("5 entries", app.Query("RidingCount").FirstOrDefault().Text, "Didn't make 5 entries for Riding//Driving//Training records.");
                    #endregion

                    #region Feed Records
                    // Spawn some feed records
                    SeekToElement("FeedBox", SeekDirection.Down);
                    app.Tap("FeedBox");

                    for (int k = 0; k < 6; k++) {
                        app.Tap(AddButton());
                        if (app.Query(c => c.Text("Cannot add any more records for the day!")).Length > 0 && k == 5)
                            continue;
                        Assert.AreNotEqual(5, k, "Didn't find the toast for max records made during Feed generation.");
                        for (int l = 0; l < k; l++) app.ScrollDown();
                        app.Tap(c => c.Id(app.Query(d => d.Text("Type")).Last().Id));
                        if (k==4) {
                            RecordBuild(new List<string>() { "TypeFeed", "DescriptionOpt", "Cost" }, k);
                        } else {
                            RecordBuild(new List<string>() { "TypeFeed", "DescriptionOpt", "Amount", "Cost" }, k);
                        }
                    }
                    app.WaitForElement(AddButton());
                    app.Back();
                    
                    // Check if we made the right number of feed records
                    SeekToElement("FeedCount", SeekDirection.Down);
                    Assert.AreEqual("5 entries", app.Query("FeedCount").FirstOrDefault().Text, "Didn't make 5 entries for Feed records.");
                    #endregion

                    #region Bedding Records
                    // Spawn some bedding records
                    SeekToElement("BeddingBox", SeekDirection.Down);
                    app.Tap("BeddingBox");

                    for (int k = 0; k < 6; k++) {
                        app.Tap(AddButton());
                        if (app.Query(c => c.Text("Cannot add any more records for the day!")).Length > 0 && k == 5)
                            continue;
                        Assert.AreNotEqual(5, k, "Didn't find the toast for max records made during Bedding generation.");
                        for (int l = 0; l < k; l++) app.ScrollDown();
                        app.Tap(c => c.Id(app.Query(d => d.Text("Type")).Last().Id));
                        RecordBuild(new List<string>() { "TypeBed", "Amount", "Cost" }, rng.Next(1, 9));
                    }
                    app.WaitForElement(AddButton());
                    app.Back();

                    // Check if we made the right number of bedding records
                    SeekToElement("BeddingCount", SeekDirection.Down);
                    Assert.AreEqual("5 entries", app.Query("BeddingCount").FirstOrDefault().Text, "Didn't make 5 entries for Bedding records.");
                    #endregion

                    #region Labor Records
                    // Spawn some labor records
                    SeekToElement("LaborBox", SeekDirection.Down);
                    app.Tap("LaborBox");

                    for (int k = 0; k < 6; k++) {
                        app.Tap(AddButton());
                        if (app.Query(c => c.Text("Cannot add any more records for the day!")).Length > 0 && k == 5)
                            continue;
                        Assert.AreNotEqual(5, k, "Didn't find the toast for max records made during Labor generation.");
                        for (int l = 0; l < k; l++) app.ScrollDown();
                        app.Tap(c => c.Id(app.Query(d => d.Text("Description")).Last().Id));
                        RecordBuild(new List<string>() { "Description", "Times" });
                    }
                    app.WaitForElement(AddButton());
                    app.Back();

                    // Check if we made the right number of labor records
                    SeekToElement("LaborCount", SeekDirection.Down);
                    Assert.AreEqual("5 entries", app.Query("LaborCount").FirstOrDefault().Text, "Didn't make 5 entries for Labor records.");
                    #endregion

                    #region Service Records
                    // Spawn some service records
                    SeekToElement("ServiceBox", SeekDirection.Down);
                    app.Tap("ServiceBox");

                    for (int k = 0; k < 6; k++) {
                        app.Tap(AddButton());
                        if (app.Query(c => c.Text("Cannot add any more records for the day!")).Length > 0 && k == 5)
                            continue;
                        Assert.AreNotEqual(5, k, "Didn't find the toast for max records made during Service generation.");
                        for (int l = 0; l < k; l++) app.ScrollDown();
                        app.Tap(c => c.Id(app.Query(d => d.Text("Type")).Last().Id));
                        if (k == 4) {
                            RecordBuild(new List<string>() { "TypeService", "DescriptionOpt", "Cost" }, rng.Next(1,4));
                        } else {
                            RecordBuild(new List<string>() { "TypeService", "DescriptionOpt", "Cost" }, k);
                        }
                    }
                    app.WaitForElement(AddButton());
                    app.Back();

                    // Check if we made the right number of service records
                    SeekToElement("ServiceCount", SeekDirection.Down);
                    Assert.AreEqual("5 entries", app.Query("ServiceCount").FirstOrDefault().Text, "Didn't make 5 entries for Service records.");
                    #endregion

                    #region Expense Records
                    // Spawn some expense records
                    SeekToElement("ExpenseBox", SeekDirection.Down);
                    app.Tap("ExpenseBox");

                    for (int k = 0; k < 6; k++) {
                        app.Tap(AddButton());
                        if (app.Query(c => c.Text("Cannot add any more records for the day!")).Length > 0 && k == 5)
                            continue;
                        Assert.AreNotEqual(5, k, "Didn't find the toast for max records made during Expense generation.");
                        for (int l = 0; l < k; l++) app.ScrollDown();
                        app.Tap(c => c.Id(app.Query(d => d.Text("Description")).Last().Id));
                        RecordBuild(new List<string>() { "Description", "Cost" });
                    }
                    app.WaitForElement(AddButton());
                    app.Back();
                    
                    // Check if we made the right number of service records
                    SeekToElement("ExpenseCount", SeekDirection.Down);
                    Assert.AreEqual("5 entries", app.Query("ExpenseCount").FirstOrDefault().Text, "Didn't make 5 entries for Service records.");
                    #endregion

                    app.Back();
                }

                app.Screenshot("After record generation, " + app.Query("MonthNameLabel").FirstOrDefault().Text);
                app.WaitForElement("MonthForwardButton");
                app.Tap("MonthForwardButton");
            }
        }
    }
}

