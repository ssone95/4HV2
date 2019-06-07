//
// _4H_Application.Models.Records.ServiceRecordEntry.cs: Class that represents
//   a single entry in a user's service record.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

namespace _4H_Application.Models.Records {

    /// <summary>
    /// Class that represents a single entry in the user's service record.
    /// </summary>
    public class ServiceRecordEntry : 
        RecordBookEntry {

        /// <summary>
        /// Reference to the type of service.
        /// </summary>
        public ServiceType Type {
            get; set;
        } = ServiceType.HealthCare;

        /// <summary>
        /// Reference to the description of the service.
        /// </summary>
        public string Description {
            get; set;
        } = string.Empty;

        /// <summary>
        /// Reference to the cost of the service.
        /// </summary>
        public float Cost {
            get; set;
        } = 0.0f;

    }

    /// <summary>
    /// List of available service types.
    /// </summary>
    public enum ServiceType {
        RidingInstruction,
        HealthCare,
        Farrier,
        Other,
    }

}
