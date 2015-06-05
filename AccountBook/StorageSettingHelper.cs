using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TimeCost
{
    public class StorageSettingHelper
    {
        private static ApplicationDataContainer localSettings = null;
        private static ApplicationDataContainer dataContainer = null;
        private  const string containerName = "DataContainer";


        public static ApplicationDataContainer GetDataContainer()
        {
            if (localSettings==null)
            {
                localSettings = ApplicationData.Current.LocalSettings;
            }
            if (dataContainer==null)
            {
                dataContainer = localSettings.CreateContainer(containerName,
                    ApplicationDataCreateDisposition.Always);
            }
            return dataContainer;
        }
       
        public static bool Exist(string key)
        {
            return GetDataContainer().Values.Keys.Contains(key);
        }

        public static object Load(string key)
        {
            if (Exist(key))
            {
                return GetDataContainer().Values[key];
            }
            return null;
        }

        public static void Save(string key, object obj)
        {
            GetDataContainer().Values[key] = obj; 
        }

    }
}
