//
// _4H_Application.Modles.Project.ProjectExperience.cs: Class that represents a
//   project experience for a single horse.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Data;
using _4H_Application.Models.Settings;

namespace _4H_Application.Models.Projects {

    /// <summary>
    /// Class that allows a user to manage their project experiences.
    /// </summary>
    public class ProjectExperiences : DatabaseEntry {

        /// <summary>
        /// Reference to the Id of the horse for a project experience.
        /// </summary>
        public int HorseId {
            get; set;
        } = -1;

        /// <summary>
        /// Reference to the program level of the experience.
        /// </summary>
        public AppSettings.HorseProgram.ProgramLevel Level {
            get; set;
        } = AppSettings.HorseProgram.ProgramLevel.First;

        /// <summary>
        /// Reference to the year for the project experiences.
        /// </summary>
        public int Year {
            get; set;
        } = -1;

        /// <summary>
        /// Reference to the feed/care experience.
        /// </summary>
        public string FeedCareExperiences {
            get; set;
        } = string.Empty;

        /// <summary>
        /// Reference to the health experience.
        /// </summary>
        public string HealthExperiences {
            get; set;
        } = string.Empty;

        /// <summary>
        /// Reference to the learning experience.
        /// </summary>
        public string LearningExperiences {
            get; set;
        } = string.Empty;

        /// <summary>
        /// Reference to the goals experience.
        /// </summary>
        public string GoalsExperiences {
            get; set;
        } = string.Empty;

        /// <summary>
        /// Reference to the other experience (first-level only).
        /// </summary>
        public string OtherExperiences {
            get; set;
        } = string.Empty;

    }

}
