//
// _4H_RESTful.RecordBundle.cs - A class used for model binding
//
// Author(s): Joel Krause - jkjk8080@gmail.com
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _4H_RESTful
{
    public class RecordBundle
    {
        public string identity;
        public int program_level;
        public List<Horse> horses;
        public List<ProjectExperience> projectExperiences;
        public List<ProjectPlan> projectPlans;
        public List<ProjectStory> projectStories;
        public List<ActivityRecordEntry> activityRecordEntries;
        public List<BeddingRecordEntry> beddingRecordEntries;
        public List<ExpenseRecordEntry> expenseRecordEntries;
        public List<FeedRecordEntry> feedRecordEntries;
        public List<LaborRecordEntry> laborRecordEntries;
        public List<RidingRecordEntry> ridingRecordEntries;
        public List<ServiceRecordEntry> serviceRecordEntries;

        public class DatabaseEntry
        {
            public int id;
            public DateTime creationDate;
        }

        public class Horse : DatabaseEntry
        {
            public string name;
            public int type;
            public string breed;
            public int sex;
            public int age;
            public float height;
            public float weight;
            public string color;
            public string markings;
            public string pedigree;
            public string sire;
            public string paternalGrandSire;
            public string paternalGrandDam;
            public string dam;
            public string maternalGrandSire;
            public string maternalGrandDam;
        }  

        public class ProjectExperience : DatabaseEntry
        {
            public int horseId;
            public int programLevel;
            public int year;
            public string feedExperience;
            public string healthExperience;
            public string learningExperience;
            public string goalsExperience;
            public string otherExperience;
        }

        public class ProjectPlan : DatabaseEntry
        {
            public int horseId;
            public int year;
            public string horsePlans;
            public string caringPlans;
            public string learningPlans;
        }

        public class ProjectStory : DatabaseEntry
        {
            public int horseId;
            public int year;
            public string story;
        }

        public class RecordEntry : DatabaseEntry
        {
            public int horseId;
            public DateTime date;
        }

        public class ActivityRecordEntry : RecordEntry
        {
            public string description;
            public string location;
        }

        public class BeddingRecordEntry : RecordEntry
        {
            public int type;
            public float amount;
            public float cost;
        }

        public class ExpenseRecordEntry : RecordEntry
        {
            public string description;
            public float cost;
        }

        public class FeedRecordEntry : RecordEntry
        {
            public int type;
            public string description;
            public float amount;
            public float cost;
        }

        public class LaborRecordEntry : RecordEntry
        {
            public TimeSpan start;
            public TimeSpan end;
            public string description;
        }

        public class RidingRecordEntry : RecordEntry
        {
            public TimeSpan start;
            public TimeSpan end;
            public string description;
        }

        public class ServiceRecordEntry : RecordEntry
        {
            public int type;
            public string description;
            public float cost;
        }
    }
}
