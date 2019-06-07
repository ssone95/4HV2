//
// _4H_Application.Models.Settings.AppSettings.cs: Helper class for storing app
//   settings: from Xam.Plugins.Settings NuGet package.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//   Joel Krause(jkjk8080 @gmail.com)
//

using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace _4H_Application.Models.Settings {

    /// <summary>
    /// Class that represents a single setting.
    /// </summary>
    public static class AppSettings  {

        /// <summary>
        /// Reference to the AppSettings connector.
        /// </summary>
        public static ISettings Settings => CrossSettings.Current;

        /// <summary>
        /// Internal static class for all of the user settings.
        /// </summary>
        public static class User {

            /// <summary>
            /// Reference to the name of the user.
            /// </summary>
            public static string UserName {
                get => Settings.GetValueOrDefault(
                    nameof(UserName), string.Empty);
                set => Settings.AddOrUpdateValue(
                    nameof(UserName), value);
            }

            /// <summary>
            /// Reference to the date of birth of the user.
            /// </summary>
            public static DateTime DateOfBirth {
                get => Settings.GetValueOrDefault(
                    nameof(DateOfBirth), DateTime.Now);
                set => Settings.AddOrUpdateValue(
                    nameof(DateOfBirth), value);
            }

            /// <summary>
            /// Reference to the address of the user.
            /// </summary>
            public static string Address {
                get => Settings.GetValueOrDefault(
                    nameof(Address), string.Empty);
                set => Settings.AddOrUpdateValue(
                    nameof(Address), value);
            }

        }

        /// <summary>
        /// Internal static class for all of the horse club settings.
        /// </summary>
        public static class HorseClub {

            /// <summary>
            /// Reference to the county of the horse club.
            /// </summary>
            public static string County {
                get => Settings.GetValueOrDefault(
                    nameof(County), string.Empty);
                set => Settings.AddOrUpdateValue(
                    nameof(County), value);
            }

            /// <summary>
            /// Reference to the name of the horse club.
            /// </summary>
            public static string ClubName {
                get => Settings.GetValueOrDefault(
                    nameof(ClubName), string.Empty);
                set => Settings.AddOrUpdateValue(
                    nameof(ClubName), value);
            }

            /// <summary>
            /// Reference to the name of the horse club leader.
            /// </summary>
            public static string LeaderName {
                get => Settings.GetValueOrDefault(
                    nameof(LeaderName), string.Empty);
                set => Settings.AddOrUpdateValue(
                    nameof(LeaderName), value);
            }

        }

        /// <summary>
        /// Internal static class for all of the horse program settings.
        /// </summary>
        public static class HorseProgram {

            /// <summary>
            /// Reference to the name of the project helper.
            /// </summary>
            public static string HelperName {
                get => Settings.GetValueOrDefault(
                    nameof(HelperName), string.Empty);
                set => Settings.AddOrUpdateValue(
                    nameof(HelperName), value);
            }

            /// <summary>
            /// Reference to the 4-H horse program level.
            /// </summary>
            public static ProgramLevel Level {
                get {
                    int i = Settings.GetValueOrDefault(
                        nameof(Level), 1);
                    return (ProgramLevel) i;
                }
                set => Settings.AddOrUpdateValue(
                    nameof(Level), (int) value);
            }

            /// <summary>
            /// Reference to the number of years spent at the current level.
            /// </summary>
            public static ProgramYear Year {
                get {
                    int i = Settings.GetValueOrDefault(
                        nameof(Year), 1);
                    return (ProgramYear) i;
                }
                set => Settings.AddOrUpdateValue(
                    nameof(Year), (int) value);
            }

            /// <summary>
            /// Reference to the date that the record book opens on.
            /// </summary>
            public static DateTime StartDate {
                get => Settings.GetValueOrDefault(
                    nameof(StartDate), DateTime.Now);
                set => Settings.AddOrUpdateValue(
                    nameof(StartDate), value);
            }

            /// <summary>
            /// Reference to the date that the record book closes on.
            /// </summary>
            public static DateTime CloseDate {
                get => Settings.GetValueOrDefault(
                    nameof(CloseDate), DateTime.Now);
                set => Settings.AddOrUpdateValue(
                    nameof(CloseDate), value);
            }

            /// <summary>
            /// Enum that contains all of the possible program levels.
            /// </summary>
            public enum ProgramLevel {
                First = 1,
                Second,
                Third,
                Fourth,
                Fifth,
                Other,
            }

            /// <summary>
            /// Enum that contains all of the possible program years.
            /// </summary>
            public enum ProgramYear {
                First = 1,
                Second,
                Third,
            }

        }

        /// <summary>
        /// Internal static class for all of the application settings.
        /// </summary>
        public static class Application {

            /// <summary>
            /// Reference to the weight units in the application.
            /// </summary>
            public static WeightUnit WeightUnits {
                get {
                    int i = Settings.GetValueOrDefault(
                        nameof(WeightUnits), 0);
                    return (WeightUnit) i;
                }
                set => Settings.AddOrUpdateValue(
                    nameof(WeightUnits), (int) value);
            }

            /// <summary>
            /// All of the available weight units for the application
            /// </summary>
            public enum WeightUnit {
                Pounds,
                Kilograms,
            }

        }

        public static class Auth0Details {
            public static string identity {
                get => Settings.GetValueOrDefault(
                    nameof(identity), string.Empty);
                set => Settings.AddOrUpdateValue(
                    nameof(identity), value);
            }

            public static bool logged_in {
                get => Settings.GetValueOrDefault(
                    nameof(logged_in), false);
                set => Settings.AddOrUpdateValue(
                    nameof(logged_in), value);
            }

            public static void init()
            {
                identity = String.Empty;
                logged_in = false;
            }
        }

    }

}
