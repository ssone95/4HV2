//
// _4H_Application.Models.Projects.ProjectPlan.cs: Class that represents a
//   project story for a single horse.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Data;

namespace _4H_Application.Models.Projects {

    /// <summary>
    /// Class that represents a project story for a single horse.
    /// </summary>
    public class ProjectStory : DatabaseEntry {

        /// <summary>
        /// Reference to the Id of the horse for the project story.
        /// </summary>
        public int HorseId {
            get; set;
        } = -1;

        /// <summary>
        /// Reference to the year of the project.
        /// </summary>
        public int Year {
            get; set;
        } = -1;

        /// <summary>
        /// Reference to the story for the current horse.
        /// </summary>
        public string Story {
            get; set;
        } = string.Empty;

    }

}
