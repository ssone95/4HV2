using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace _4H_RESTful
{
    public class DBConnection
    {
        string connectionString = "Server=localhost;Port=3306;Database=_4hbase;Uid=root;Pwd=pass;";
        MySqlConnection connection;
        private static DBConnection instance;

        private DBConnection()
        {
            connection = new MySqlConnection(connectionString);
        }

        public static DBConnection GetInstance()
        {
            if (instance == null)
                instance = new DBConnection();
            return instance;
        }

        public void Insert(RecordBundle bundle)
        {
            // get rid of the old entries for the user
            Remove(bundle.identity);

            // open the connection
            connection.Open();

            // Add in the new entries
            MySqlCommand cmd = connection.CreateCommand();

            // add user information
            cmd.CommandText = "INSERT INTO app_user(auth0token, program_lvl) VALUES (@auth0token, @program_lvl)";
            cmd.Parameters.AddWithValue("@auth0token", bundle.identity);
            cmd.Parameters.AddWithValue("program_lvl", bundle.program_level);
            cmd.ExecuteNonQuery();

            // add horses
            foreach (RecordBundle.Horse horse in bundle.horses) {
                cmd = connection.CreateCommand();
                cmd.CommandText =
                    "INSERT INTO horse (user_token, localid, _name, _type, breed, sex, age, height, weight, color, markings," +
                    " pedigree, sire, paternalGrandSire, paternalGrandDam, dam, maternalGrandSire, maternalGrandDam) " +
                    "VALUES (@user_token, @localid, @_name, @_type, @breed, @sex, @age, @height, @weight, @color, @markings," +
                    " @pedigree, @sire, @paternalGrandSire, @paternalGrandDam, @dam, @maternalGrandSire, @maternalGrandDam)";
                cmd.Parameters.AddWithValue("@user_token", bundle.identity);
                cmd.Parameters.AddWithValue("@localid", horse.id);
                cmd.Parameters.AddWithValue("@_name", horse.name);
                cmd.Parameters.AddWithValue("@_type", horse.type);
                cmd.Parameters.AddWithValue("@breed", horse.breed);
                cmd.Parameters.AddWithValue("@sex", horse.sex);
                cmd.Parameters.AddWithValue("@height", horse.height);
                cmd.Parameters.AddWithValue("@weight", horse.weight);
                cmd.Parameters.AddWithValue("@age", horse.age);
                cmd.Parameters.AddWithValue("@color", horse.color);
                cmd.Parameters.AddWithValue("@markings", horse.markings);
                cmd.Parameters.AddWithValue("@pedigree", horse.pedigree);
                cmd.Parameters.AddWithValue("@sire", horse.sire);
                cmd.Parameters.AddWithValue("@paternalGrandSire", horse.paternalGrandSire);
                cmd.Parameters.AddWithValue("@paternalGrandDam", horse.paternalGrandDam);
                cmd.Parameters.AddWithValue("@dam", horse.dam);
                cmd.Parameters.AddWithValue("@maternalGrandSire", horse.maternalGrandSire);
                cmd.Parameters.AddWithValue("@maternalGrandDam", horse.maternalGrandDam);
                cmd.ExecuteNonQuery();
            }

            // add project experiences
            foreach (RecordBundle.ProjectExperience exp in bundle.projectExperiences) {
                cmd = connection.CreateCommand();
                cmd.CommandText =
                    "INSERT INTO project_experiences (user_token, horse_id, program_level, _year, feedExperience, healthExperience, learningExperience, goalsExperience, otherExperience)" +
                    "VALUES (@user_token, @horse_id, @program_level, @_year, @feedExperience, @healthExperience, @learningExperience, @goalsExperience, @otherExperience)";
                cmd.Parameters.AddWithValue("@user_token", bundle.identity);
                cmd.Parameters.AddWithValue("@horse_id", exp.horseId);
                cmd.Parameters.AddWithValue("@program_level", exp.programLevel);
                cmd.Parameters.AddWithValue("@_year", exp.year);
                cmd.Parameters.AddWithValue("@feedExperience", exp.feedExperience);
                cmd.Parameters.AddWithValue("@healthExperience", exp.healthExperience);
                cmd.Parameters.AddWithValue("@learningExperience", exp.learningExperience);
                cmd.Parameters.AddWithValue("@goalsExperience", exp.goalsExperience);
                cmd.Parameters.AddWithValue("@otherExperience", exp.otherExperience);
                cmd.ExecuteNonQuery();
            }

            // add project plans
            foreach (RecordBundle.ProjectPlan plan in bundle.projectPlans) {
                cmd = connection.CreateCommand();
                cmd.CommandText =
                    "INSERT INTO project_plans (user_token, horse_id, _year, horse_plans, caring_plans, learning_plans)" +
                    "VALUES (@user_token, @horse_id, @_year, @horse_plans, @caring_plans, @learning_plans)";
                cmd.Parameters.AddWithValue("@user_token", bundle.identity);
                cmd.Parameters.AddWithValue("@horse_id", plan.horseId);
                cmd.Parameters.AddWithValue("@_year", plan.year);
                cmd.Parameters.AddWithValue("@horse_plans", plan.horsePlans);
                cmd.Parameters.AddWithValue("@caring_plans", plan.caringPlans);
                cmd.Parameters.AddWithValue("@learning_plans", plan.learningPlans);
                cmd.ExecuteNonQuery();
            }

            // add project stories
            foreach (RecordBundle.ProjectStory story in bundle.projectStories)
            {
                cmd = connection.CreateCommand();
                cmd.CommandText =
                    "INSERT INTO project_story (user_token, horse_id, _year, story)" +
                    "VALUES (@user_token, @horse_id, @_year, @story)";
                cmd.Parameters.AddWithValue("@user_token", bundle.identity);
                cmd.Parameters.AddWithValue("@horse_id", story.horseId);
                cmd.Parameters.AddWithValue("@_year", story.year);
                cmd.Parameters.AddWithValue("@story", story.story);
                cmd.ExecuteNonQuery();
            }

            // add activity records
            foreach (RecordBundle.ActivityRecordEntry record in bundle.activityRecordEntries)
            {
                cmd = connection.CreateCommand();
                cmd.CommandText =
                    "INSERT INTO activity_record (user_token, horse_id, _date, description, location)" +
                    "VALUES (@user_token, @horse_id, @_date, @description, @location)";
                cmd.Parameters.AddWithValue("@user_token", bundle.identity);
                cmd.Parameters.AddWithValue("@horse_id", record.horseId);
                cmd.Parameters.AddWithValue("@_date", record.date);
                cmd.Parameters.AddWithValue("@description", record.description);
                cmd.Parameters.AddWithValue("@location", record.location);
                cmd.ExecuteNonQuery();
            }

            // add bedding records
            foreach (RecordBundle.BeddingRecordEntry record in bundle.beddingRecordEntries)
            {
                cmd = connection.CreateCommand();
                cmd.CommandText =
                    "INSERT INTO bedding_record (user_token, horse_id, _date, _type, amount, cost)" +
                    "VALUES (@user_token, @horse_id, @_date, @_type, @amount, @cost)";
                cmd.Parameters.AddWithValue("@user_token", bundle.identity);
                cmd.Parameters.AddWithValue("@horse_id", record.horseId);
                cmd.Parameters.AddWithValue("@_date", record.date);
                cmd.Parameters.AddWithValue("@_type", record.type);
                cmd.Parameters.AddWithValue("@amount", record.amount);
                cmd.Parameters.AddWithValue("@cost", record.cost);
                cmd.ExecuteNonQuery();
            }

            // add expense records
            foreach (RecordBundle.ExpenseRecordEntry record in bundle.expenseRecordEntries)
            {
                cmd = connection.CreateCommand();
                cmd.CommandText =
                    "INSERT INTO expense_record (user_token, horse_id, _date, description, cost)" +
                    "VALUES (@user_token, @horse_id, @_date, @description, @cost)";
                cmd.Parameters.AddWithValue("@user_token", bundle.identity);
                cmd.Parameters.AddWithValue("@horse_id", record.horseId);
                cmd.Parameters.AddWithValue("@_date", record.date);
                cmd.Parameters.AddWithValue("@description", record.description);
                cmd.Parameters.AddWithValue("@cost", record.cost);
                cmd.ExecuteNonQuery();
            }

            // add feed records
            foreach (RecordBundle.FeedRecordEntry record in bundle.feedRecordEntries)
            {
                cmd = connection.CreateCommand();
                cmd.CommandText =
                    "INSERT INTO feed_record (user_token, horse_id, _date, _type, description, amount, cost)" +
                    "VALUES (@user_token, @horse_id, @_date, @_type, @description, @amount, @cost)";
                cmd.Parameters.AddWithValue("@user_token", bundle.identity);
                cmd.Parameters.AddWithValue("@horse_id", record.horseId);
                cmd.Parameters.AddWithValue("@_date", record.date);
                cmd.Parameters.AddWithValue("@_type", record.type);
                cmd.Parameters.AddWithValue("@description", record.description);
                cmd.Parameters.AddWithValue("@amount", record.amount);
                cmd.Parameters.AddWithValue("@cost", record.cost);
                cmd.ExecuteNonQuery();
            }

            // add labor records
            foreach (RecordBundle.LaborRecordEntry record in bundle.laborRecordEntries)
            {
                cmd = connection.CreateCommand();
                cmd.CommandText =
                    "INSERT INTO labor_record (user_token, horse_id, _date, _start, _end, description)" +
                    "VALUES (@user_token, @horse_id, @_date, @_start, @_end, @description)";
                cmd.Parameters.AddWithValue("@user_token", bundle.identity);
                cmd.Parameters.AddWithValue("@horse_id", record.horseId);
                cmd.Parameters.AddWithValue("@_date", record.date);
                cmd.Parameters.AddWithValue("@_start", record.start);
                cmd.Parameters.AddWithValue("@_end", record.end);
                cmd.Parameters.AddWithValue("@description", record.description);
                cmd.ExecuteNonQuery();
            }

            // add riding records
            foreach (RecordBundle.RidingRecordEntry record in bundle.ridingRecordEntries)
            {
                cmd = connection.CreateCommand();
                cmd.CommandText =
                    "INSERT INTO riding_record (user_token, horse_id, _date, _start, _end, description)" +
                    "VALUES (@user_token, @horse_id, @_date, @_start, @_end, @description)";
                cmd.Parameters.AddWithValue("@user_token", bundle.identity);
                cmd.Parameters.AddWithValue("@horse_id", record.horseId);
                cmd.Parameters.AddWithValue("@_date", record.date);
                cmd.Parameters.AddWithValue("@_start", record.start);
                cmd.Parameters.AddWithValue("@_end", record.end);
                cmd.Parameters.AddWithValue("@description", record.description);
                cmd.ExecuteNonQuery();
            }

            // add service records
            foreach (RecordBundle.ServiceRecordEntry record in bundle.serviceRecordEntries)
            {
                cmd = connection.CreateCommand();
                cmd.CommandText =
                    "INSERT INTO service_record (user_token, horse_id, _date, _type, description, cost)" +
                    "VALUES (@user_token, @horse_id, @_date, @_type, @description, @cost)";
                cmd.Parameters.AddWithValue("@user_token", bundle.identity);
                cmd.Parameters.AddWithValue("@horse_id", record.horseId);
                cmd.Parameters.AddWithValue("@_date", record.date);
                cmd.Parameters.AddWithValue("@_type", record.type);
                cmd.Parameters.AddWithValue("@description", record.description);
                cmd.Parameters.AddWithValue("@cost", record.cost);
                cmd.ExecuteNonQuery();
            }

            // close the connection
            connection.Close();
        }

        public void Remove(string user_id)
        {
            // open the connection
            connection.Open();
            MySqlCommand cmd;

            // delete all records for the user
            cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM activity_record WHERE user_token = @user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.ExecuteNonQuery();
            cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM bedding_record WHERE user_token = @user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.ExecuteNonQuery();
            cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM expense_record WHERE user_token = @user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.ExecuteNonQuery();
            cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM feed_record WHERE user_token = @user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.ExecuteNonQuery();
            cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM labor_record WHERE user_token = @user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.ExecuteNonQuery();
            cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM riding_record WHERE user_token = @user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.ExecuteNonQuery();
            cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM service_record WHERE user_token = @user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.ExecuteNonQuery();

            // delete all project items for the user
            cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM project_experiences WHERE user_token = @user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.ExecuteNonQuery();
            cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM project_plans WHERE user_token = @user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.ExecuteNonQuery();
            cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Project_story WHERE user_token = @user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.ExecuteNonQuery();

            // delete all horses for the user
            cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM horse WHERE user_token = @user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.ExecuteNonQuery();

            // delete the user
            cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM app_user WHERE auth0token = @user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.ExecuteNonQuery();

            // close the connection
            connection.Close();
        }

        public RecordBundle Obtain(string user_id)
        {
            connection.Open();
            var cmd = connection.CreateCommand();

            // verify the requested user_id is in the database
            cmd.CommandText = "SELECT * FROM app_user WHERE auth0token=@user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
                return new RecordBundle { identity = "didnotfind" };

            var bundle = new RecordBundle {
                identity = user_id,
                horses = new List<RecordBundle.Horse>(),
                projectExperiences = new List<RecordBundle.ProjectExperience>(),
                projectPlans = new List<RecordBundle.ProjectPlan>(),
                projectStories = new List<RecordBundle.ProjectStory>(),
                activityRecordEntries = new List<RecordBundle.ActivityRecordEntry>(),
                beddingRecordEntries = new List<RecordBundle.BeddingRecordEntry>(),
                expenseRecordEntries = new List<RecordBundle.ExpenseRecordEntry>(),
                feedRecordEntries = new List<RecordBundle.FeedRecordEntry>(),
                laborRecordEntries = new List<RecordBundle.LaborRecordEntry>(),
                ridingRecordEntries = new List<RecordBundle.RidingRecordEntry>(),
                serviceRecordEntries = new List<RecordBundle.ServiceRecordEntry>()
            };

            // Get the user information first
            while (reader.Read()) {
                bundle.program_level = reader.GetInt16("program_lvl");
            }
            reader.Close();

            // Get the horses
            cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM horse WHERE user_token=@user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            reader = cmd.ExecuteReader();

            while(reader.Read()) {
                bundle.horses.Add(new RecordBundle.Horse {
                    id = reader.GetInt32("localid"),
                    name = reader.GetString("_name"),
                    type = reader.GetInt16("_type"),
                    breed = reader.GetString("breed"),
                    sex = reader.GetInt16("sex"),
                    age = reader.GetInt32("age"),
                    height = reader.GetFloat("height"),
                    weight = reader.GetFloat("weight"),
                    pedigree = reader.GetString("pedigree"),
                    sire = reader.GetString("sire"),
                    paternalGrandSire = reader.GetString("paternalGrandSire"),
                    paternalGrandDam = reader.GetString("paternalGrandDam"),
                    dam = reader.GetString("dam"),
                    maternalGrandSire = reader.GetString("maternalGrandSire"),
                    maternalGrandDam = reader.GetString("maternalGrandDam"),
                    color = reader.GetString("color"),
                    markings = reader.GetString("markings")
                });
            }
            reader.Close();

            // Get the project experiences
            cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM project_experiences WHERE user_token=@user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            reader = cmd.ExecuteReader();

            while (reader.Read()) {
                bundle.projectExperiences.Add(new RecordBundle.ProjectExperience {
                    horseId = reader.GetInt32("horse_id"),
                    programLevel = reader.GetInt32("program_level"),
                    year = reader.GetInt32("_year"),
                    feedExperience = reader.GetString("feedExperience"),
                    healthExperience = reader.GetString("healthExperience"),
                    learningExperience = reader.GetString("learningExperience"),
                    goalsExperience = reader.GetString("goalsExperience"),
                    otherExperience = reader.GetString("otherExperience")
                });
            }
            reader.Close();

            // Get the project plans
            cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM project_plans WHERE user_token=@user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            reader = cmd.ExecuteReader();

            while (reader.Read()) {
                bundle.projectPlans.Add(new RecordBundle.ProjectPlan {
                    horseId = reader.GetInt32("horse_id"),
                    year = reader.GetInt32("_year"),
                    horsePlans = reader.GetString("horse_plans"),
                    caringPlans = reader.GetString("caring_plans"),
                    learningPlans = reader.GetString("learning_plans")
                });
            }
            reader.Close();

            // Get the project story
            cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM project_story WHERE user_token=@user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            reader = cmd.ExecuteReader();

            while (reader.Read()) {
                bundle.projectStories.Add(new RecordBundle.ProjectStory {
                    horseId = reader.GetInt32("horse_id"),
                    year = reader.GetInt32("_year"),
                    story = reader.GetString("story")
                });
            }
            reader.Close();

            // Get the activity records
            cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM activity_record WHERE user_token=@user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                bundle.activityRecordEntries.Add(new RecordBundle.ActivityRecordEntry
                {
                    horseId = reader.GetInt32("horse_id"),
                    date = reader.GetDateTime("_date"),
                    description = reader.GetString("description"),
                    location = reader.GetString("location")
                });
            }
            reader.Close();

            // Get the bedding records
            cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM bedding_record WHERE user_token=@user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                bundle.beddingRecordEntries.Add(new RecordBundle.BeddingRecordEntry
                {
                    horseId = reader.GetInt32("horse_id"),
                    date = reader.GetDateTime("_date"),
                    type = reader.GetInt16("_type"),
                    amount = reader.GetFloat("amount"),
                    cost = reader.GetFloat("cost"),
                });
            }
            reader.Close();

            // Get the expense records
            cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM expense_record WHERE user_token=@user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                bundle.expenseRecordEntries.Add(new RecordBundle.ExpenseRecordEntry
                {
                    horseId = reader.GetInt32("horse_id"),
                    date = reader.GetDateTime("_date"),
                    description = reader.GetString("description"),
                    cost = reader.GetFloat("cost")
                });
            }
            reader.Close();

            // Get the feed records
            cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM feed_record WHERE user_token=@user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                bundle.feedRecordEntries.Add(new RecordBundle.FeedRecordEntry
                {
                    horseId = reader.GetInt32("horse_id"),
                    date = reader.GetDateTime("_date"),
                    type = reader.GetInt16("_type"),
                    description = reader.GetString("description"),
                    amount = reader.GetFloat("amount"),
                    cost = reader.GetFloat("cost")
                });
            }
            reader.Close();

            // Get the labor records
            cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM labor_record WHERE user_token=@user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                bundle.laborRecordEntries.Add(new RecordBundle.LaborRecordEntry
                {
                    horseId = reader.GetInt32("horse_id"),
                    date = reader.GetDateTime("_date"),
                    start = reader.GetTimeSpan("_start"),
                    end = reader.GetTimeSpan("_end"),
                    description = reader.GetString("description")
                });
            }
            reader.Close();

            // Get the riding records
            cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM riding_record WHERE user_token=@user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                bundle.ridingRecordEntries.Add(new RecordBundle.RidingRecordEntry
                {
                    horseId = reader.GetInt32("horse_id"),
                    date = reader.GetDateTime("_date"),
                    start = reader.GetTimeSpan("_start"),
                    end = reader.GetTimeSpan("_end"),
                    description = reader.GetString("description")
                });
            }
            reader.Close();

            // Get the service records
            cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM service_record WHERE user_token=@user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                bundle.serviceRecordEntries.Add(new RecordBundle.ServiceRecordEntry
                {
                    horseId = reader.GetInt32("horse_id"),
                    date = reader.GetDateTime("_date"),
                    type = reader.GetInt16("_type"),
                    description = reader.GetString("description"),
                    cost = reader.GetFloat("cost")
                });
            }
            reader.Close();

            connection.Close();

            return bundle;
        }
    }
}
