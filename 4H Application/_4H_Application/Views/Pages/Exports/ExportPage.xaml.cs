//
// _4H_Application.Views.Pages.Exports.ExportPage.xaml.cs: Method to export
//   horse information to user's recordbooks.
//
// Author(s):
//   Joel Krause (jkjk8080@gmail.com)
//   Michael Okobi (mikaelobos@gmail.com)
//

using _4H_Application.Models.Horses;
using _4H_Application.Models.Records;
using _4H_Application.Models.Projects;
using _4H_Application.Models.Exports;
using _4H_Application.Data;
using _4H_Application.Models.Settings;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using _4H_Application.Models.Pictures;

namespace _4H_Application.Views.Pages.Exports
{

    /// <summary>
    /// Entry page for exporting all of the record books.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExportPage : ContentPage
    {

        /// <summary>
        /// Default constructor for the page.
        /// </summary>
        public ExportPage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Method that is called when the test button is clicked.
        /// </summary>
        /// <param name="sender">Object that sent the click event.</param>
        /// <param name="e">Arguments for the click event.</param>
        private async void TestExportButton_Clicked(object sender, EventArgs e)
        {
            var db = AppDatabase.GetInstance(); // gets the database
            Horse horse = HorseManager.GetInstance().ActiveHorse; //selects the active horse
            int currentYear = (DateTime.Now.Month < 7) ?
                DateTime.Now.Year - 1 : DateTime.Now.Year;

            if (horse == null) //determines if an active horse is selected
            {
                return;
            }

            // Test the PdfSharp.Xamarin functionality
            IFileHelper director = DependencyService.Get<IFileHelper>();
            IExportHelper helper = DependencyService.Get<IExportHelper>();

            //establishes the 4-H document path
            string path = director.GenerateLocalAsset("level1book.pdf", horse.Name);
            if (!IsFirstLevel())
            {
                path = director.GenerateLocalAsset("level2-5book.pdf", horse.Name);
            }

            //opens the document for modification
            PdfDocument document = PdfReader.Open(path, PdfDocumentOpenMode.Modify);

            // gets the document's form fillable components
            PdfAcroForm acroForm = document.AcroForm;

            #region Photos
            // get Photos from Picture manager and print onto pages 11&12
            // ***supports upto 6 currently***
            List<Picture> pics = PictureManager.Instance.Pictures;
            int len = 6;
            if (pics.Count < len)
            {
                len = pics.Count;
            }
            int pageNo = 10;
            double[,] poses = new double[,] { 
                { 1, 5 }, { 4.25, 5 },
                { 1, 10 }, { 4.25, 10 },
                { 1, 5.5 }, { 4.25, 5.5 } };
            for (int i = 0; i < len; i++)
            {
                XImage img = XImage.FromFile(pics[i].FilePath);
                
                double xPos = poses[i, 0] * 71;
                double yPos = (11 * 71) - (poses[i, 1] * 71);

                if (i >= 2)
                {
                    pageNo = 11;
                }
                XGraphics gfx = XGraphics.FromPdfPage(document.Pages[pageNo]);

                // each photo has up to 3.25 in by 4 in
                // 71 pixels per inch for pdfsharp stuff
                double maxWidth = 3.25 * 71, maxHeight = 4 * 71;
                var ratioX = (double)maxWidth / img.PixelWidth;
                var ratioY = (double)maxHeight / img.PixelHeight;
                var ratio = Math.Min(ratioX, ratioY);

                var newWidth = (int)(img.PixelWidth * ratio);
                var newHeight = (int)(img.PixelHeight * ratio);
                
                gfx.DrawImage(img, xPos, yPos, newWidth, newHeight);
                gfx.Dispose();
            }
            #endregion

            #region Project Plans
            // Sets the project plans page

            var plans = await db.Query<ProjectPlan>(
                "SELECT * FROM ProjectPlan WHERE HorseId = ? AND Year = ?",
                horse.Id, currentYear);
            if (plans.Count > 0) { 
                acroForm.Fields["horseList"].Value = new PdfString(plans[0].HorsePlans);
                acroForm.Fields["careList"].Value = new PdfString(plans[0].CaringPlans);
                acroForm.Fields["experienceList"].Value = new PdfString(plans[0].LearningPlans);
            }
            #endregion

            #region Horse Info
            // Sets the horse info section

            acroForm.Fields["name"].Value = new PdfString(horse.Name);
            acroForm.Fields["breed"].Value = new PdfString(horse.Breed);
            acroForm.Fields["sex"].Value = new PdfString(horse.Sex.ToString());
            acroForm.Fields["height"].Value = new PdfString(horse.Height.ToString());
            acroForm.Fields["weight"].Value = new PdfString(horse.Weight.ToString());
            acroForm.Fields["age"].Value = new PdfString(horse.Age.ToString());
            acroForm.Fields["color"].Value = new PdfString(horse.Color);
            acroForm.Fields["markings"].Value = new PdfString(horse.Markings);

            // add pedigree information for year 1 book
            if(IsFirstLevel()) { 
                acroForm.Fields["pedigree"].Value = new PdfString(horse.Pedigree);
                acroForm.Fields["sire"].Value = new PdfString(horse.Sire);
                acroForm.Fields["dam"].Value = new PdfString(horse.Dam);
                acroForm.Fields["paternalGrandSire"].Value = new PdfString(horse.PaternalGrandSire);
                acroForm.Fields["paternalGrandDam"].Value = new PdfString(horse.PaternalGrandDam);
                acroForm.Fields["maternalGrandSire"].Value = new PdfString(horse.MaternalGrandSire);
                acroForm.Fields["maternalGrandDam"].Value = new PdfString(horse.MaternalGrandDam);
            }

            #endregion

            #region Activities
            // Sets the Participation in activities section

            // set the upper limit on allowed activities
            int activityLimit = (IsFirstLevel()) ? 8 : 11;

            // get the activity records
            var activities = await RecordBookEntry.GetInRange<ActivityRecordEntry>(
                horse.Id, new DateTime(currentYear, 7, 1), new DateTime(currentYear + 1, 6, 30));

            // populate the activites until you hit the limit or you run out of activities
            for(int i = 0; i < activityLimit && i < activities.Count; i++) {
                acroForm.Fields["activityDate" + i].Value = new PdfString(activities[i].Date.ToString());
                acroForm.Fields["activity" + i].Value = new PdfString(activities[i].Description);
                acroForm.Fields["activityLocation" + i].Value = new PdfString(activities[i].Location);
            }

            #endregion
    
            #region Stable, Feed, Other
            // In book 1: fills out the Stable Record page
            // In book 2: fills out the Feed record page, the Stable record page, and the Other Expenses section

            int yearCrossover = 0; // helps the program jump the gap from Dec to Jan

            // Year-long aggregates
            float book1OverallCost = 0;
            float book2OverallFeedCost = 0;
            float book2OverallStableCost = 0;
            float overallBedWeight = 0;
            float overallBedCost = 0;
            var overallFeedAmounts = new Dictionary<FeedType, float> {
                { FeedType.Grain, 0 },
                { FeedType.Hay, 0 },
                { FeedType.Pasture, 0 },
                { FeedType.SaltAndMinerals, 0 }
            };
            var overallFeedCosts = new Dictionary<FeedType, float> {
                { FeedType.Grain, 0 },
                { FeedType.Hay, 0 },
                { FeedType.Pasture, 0 },
                { FeedType.SaltAndMinerals, 0 },
                { FeedType.Other, 0 }
            };
            float overallOtherExpenses = 0;
            var overallServiceCosts = new Dictionary<ServiceType, float> {
                { ServiceType.Farrier, 0 },
                { ServiceType.HealthCare, 0 },
                { ServiceType.RidingInstruction, 0 },
                { ServiceType.Other, 0 },
            };
            TimeSpan overallLaborHours = new TimeSpan(0, 0, 0);

            for (int j = 6; j < 18; j++)
            {
                int index = (j % 12);
                if (index == 0) yearCrossover = 1;

                // get current month's first/last date
                var firstOfTheMonth = new DateTime(currentYear + yearCrossover, index+1, 1);
                var lastOfTheMonth = firstOfTheMonth.AddMonths(1).AddDays(-1);
                string fullMonthName = new DateTime(2015, index+1, 1).ToString("MMMM");

                // monthly aggregate values
                float book1TotalCost = 0;
                float book2TotalFeedCost = 0;
                float book2TotalStableCost = 0;

                #region Bedding
                // get the bedding records
                var bedding = await RecordBookEntry.GetInRange<BeddingRecordEntry>(
                    horse.Id, firstOfTheMonth, lastOfTheMonth);

                // Aggregate and export bedding records
                var totalBeddingWeight = new Dictionary<BeddingType, float> {
                    { BeddingType.Hay, 0 },
                    { BeddingType.Hemp, 0 },
                    { BeddingType.Moss, 0 },
                    { BeddingType.Paper, 0 },
                    { BeddingType.Sawdust, 0 },
                    { BeddingType.Shavings, 0 },
                    { BeddingType.StallMats, 0 },
                    { BeddingType.Straw, 0 },
                    { BeddingType.WoodPellets, 0 }
                };
                float totalBeddingCost = 0;
                foreach(BeddingRecordEntry bed in bedding) {
                    totalBeddingWeight[bed.Type] += bed.Amount;
                    totalBeddingCost += bed.Cost;
                    overallBedWeight += bed.Amount;
                    overallBedCost += bed.Cost;
                    book1TotalCost += bed.Cost;
                    book2TotalStableCost += bed.Cost;
                }

                // format the kind/amount string
                string weightString = "";
                if (totalBeddingWeight[BeddingType.Hay] > 0) {
                    weightString = String.Format("{0}Hay, {1}\n", weightString, totalBeddingWeight[BeddingType.Hay]);
                }
                if (totalBeddingWeight[BeddingType.Hemp] > 0) {
                    weightString = String.Format("{0}Hemp, {1}\n", weightString, totalBeddingWeight[BeddingType.Hemp]);
                }
                if (totalBeddingWeight[BeddingType.Moss] > 0) {
                    weightString = String.Format("{0}Moss, {1}\n", weightString, totalBeddingWeight[BeddingType.Moss]);
                }
                if (totalBeddingWeight[BeddingType.Paper] > 0) {
                    weightString = String.Format("{0}Paper, {1}\n", weightString, totalBeddingWeight[BeddingType.Paper]);
                }
                if (totalBeddingWeight[BeddingType.Sawdust] > 0) {
                    weightString = String.Format("{0}Sawdust, {1}\n", weightString, totalBeddingWeight[BeddingType.Sawdust]);
                }
                if (totalBeddingWeight[BeddingType.Shavings] > 0) {
                    weightString = String.Format("{0}Shavings, {1}\n", weightString, totalBeddingWeight[BeddingType.Shavings]);
                }
                if (totalBeddingWeight[BeddingType.StallMats] > 0) {
                    weightString = String.Format("{0}Stall Mats, {1}\n", weightString, totalBeddingWeight[BeddingType.StallMats]);
                }
                if (totalBeddingWeight[BeddingType.Straw] > 0) {
                    weightString = String.Format("{0}Straw, {1}\n", weightString, totalBeddingWeight[BeddingType.Straw]);
                }
                if (totalBeddingWeight[BeddingType.WoodPellets] > 0) {
                    weightString = String.Format("{0}Wood Pellets, {1}\n", weightString, totalBeddingWeight[BeddingType.WoodPellets]);
                }

                // commit this month's records
                acroForm.Fields["bedding" + index].Value = new PdfString(weightString);
                acroForm.Fields["beddingCost" + index].Value = new PdfString(String.Format("{0:C2}", totalBeddingCost));
                #endregion
                
                #region Feed
                // get the feed records
                var feed = await RecordBookEntry.GetInRange<FeedRecordEntry>(
                    horse.Id, firstOfTheMonth, lastOfTheMonth);

                // Aggregate the records
                var totalFeedAmounts = new Dictionary<FeedType, float> {
                    { FeedType.Grain, 0 },
                    { FeedType.Hay, 0 },
                    { FeedType.Pasture, 0 },
                    { FeedType.SaltAndMinerals, 0 }
                };
                var totalFeedCosts = new Dictionary<FeedType, float> {
                    { FeedType.Grain, 0 },
                    { FeedType.Hay, 0 },
                    { FeedType.Pasture, 0 },
                    { FeedType.SaltAndMinerals, 0 },
                    { FeedType.Other, 0 }
                };
                foreach(FeedRecordEntry food in feed) {
                    if (food.Type != FeedType.Other) {
                        totalFeedAmounts[food.Type] += food.Amount;
                        overallFeedAmounts[food.Type] += food.Amount;
                    }
                    totalFeedCosts[food.Type] += food.Cost;
                    overallFeedCosts[food.Type] += food.Cost;
                    book1OverallCost += food.Cost;
                    book2OverallFeedCost += food.Cost;
                }

                // commit this month's feed records
                acroForm.Fields["grain" + index].Value = new PdfString(totalFeedAmounts[FeedType.Grain].ToString());
                acroForm.Fields["grainCost" + index].Value = new PdfString(String.Format("{0:C2}", totalFeedCosts[FeedType.Grain]));
                acroForm.Fields["hay" + index].Value = new PdfString(totalFeedAmounts[FeedType.Hay].ToString());
                acroForm.Fields["hayCost" + index].Value = new PdfString(String.Format("{0:C2}", totalFeedCosts[FeedType.Hay]));
                acroForm.Fields["salt" + index].Value = new PdfString(totalFeedAmounts[FeedType.SaltAndMinerals].ToString());
                acroForm.Fields["saltCost" + index].Value = new PdfString(String.Format("{0:C2}", totalFeedCosts[FeedType.SaltAndMinerals]));
                acroForm.Fields["pasture" + index].Value = new PdfString(totalFeedAmounts[FeedType.Pasture].ToString());

                // these do not go into tier 1 record book
                if (!IsFirstLevel()) {
                    acroForm.Fields["pastureCost" + index].Value = new PdfString(String.Format("{0:C2}", totalFeedCosts[FeedType.Pasture]));
                    acroForm.Fields["otherFeedCost" + index].Value = new PdfString(String.Format("{0:C2}", totalFeedCosts[FeedType.Other]));
                }
                #endregion

                #region Service
                // get the service records
                var service = await RecordBookEntry.GetInRange<ServiceRecordEntry>(
                    horse.Id, firstOfTheMonth, lastOfTheMonth);

                // aggregate the service record information
                string otherExpenses = "";
                float otherExpensesCost = 0;
                var serviceStrings = new Dictionary<ServiceType, string> {
                    { ServiceType.Farrier, "" },
                    { ServiceType.HealthCare, "" },
                    { ServiceType.RidingInstruction, "" },
                    { ServiceType.Other, "" },
                };
                var serviceCosts = new Dictionary<ServiceType, float> {
                    { ServiceType.Farrier, 0 },
                    { ServiceType.HealthCare, 0 },
                    { ServiceType.RidingInstruction, 0 },
                    { ServiceType.Other, 0 },
                };

                foreach (ServiceRecordEntry serve in service) { 
                    // fills in "other expenses" for a year 1 book
                    if(IsFirstLevel()) {
                        if(!serve.Description.Equals(String.Empty))
                            otherExpenses = String.Format("{0}; {1}", otherExpenses, serve.Description);
                        otherExpensesCost += serve.Cost;
                        overallOtherExpenses += serve.Cost;
                    } else { // fills in "Stable Record" for a year 2-5 book
                        if(!serve.Description.Equals(String.Empty))
                            serviceStrings[serve.Type] = String.Format(
                                "{0}; {1}", serviceStrings[serve.Type], serve.Description);
                        serviceCosts[serve.Type] += serve.Cost;
                        overallServiceCosts[serve.Type] += serve.Cost;
                    }
                    book1OverallCost += serve.Cost;
                    book2OverallStableCost += serve.Cost;
                }
                
                // commit the service record information to the pdf
                if(IsFirstLevel()) {
                    acroForm.Fields["otherExpenses" + index].Value = new PdfString(otherExpenses);
                    acroForm.Fields["otherExpensesCost" + index].Value = new PdfString(String.Format("{0:C2}", otherExpensesCost));
                } else {
                    acroForm.Fields["ridingCost" + index].Value = new PdfString(
                        String.Format("{0}, {1:C2}",
                        serviceStrings[ServiceType.RidingInstruction],
                        serviceCosts[ServiceType.RidingInstruction]));
                    acroForm.Fields["healthCare" + index].Value = new PdfString(serviceStrings[ServiceType.HealthCare]);
                    acroForm.Fields["healthCareCost" + index].Value = new PdfString(
                        String.Format("{0:C2}", serviceCosts[ServiceType.HealthCare]));
                    acroForm.Fields["farrier" + index].Value = new PdfString(serviceStrings[ServiceType.Farrier]);
                    acroForm.Fields["farrierCost" + index].Value = new PdfString(
                        String.Format("{0:C2}", serviceCosts[ServiceType.Farrier]));
                    acroForm.Fields["equipment" + index].Value = new PdfString(serviceStrings[ServiceType.Other]);
                    acroForm.Fields["equipmentCost" + index].Value = new PdfString(
                        String.Format("{0:C2}", serviceCosts[ServiceType.Other]));
                }

                #endregion
                
                #region Riding
                // Get the riding records
                var rides = await RecordBookEntry.GetInRange<RidingRecordEntry>(
                    horse.Id, firstOfTheMonth, lastOfTheMonth);

                // set the riding month if the book level is 2-5
                if(!IsFirstLevel()) {
                    acroForm.Fields["ridingMonth" + index].Value = new PdfString(fullMonthName);
                }

                // aggregate riding hours and descriptions
                TimeSpan rideHours = new TimeSpan(0, 0, 0);
                string rideDescriptions = "";
                foreach (RidingRecordEntry ride in rides) {
                    rideHours.Add(ride.End.Subtract(ride.Start));
                    if(!ride.Description.Equals(String.Empty))
                        rideDescriptions = String.Format("{0}; {1}", rideDescriptions, ride.Description);
                }

                // commit aggregates
                acroForm.Fields["ridingHours" + index].Value = new PdfString(rideHours.ToString(@"hh\:mm"));
                acroForm.Fields["riding" + index].Value = new PdfString(rideDescriptions);
                #endregion

                #region Labor
                // Get the labor records
                var labor = await RecordBookEntry.GetInRange<LaborRecordEntry>(
                    horse.Id, firstOfTheMonth, lastOfTheMonth);

                // aggregate labor hours and descriptions
                // aggregate riding hours and descriptions
                TimeSpan laborHours = new TimeSpan(0, 0, 0);
                string laborDescriptions = "";
                foreach (LaborRecordEntry job in labor)
                {
                    laborHours.Add(job.End.Subtract(job.Start));
                    if (!job.Description.Equals(String.Empty))
                        laborDescriptions = String.Format("{0}; {1}", laborDescriptions, job.Description);
                }

                // commit aggregates
                if (IsFirstLevel()) {
                    acroForm.Fields["labor" + index].Value = new PdfString(laborDescriptions);
                    acroForm.Fields["laborHours" + index].Value = new PdfString(laborHours.ToString(@"hh\:mm"));
                } else {
                    acroForm.Fields["labor" + index].Value = new PdfString(laborHours.ToString(@"hh\:mm"));
                }
                #endregion

                #region monthly aggregate commit
                // Commit any aggregated totals for the current month
                if (IsFirstLevel()) {
                    acroForm.Fields["totalCost" + index].Value = new PdfString(
                        String.Format("{0:C2}", book1TotalCost));
                } else {
                    acroForm.Fields["totalFeedCost" + index].Value = new PdfString(
                        String.Format("{0:C2}", book2TotalFeedCost));
                    acroForm.Fields["totalFeedCostS" + index].Value = new PdfString(
                        String.Format("{0:C2}", book2TotalFeedCost));
                    acroForm.Fields["totalStableCost" + index].Value = new PdfString(
                        String.Format("{0:C2}", book2TotalStableCost));
                }
                #endregion

                #region aggregate yearly
                // Add aggregate monthly values to their respective year-long totals
                book1OverallCost += book1TotalCost;
                book2OverallFeedCost += book2TotalFeedCost;
                book2OverallStableCost += book2TotalStableCost;
                overallBedCost += totalBeddingCost;
                overallBedWeight += totalBeddingWeight[BeddingType.Hay];
                overallBedWeight += totalBeddingWeight[BeddingType.Hemp];
                overallBedWeight += totalBeddingWeight[BeddingType.Moss];
                overallBedWeight += totalBeddingWeight[BeddingType.Paper];
                overallBedWeight += totalBeddingWeight[BeddingType.Sawdust];
                overallBedWeight += totalBeddingWeight[BeddingType.Shavings];
                overallBedWeight += totalBeddingWeight[BeddingType.StallMats];
                overallBedWeight += totalBeddingWeight[BeddingType.Straw];
                overallBedWeight += totalBeddingWeight[BeddingType.WoodPellets];
                overallFeedAmounts[FeedType.Grain] += totalFeedAmounts[FeedType.Grain];
                overallFeedAmounts[FeedType.Hay] += totalFeedAmounts[FeedType.Hay];
                overallFeedAmounts[FeedType.Pasture] += totalFeedAmounts[FeedType.Pasture];
                overallFeedAmounts[FeedType.SaltAndMinerals] += totalFeedAmounts[FeedType.SaltAndMinerals];
                overallFeedCosts[FeedType.Grain] += totalFeedCosts[FeedType.Grain];
                overallFeedCosts[FeedType.Hay] += totalFeedCosts[FeedType.Hay];
                overallFeedCosts[FeedType.Pasture] += totalFeedCosts[FeedType.Pasture];
                overallFeedCosts[FeedType.SaltAndMinerals] += totalFeedCosts[FeedType.SaltAndMinerals];
                overallFeedCosts[FeedType.Other] += totalFeedCosts[FeedType.Other];
                overallLaborHours.Add(laborHours);
                #endregion
                
            }

            
            #region Year long aggregates commit
            // Commit aggragated totals for the year

            // Universal to both books
            acroForm.Fields["beddingTotal"].Value = new PdfString(overallBedWeight.ToString());
            acroForm.Fields["beddingCostTotal"].Value = new PdfString(
                String.Format("{0:C2}", overallBedCost));

            if (IsFirstLevel()) { // first book only
                acroForm.Fields["hayTotal"].Value = new PdfString(
                    overallFeedAmounts[FeedType.Hay].ToString());
                acroForm.Fields["hayCostTotal"].Value = new PdfString(
                    String.Format("{0:C2}", overallFeedCosts[FeedType.Hay]));
                acroForm.Fields["grainTotal"].Value = new PdfString(
                    overallFeedAmounts[FeedType.Grain].ToString());
                acroForm.Fields["grainCostTotal"].Value = new PdfString(
                    String.Format("{0:C2}", overallFeedCosts[FeedType.Grain]));
                acroForm.Fields["saltTotal"].Value = new PdfString(
                    overallFeedAmounts[FeedType.SaltAndMinerals].ToString());
                acroForm.Fields["saltCostTotal"].Value = new PdfString(
                    String.Format("{0:C2}", overallFeedCosts[FeedType.SaltAndMinerals]));
                acroForm.Fields["pastureTotal"].Value = new PdfString(
                    overallFeedCosts[FeedType.Pasture].ToString());
                acroForm.Fields["otherExpensesCostTotal"].Value = new PdfString(
                    String.Format("{0:C2}", overallOtherExpenses));
                acroForm.Fields["totalCostTotal"].Value = new PdfString(
                    String.Format("{0:C2}", book1OverallCost));
            } else { // second book only
                acroForm.Fields["totalFeedCostTotal"].Value = new PdfString(
                    String.Format("{0:C2}", book2OverallFeedCost));
                acroForm.Fields["totalFeedCostSTotal"].Value = new PdfString(
                    String.Format("{0:C2}", book2OverallFeedCost));
                acroForm.Fields["laborTotal"].Value = new PdfString(
                    overallLaborHours.ToString(@"hh\:mm"));
                acroForm.Fields["ridingCostTotal"].Value = new PdfString(
                    String.Format("{0:C2}", overallServiceCosts[ServiceType.RidingInstruction]));
                acroForm.Fields["healthCareCostTotal"].Value = new PdfString(
                    String.Format("{0:C2}", overallServiceCosts[ServiceType.HealthCare]));
                acroForm.Fields["farrierCostTotal"].Value = new PdfString(
                    String.Format("{0:C2}", overallServiceCosts[ServiceType.Farrier]));
                acroForm.Fields["equipmentCostTotal"].Value = new PdfString(
                    String.Format("{0:C2}", overallServiceCosts[ServiceType.Other]));
                acroForm.Fields["totalStableCostTotal"].Value = new PdfString(
                    String.Format("{0:C2}", book2OverallStableCost));
            }
            #endregion

            #region Other Expenses (book 2 only)
            if (!IsFirstLevel()) {
                // Get the expense records
                var expense = await RecordBookEntry.GetInRange<ExpenseRecordEntry>(
                    horse.Id, new DateTime(currentYear, 7, 1), new DateTime(currentYear + 1, 6, 30));
                float otherExpensesTotal = 0;

                // Commit the extra expenses to the record book
                for (int i = 0; i < 6 && i < expense.Count; i++)
                {
                    acroForm.Fields["otherDate" + i].Value = new PdfString(expense[i].Date.ToString());
                    acroForm.Fields["other" + i].Value = new PdfString(expense[i].Description);
                    acroForm.Fields["otherCost" + i].Value = new PdfString(String.Format("{0:C2", expense[i].Cost));
                    otherExpensesTotal += expense[i].Cost;
                }
                acroForm.Fields["otherCostTotal"].Value = new PdfString(String.Format("{0:C2}", otherExpensesTotal));
            }
            #endregion

            #endregion

            #region Experiences
            // get the project experiences
            var experiences = await db.Query<ProjectExperiences>(
                "SELECT * FROM ProjectExperiences WHERE HorseId = ? AND Year = ?",
                horse.Id, currentYear);

            // if there are experiences, set them into the PDF
            if (experiences.Count > 0) {
                acroForm.Fields["feedSummary"].Value = new PdfString(experiences[0].FeedCareExperiences);
                acroForm.Fields["healthSummary"].Value = new PdfString(experiences[0].HealthExperiences);
                acroForm.Fields["learnSummary"].Value = new PdfString(experiences[0].LearningExperiences);
                acroForm.Fields["goalSummary"].Value = new PdfString(experiences[0].GoalsExperiences);
                if(IsFirstLevel())
                    acroForm.Fields["otherSummary"].Value = new PdfString(experiences[0].OtherExperiences);
            }
            #endregion

            #region Story
            // get the project story
            var story = await db.Query<ProjectStory>(
                "SELECT * FROM ProjectStory WHERE HorseId = ? AND Year = ?",
                horse.Id, currentYear);
            if (story.Count > 0)
                acroForm.Fields["projectStory"].Value = new PdfString(story[0].Story);
            #endregion

            #region User Information
            acroForm.Fields["userName"].Value = new PdfString(
                AppSettings.User.UserName);
            var today = DateTime.Today;
            var age = today.Year - AppSettings.User.DateOfBirth.Year;
            if (AppSettings.User.DateOfBirth > today.AddYears(-age)) age--;
            acroForm.Fields["userAge"].Value = new PdfString(age.ToString());
            acroForm.Fields["userBirthday"].Value = new PdfString(
                AppSettings.User.DateOfBirth.ToString("MM/dd/yyyy"));
            acroForm.Fields["userAddress0"].Value = new PdfString(
                AppSettings.User.Address);
            acroForm.Fields["county"].Value = new PdfString(
                AppSettings.HorseClub.County);
            acroForm.Fields["clubName"].Value = new PdfString(
                AppSettings.HorseClub.ClubName);
            acroForm.Fields["clubLeaderName"].Value = new PdfString(
                AppSettings.HorseClub.LeaderName);
            acroForm.Fields["projectHelperName"].Value = new PdfString(
                AppSettings.HorseProgram.HelperName);
            acroForm.Fields["startDate"].Value = new PdfString(
                AppSettings.HorseProgram.StartDate.ToString("MM/dd/yyyy"));
            acroForm.Fields["endDate"].Value = new PdfString(
                AppSettings.HorseProgram.CloseDate.ToString("MM/dd/yyyy"));
                
            #endregion

            // Save the file to the device document directory
            string file = Path.Combine(helper.GetDocumentsDirectory(), horse.Name + "4-HRecordBook.pdf");
            document.Save(file);
            helper.SendEmail(file);

            // Set the filepath of the WebView to the new PDF
            //this.PdfView.Path = file;

        }

        private bool IsFirstLevel()
        {
            return (AppSettings.HorseProgram.Level == AppSettings.HorseProgram.ProgramLevel.First);
        }

    }

}