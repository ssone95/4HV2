//
// _4H_Application.Models.Horses.Horse.cs: Class that represents a horse.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//   Joel Krause (jkjk8080@gmail.com)
//

using SQLite;
using _4H_Application.Data;
using _4H_Application.Models.Settings;

namespace _4H_Application.Models.Horses {

    /// <summary>
    /// Class that represents a horse.
    /// </summary>
    public class Horse : DatabaseEntry {

        /// <summary>
        /// Default constructor for the horse object.
        /// </summary>
        public Horse() {
        }

        ///////////////////////////////////////////////////
        // Information Section

        /// <summary>
        /// The name of the horse.
        /// </summary>
        public string Name {
            get; set;
        } = string.Empty;

        /// <summary>
        /// The type of horse.
        /// </summary>
        public HorseType Type {
            get; set;
        } = HorseType.Standard;

        /// <summary>
        /// The breed of the horse.
        /// </summary>
        public string Breed {
            get; set;
        } = string.Empty;

        /// <summary>
        /// The sex of the horse.
        /// </summary>
        public HorseSex Sex {
            get; set;
        } = HorseSex.Colt;

        /// <summary>
        /// The date of birth of the specified horse.
        /// </summary>
        public int Age {
            get; set;
        } = 1;

        ///////////////////////////////////////////////////
        // Appearance Section

        /// <summary>
        /// The height of the horse, in the specified units.
        /// </summary>
        public float Height {
            get; set;
        } = 0.0f;

        /// <summary>
        /// The weight of the horse, in the specified units.
        /// </summary>
        public float Weight {
            get; set;
        } = 0.0f;

        /// <summary>
        /// The color of the horse.
        /// </summary>
        public string Color {
            get; set;
        } = "";

        /// <summary>
        /// A short description of the markings on the horse.
        /// </summary>
        public string Markings {
            get; set;
        } = "";

        ///////////////////////////////////////////////////
        // Pedigree Information

        /// <summary>
        /// Breed of the current horse.
        /// </summary>
        public string Pedigree {
            get; set;
        } = "";

        /// <summary>
        /// Breed of the horse's sire (father).
        /// </summary>
        public string Sire {
            get; set;
        } = "";

        /// <summary>
        /// Breed of the horse's paternal grand sire (father's father).
        /// </summary>
        public string PaternalGrandSire {
            get; set;
        } = "";

        /// <summary>
        /// Breed of the horse's paternal grand dam (father's mother).
        /// </summary>
        public string PaternalGrandDam {
            get; set;
        } = "";

        /// <summary>
        /// Breed of the horse's dam (mother).
        /// </summary>
        public string Dam {
            get; set;
        } = "";

        /// <summary>
        /// Breed of the horses's maternal grand sire (mother's father).
        /// </summary>
        public string MaternalGrandSire {
            get; set;
        }

        /// <summary>
        /// Breed of the horse's maternal grand dam (mother's mother).
        /// </summary>
        public string MaternalGrandDam {
            get; set;
        }

        /// <summary>
        /// Method to get the height units of the horse based on its type.
        /// </summary>
        /// <returns>A string indicating height units.</returns>
        public string GetHeightUnits() {

            // Switch based on the type of horse
            switch (this.Type) {

                case HorseType.Standard:
                    return "hands";

                case HorseType.Miniature:
                    return "inches";

                // Handle if the current type of horse does not have a defined
                // measurement for its height
                default:
                    return "hands";

            }

        }

        /// <summary>
        /// Method to get the weight units of the horse.
        /// </summary>
        /// <returns>A string indicating weight units.</returns>
        public string GetWeightUnits() {

            // Switch based on what is configured under user settings
            switch (AppSettings.Application.WeightUnits) {

                case AppSettings.Application.WeightUnit.Pounds:
                    return "lbs";

                case AppSettings.Application.WeightUnit.Kilograms:
                    return "kg";

                // Handle if the current type of horse does not have a defined
                // measurement for its weight
                default:
                    return "lbs";

            }

        }

    }

}
