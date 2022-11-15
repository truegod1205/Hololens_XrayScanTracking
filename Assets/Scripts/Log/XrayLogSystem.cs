using System;
using System.Threading.Tasks;
using UnityEngine;

#if WINDOWS_UWP
using Windows.Storage;
using Windows.System;
using Windows.Storage.Streams;
#endif

namespace XRayScan.Log 
{
    public class XrayLogSystem : MonoBehaviour
    {
#if WINDOWS_UWP
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        Windows.Storage.StorageFolder storageFolder =  Windows.Storage.KnownFolders.DocumentsLibrary;
#endif

        private static XrayLogSystem instance = null;
        public static XrayLogSystem Instance
        {
            get
            {
                return instance;
            }
        }


        public bool IsWritingResult { get; set; }
        private DateTime ExecuteTime;
        private string testXrayLogFileName = string.Empty;
        private string readingXrayLogFileName = string.Empty;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
            this.ExecuteTime = DateTime.Now;
            this.IsWritingResult = false;
            this.testXrayLogFileName = "TestModeLog_" + this.ExecuteTime.ToString("yyyyMMddhhmmss") + ".txt";
            this.readingXrayLogFileName = "ReadingModeLog_" + this.ExecuteTime.ToString("yyyyMMddhhmmss") + ".txt";
            Debug.Log(this.testXrayLogFileName);
            Debug.Log(this.readingXrayLogFileName);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool WriteXRayLog(XrayLog log) 
        {
            if (this.IsWritingResult)
            {
                return false;
            }
#if WINDOWS_UWP
            if (log.GetLogMode() == LogMode.TestMode)
            { 
                WriteXRayLogAsync(this.testXrayLogFileName, log.LogDataToString());
            }
            else 
            {
                WriteXRayLogAsync(this.readingXrayLogFileName, log.LogDataToString());
            }
#endif
            return true;
        }
#if WINDOWS_UWP
        private async Task WriteXRayLogAsync(string fileName, string logResult)
        {
            this.IsWritingResult = true;
            StorageFile sampleFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await FileIO.AppendTextAsync(sampleFile, logResult + "\r\n");
            this.IsWritingResult = false;
        }
#endif
    }
}
