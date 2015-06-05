using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace TimeCost
{
    public class StorageFileHelper
    {
        private const string FolderName="Data";
        private static IStorageFolder DataFolder = null;
        private static async Task<IStorageFolder> GetDataFolder()
        {
            if (DataFolder == null)
            {
                DataFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(FolderName, CreationCollisionOption.OpenIfExists);
            }
            return DataFolder;
        }

        public static async Task<string> ReadFileAsync(string fileName)
        {
            string text;
            try
            {
                IStorageFolder applicationFolder = await GetDataFolder();
                IStorageFile storageFile = await applicationFolder.GetFileAsync(fileName);
                IRandomAccessStream accessStream = await storageFile.OpenReadAsync();
                using (StreamReader streamReader = new StreamReader(accessStream.AsStreamForRead((int)accessStream.Size)))
                {
                    text = streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                text = "文件读取错误：" + e.Message;
            }
            return text;
        }

        public static async Task WriteFileAsync(string fileName, string content)
        {
            IStorageFolder applicationFolder = await GetDataFolder();
            IStorageFile storageFile = await applicationFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(storageFile, content);
        }

        public static async Task WriteAsync<T>(T data, string filename)
        {
            IStorageFolder applicationFolder = await GetDataFolder();
            StorageFile file = await applicationFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            using (IRandomAccessStream raStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    serializer.WriteObject(outStream.AsStreamForWrite(), data);
                    await outStream.FlushAsync();
                }
            }

        }

        public static async Task<T> ReadAsync<T>(string filename)
        {
            T sessionState_ = default(T);
            IStorageFolder applicationFolder = await GetDataFolder();
            StorageFile file = await applicationFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            if (file == null)
                return sessionState_;
            try
            {
                using (IInputStream inStream = await file.OpenSequentialReadAsync())
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    sessionState_ = (T)serializer.ReadObject(inStream.AsStreamForRead());
                }
            }
            catch(Exception)
            {

            }

            return sessionState_;
        }
    }
}
