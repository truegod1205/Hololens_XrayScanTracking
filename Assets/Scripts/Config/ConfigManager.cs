using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Config
{
    public class GraphScanConfig
    {
        public string language;
        // public float screenWidth;
        // public float screenHeight;
        public int horizontalPartition;
        public int verticalPartition;
        public float perGridTimeLimit;
    }

    public class ConfigManager : MonoBehaviour
    {
        private static ConfigManager instance = null;
        public static ConfigManager Instance
        {
            get
            {
                return instance;
            }
        }

        private GraphScanConfig config;

        public GraphScanConfig Config
        {
            get
            {
                return this.config;
            }
        }

        private string defaultLanguage = "Chinese";

        private float defaultScreenWidth = 50f;

        private float defaultScreenHeight = 31f;

        private int defaultHorizontalPartition = 10;

        private int defaultVerticalPartition = 6;

        private float defaulPerGridTimeLimit = 0.5f;

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
            this.config = new GraphScanConfig();
        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadConfig()
        {
            var filePath = Application.persistentDataPath + "/config.json";
            if (!File.Exists(filePath))
            {
                this.config.language = this.defaultLanguage;
                // this.config.screenWidth = this.defaultScreenWidth;
                // this.config.screenHeight = this.defaultScreenHeight;
                this.config.horizontalPartition = this.defaultHorizontalPartition;
                this.config.verticalPartition = this.defaultVerticalPartition;
                this.config.perGridTimeLimit = this.defaulPerGridTimeLimit;
                string output = JsonConvert.SerializeObject(this.config);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(output);
                sw.Close();
            }
            else
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    var readedConfig = sr.ReadToEnd();
                    this.config = JsonConvert.DeserializeObject<GraphScanConfig>(readedConfig);
                }
            }
        }

        public void SaveConfig(GraphScanConfig saveConfig)
        {
            var filePath = Application.persistentDataPath + "/config.json";
            this.config.language = saveConfig.language;
            // this.config.screenWidth = saveConfig.screenWidth;
            // this.config.screenHeight = saveConfig.screenHeight;
            this.config.horizontalPartition = saveConfig.horizontalPartition;
            this.config.verticalPartition = saveConfig.verticalPartition;
            this.config.perGridTimeLimit = saveConfig.perGridTimeLimit;
            string output = JsonConvert.SerializeObject(this.config);

            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(output);
            sw.Close();
        }
    }
}