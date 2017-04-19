using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Alerts;
using Common.Communication;
using Common.Messages;
using Common.Users;
using Common.Utilities;
using Common.WorkItems;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Common.Utilities
{
    [DataContract]
    public class PersistentSettings
    {
        public PersistentSettings()
        {
            UserList = new List<User>();
            ContractList = new List<Contract>();
            PhaseList = new List<Phase>();
            TaskList = new List<WorkItems.Task>();
            CustomItems = new Dictionary<string, string>();
        }

        [DataMember]
        public List<User> UserList { get; set; }

        [DataMember]
        public Dictionary<string, string> CustomItems { get; set; }

        [DataMember]
        public List<Contract> ContractList { get; set; }

        [DataMember]
        public List<Phase> PhaseList { get; set; }

        [DataMember]
        public List<WorkItems.Task> TaskList { get; set; }
    }

    public class PersistentFileHandler
    {
        public PersistentFileHandler(String handlerName)
        {
            Logger = new LogUtility(handlerName);
        }

        protected StreamReader Reader { get; set; }
        protected StreamWriter Writer { get; set; }

        public PersistentSettings Load(string filename)
        {
            PersistentSettings settings = null;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PersistentSettings), new Type[] { typeof(PersistentSettings) });

            if (OpenReader(filename))
            {
                settings = serializer.ReadObject(Reader.BaseStream) as PersistentSettings;
                Reader.Close();
            }

            return settings;
        }

        public bool Save(string filename, PersistentSettings setting)
        {
            bool success = false;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PersistentSettings), new Type[] { typeof(PersistentSettings) });

            if (OpenWriter(filename))
            {
                serializer.WriteObject(Writer.BaseStream, setting);
                Writer.Close();
                success = true;
            }

            return success;
        }

        protected virtual bool OpenReader(string filename)
        {
            bool result = false;
            try
            {
                Reader = new StreamReader(filename);
                result = true;
            }
            catch (Exception err)
            {
                Console.WriteLine($"Cannot open input file {filename}, error={err}");
            }

            return result;
        }


        protected virtual bool OpenWriter(string filename)
        {
            bool result = false;
            try
            {
                if (Directory.Exists(Path.GetDirectoryName(filename)))
                {
                    Writer = new StreamWriter(filename);
                    result = true;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine($"Cannot open output file {filename}, error={err}");
            }

            return result;
        }
        
        protected LogUtility Logger;
    }
}




//    abstract public class PersistenceHandler
//    {
//        public PersistenceHandler(String handlerName)
//        {
//            Logger = new LogUtility(handlerName);
//        }

//        protected StreamReader Reader { get; set; }
//        protected StreamWriter Writer { get; set; }
//        public abstract List<Effort> Load(string filename);
//        public abstract bool Save(string filename, List<MatchResult> persons);

//        protected virtual bool OpenReader(string filename)
//        {
//            bool result = false;
//            try
//            {
//                Reader = new StreamReader(filename);
//                result = true;
//            }
//            catch (Exception err)
//            {
//                Console.WriteLine($"Cannot open input file {filename}, error={err}");
//            }

//            return result;
//        }


//        protected virtual bool OpenWriter(string filename)
//        {
//            bool result = false;
//            try
//            {
//                if (Directory.Exists(Path.GetDirectoryName(filename)))
//                {
//                    Writer = new StreamWriter(filename);
//                    result = true;
//                }
//            }
//            catch (Exception err)
//            {
//                Console.WriteLine($"Cannot open output file {filename}, error={err}");
//            }

//            return result;
//        }


//        public static List<Person> Load(string filename)
//        {
//            List<Person> personList = null;
//            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Person>), new Type[] { typeof(Person), typeof(Adult), typeof(Child) });

//            if (OpenReader(filename))
//            {
//                personList = serializer.ReadObject(Reader.BaseStream) as List<Person>;
//                Reader.Close();
//            }

//            return personList;
//        }

//        public static bool Save(string filename, List<MatchResult> matches)
//        {
//            bool success = false;
//            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<MatchResult>), new Type[] { typeof(MatchResult), typeof(Person), typeof(Adult), typeof(Child) });

//            if (OpenWriter(filename))
//            {
//                serializer.WriteObject(Writer.BaseStream, matches);
//                Writer.Close();
//                success = true;
//            }

//            return success;
//        }

//        protected LogUtility Logger;
//    }

//}
