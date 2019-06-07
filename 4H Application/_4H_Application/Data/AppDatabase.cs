//
// _4H_Application.Data.Database.cs: Class that serves as a facade for the
//   application's database connection.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Models.Horses;
using _4H_Application.Models.Pictures;
using _4H_Application.Models.Projects;
using _4H_Application.Models.Records;
using _4H_Application.Models.Settings;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace _4H_Application.Data {

    /// <summary>
    /// Facade for the database connection of the application.
    /// </summary>
    public class AppDatabase {

        /// <summary>
        /// Reference to the filename of the database.
        /// </summary>
        public static string DatabaseFilename {
            get; set; 
        } = "4hAppDb-v1.0.db3";


        /// <summary>
        /// Private instance of the application database facade.
        /// </summary>
        private static AppDatabase instance;

        /// <summary>
        /// Method to get the singleton instance of the database connection.
        /// </summary>
        /// <returns>The singleton database instance.</returns>
        public static AppDatabase GetInstance() {

            // Check if the instance of the database has not yet been
            // instantiated.
            if (instance == null) {

                // If the instance is not instantiated, create a new instance
                // with the specified file path to the database.
                AppDatabase.instance = new AppDatabase(
                    DependencyService.Get<IFileHelper>()
                    .GetLocalFilePath(AppDatabase.DatabaseFilename));
                AppDatabase.instance.Initialize();

            }

            // Return the singleton instance of the database connection.
            return AppDatabase.instance;

        }

        /// <summary>
        /// Private connection to the database.
        /// </summary>
        private readonly SQLiteAsyncConnection connection;

        /// <summary>
        /// Default constructor for the application's database.
        /// </summary>
        /// <param name="path">Path to the specified database.</param>
        private AppDatabase(string path) {
            this.connection = new SQLiteAsyncConnection(path);
        }

        /// <summary>
        /// Method to initialize the database with all of its tables.
        /// </summary>
        private void Initialize() {

            // Create the table for the horse
            this.connection.CreateTableAsync<Horse>().Wait();

            // Create the tables for the horse record book
            this.connection.CreateTableAsync<ActivityRecordEntry>().Wait();
            this.connection.CreateTableAsync<RidingRecordEntry>().Wait();
            this.connection.CreateTableAsync<FeedRecordEntry>().Wait();
            this.connection.CreateTableAsync<BeddingRecordEntry>().Wait();
            this.connection.CreateTableAsync<LaborRecordEntry>().Wait();
            this.connection.CreateTableAsync<ServiceRecordEntry>().Wait();
            this.connection.CreateTableAsync<ExpenseRecordEntry>().Wait();

            // Create the tables for the project record
            this.connection.CreateTableAsync<ProjectPlan>().Wait();
            this.connection.CreateTableAsync<ProjectExperiences>().Wait();
            this.connection.CreateTableAsync<ProjectStory>().Wait();

            // Create the tables for the pictures
            this.connection.CreateTableAsync<Picture>().Wait();

        }

        /// <summary>
        /// Method to execute a specified query.
        /// </summary>
        /// <typeparam name="T">The return type of the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="args">Any arguments to the query.</param>
        /// <returns>A list of the results of the query.</returns>
        public Task<List<T>> Query<T>(string query, params object[] args)
            where T : DatabaseEntry, new() {

            // Execute the specified query with the specified arguments
            return this.connection.QueryAsync<T>(query, args);

        }

        /// <summary>
        /// Method to get the specified entry by its Id and Type.
        /// </summary>
        /// <typeparam name="T">The type of the entry.</typeparam>
        /// <param name="Id">The Id of the entry.</param>
        /// <returns>The specified entry, given it exists.</returns>
        public Task<T> Get<T>(int Id)
            where T : DatabaseEntry, new() {

            // Get the specified element by its Id and return it.
            return this.connection.Table<T>().Where(
                entry => entry.Id == Id).FirstOrDefaultAsync();

        }

        /// <summary>
        /// Method to get all of the entries available to the user by its type.
        /// </summary>
        /// <returns>A list of all the entries by their type.</returns>
        public Task<List<T>> GetAll<T>()
            where T : DatabaseEntry, new() {

            // Get the specified collection elements by their type.
            return this.connection.Table<T>().ToListAsync();

        }

        /// <summary>
        /// Method to save or insert an element into the database.
        /// </summary>
        /// <param name="entry">The entry to insert or save.</param>
        /// <param name="restore">If set to true, will insert regardless of index</param>
        /// <returns>An indication of if the entry was saved.</returns>
        public Task<int> Save<T>(T entry, bool restore = false)
            where T : DatabaseEntry, new() {

            // If the item doesn't currently exist in the database, save it as
            // a new entry.
            if (entry.Id == -1 || restore) {
                return this.connection.InsertAsync(entry);
            }

            // Otherwise, update the current entry in the database.
            else {
                return this.connection.UpdateAsync(entry);
            }

        }

        /// <summary>
        /// Method to delete the specified entry from the database.
        /// </summary>
        /// <param name="entry">The entry to remove from the database.</param>
        /// <returns>An indication of if the entry was deleted.</returns>
        public Task<int> Delete<T>(T entry)
            where T : DatabaseEntry, new() {

            // Delete the specified entry from the database.
            return this.connection.DeleteAsync(entry);

        }

        /// <summary>
        /// Method to delete EVERYTHING in the database.
        /// DO NOT USE OUTSIDE OF THE BACKUP MANAGER.
        /// </summary>
        /// <returns></returns>
        public void DeleteAll()
        {
            // Delete the table for the horse
            this.connection.DropTableAsync<Horse>().Wait();

            // Create the tables for the horse record book
            this.connection.DropTableAsync<ActivityRecordEntry>().Wait();
            this.connection.DropTableAsync<RidingRecordEntry>().Wait();
            this.connection.DropTableAsync<FeedRecordEntry>().Wait();
            this.connection.DropTableAsync<BeddingRecordEntry>().Wait();
            this.connection.DropTableAsync<LaborRecordEntry>().Wait();
            this.connection.DropTableAsync<ServiceRecordEntry>().Wait();
            this.connection.DropTableAsync<ExpenseRecordEntry>().Wait();

            // Create the tables for the project record
            this.connection.DropTableAsync<ProjectPlan>().Wait();
            this.connection.DropTableAsync<ProjectExperiences>().Wait();
            this.connection.DropTableAsync<ProjectStory>().Wait();

            Initialize();
        }

    }

}
