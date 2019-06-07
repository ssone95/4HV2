//
// _4H_Application.Models.Projects.ProjectPlan.cs: Class that represents a
//   project plan page for a single horse.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Data;
using System;

namespace _4H_Application.Models.Projects {

    /// <summary>
    /// Class that represents a project plan page for a single horse.
    /// </summary>
    public class ProjectPlan : DatabaseEntry {

        /// <summary>
        /// Reference to the Id of the horse for the project plan.
        /// </summary>
        public int HorseId {
            get; set;
        } = -1;

        /// <summary>
        /// Reference to the 4-H year (start).
        /// </summary>
        public int Year {
            get; set;
        } = -1;

        /// <summary>
        /// Reference to the horse plans for the current horse.
        /// </summary>
        public string HorsePlans {
            get; set;
        } = string.Empty;

        /// <summary>
        /// Reference to the caring plans for the current horse.
        /// </summary>
        public string CaringPlans {
            get; set;
        } = string.Empty;

        /// <summary>
        /// Reference to the learning plans for the current horse.
        /// </summary>
        public string LearningPlans {
            get; set;
        } = string.Empty;

    }

}
