//
// _4H_Application.Models.Horses.HorseManager.cs: Class that represents a
//   singleton entity that manages all of the user's horses.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Data;
using _4H_Application.Models.Horses.Exceptions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace _4H_Application.Models.Horses {

    /// <summary>
    /// Class that serves as a manager for all of the user's horses.
    /// </summary>
    /// This class follows the singleton design pattern, so it is always
    /// available in the same state regardless of where it is being used, has
    /// methods for serializing and deserializing horse data from XML, and
    /// serves as a general facade for all other classes that will use horses.
    public class HorseManager {

        /// <summary>
        /// Reference to the maximum amount of horses for the application.
        /// </summary>
        public const int MAX_HORSES = 20;

        /// <summary>
        /// Private, singleton instance of the HorseManager.
        /// </summary>
        private static HorseManager instance = null;

        /// <summary>
        /// Method that gets the singleton instance of the HorseManager.
        /// </summary>
        /// <returns>The singleton instance of the HorseManager.</returns>
        public static HorseManager GetInstance() {

            // Create the HorseManager if it has not yet been created.
            if (HorseManager.instance == null) {
                HorseManager.instance = new HorseManager();
            }

            // Return the singleton HorseManager instance
            return HorseManager.instance;

        }

        /// <summary>
        /// Private list of the available horses, used for modifying the 
        /// contents of the array.
        /// </summary>
        public ObservableCollection<Horse> Horses {
            get; set;
        } = null;

        /// <summary>
        /// Reference to the currently active horse.
        /// </summary>
        public Horse ActiveHorse {
            get; set;
        } = null;

        /// <summary>
        /// Default constructor for the HorseManager.
        /// </summary>
        private HorseManager() {

            // Initialize the list of horses
            this.Horses = new ObservableCollection<Horse>();
            Task.Run(async () => {
                await this.LoadHorses();
            });

        }

        /// <summary>
        /// Method to get the specified horse by its id.
        /// </summary>
        /// <param name="id">The ID of the horse.</param>
        /// <returns>The requested horse, or null if none were found.</returns>
        /// <exception cref="HorseNotFoundException">Thrown if the horse does not exist.</exception>
        public Horse GetHorseById(int id) {

            // Iterate through each of the horses - if a match is found then
            // return it to the caller
            foreach (Horse horse in this.Horses) {
                if (horse.Id == id) return horse;
            }

            // If no horse was found, return null
            throw new HorseNotFoundException();

        }

        /// <summary>
        /// Method to create a new horse in the horse manager.
        /// </summary>
        /// <returns>The new horse that has been created.</returns>
        /// <exception cref="TooManyHorsesException">Thrown if there are too many horses.</exception>
        public async Task<Horse> Create(HorseType type) {

            // Check if the maximum number of horses is already met
            if (this.Horses.Count == HorseManager.MAX_HORSES) {
                throw new TooManyHorsesException();
            }

            // Create a new horse entity with default values and add it to the
            // database (this will give it an Id automatically).
            Horse horse = new Horse() {
                Type = type,
            };
            await AppDatabase.GetInstance().Save<Horse>(horse);

            // Add the horse to the list of horses and set it as the active
            // horse in the application.
            this.Horses.Add(horse);
            this.ActiveHorse = horse;

            // Return the new horse
            return horse;

        }

        /// <summary>
        /// Method to remove the specified horse from the list.
        /// </summary>
        /// <param name="horse">The horse to remove.</param>
        /// <returns>An indication of task completion.</returns>
        /// <exception cref="HorseNotFoundException">Thrown if the horse doesn't exist.</exception>
        public async Task Delete(Horse horse) {

            // Check if the horse exists in the list of horses
            if (!this.Horses.Contains(horse)) {
                throw new HorseNotFoundException();
            }

            // Delete any references to the current horse
            this.Horses.Remove(horse);
            await AppDatabase.GetInstance().Delete<Horse>(horse);

            // Set the new active horse based on how many horses there are
            if (this.Horses.Count == 0) {
                this.ActiveHorse = null;
            } else {
                this.ActiveHorse = this.Horses[0];
            }

        }

        /// <summary>
        /// Method to load the list of horses from the database.
        /// </summary>
        /// <returns>A void task, for awaiting purposes.</returns>
        private async Task LoadHorses() {

            // Get a list of horses from the database
            List<Horse> horses =
                await AppDatabase.GetInstance().GetAll<Horse>();
            this.Horses = new ObservableCollection<Horse>(horses);

            // Do not set an active horse if there is no presently active
            // horse available.
            if (this.Horses.Count == 0) {
                this.ActiveHorse = null;
                return;
            }

            // If the active horse is currently set to null, set it to the
            // first horse in the list.
            if (this.ActiveHorse == null) {
                this.ActiveHorse = this.Horses[0];
            }

            // If the active horse already exists, make sure that its Id is
            // present in some horse in the list.
            foreach (Horse horse in this.Horses) {
                if (horse.Id == this.ActiveHorse.Id) {
                    return;
                }
            }

            // If this point has been reached, then an invalid Id was set for
            // the active horse and the active horse should be set to the first
            // horse in the list.
            this.ActiveHorse = this.Horses[0];

        }

    }

}
