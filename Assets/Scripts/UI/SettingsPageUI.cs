using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class SettingsPageUI : MonoBehaviour
    {
        [SerializeField]
        private MainUI mainUI;

        [SerializeField]
        private TextMesh languageText;

        [SerializeField]
        private TextMesh screenWidthText;

        [SerializeField]
        private TextMesh screenHeightText;

        [SerializeField]
        private TextMesh horizontalPartitionText;

        [SerializeField]
        private TextMesh verticalPartitionText;

        [SerializeField]
        private TextMesh perGridTimeLimitText;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InitPage()
        {
            string LocalizationKey = "";
            if (Config.ConfigManager.Instance.Config.language == "Chinese")
            {
                LocalizationKey = "Main_Settings_Language_Chinese";
            }
            else
            {
                LocalizationKey = "Main_Settings_Language_English";
            }
            this.languageText.text = Localization.LocalizationManager.Instance.GetLocalizationContent(LocalizationKey);
            // this.screenWidthText.text = Config.ConfigManager.Instance.Config.screenWidth.ToString() + "(cm)";
            // this.screenHeightText.text = Config.ConfigManager.Instance.Config.screenHeight.ToString() + "(cm)";
            this.horizontalPartitionText.text = Config.ConfigManager.Instance.Config.horizontalPartition.ToString();
            this.verticalPartitionText.text = Config.ConfigManager.Instance.Config.verticalPartition.ToString();
            this.perGridTimeLimitText.text = Config.ConfigManager.Instance.Config.perGridTimeLimit.ToString("#0.00") + "(sec)";
        }

        public void BackToMainMenuPage()
        {
            this.mainUI.BackToMainMenuPage(this.gameObject);
        }

        public void SetLanguage()
        {
            var config = new Config.GraphScanConfig();
            // config.screenWidth = Config.ConfigManager.Instance.Config.screenWidth;
            // config.screenHeight = Config.ConfigManager.Instance.Config.screenHeight;
            config.horizontalPartition = Config.ConfigManager.Instance.Config.horizontalPartition;
            config.verticalPartition = Config.ConfigManager.Instance.Config.verticalPartition;
            config.perGridTimeLimit = Config.ConfigManager.Instance.Config.perGridTimeLimit;
            string LocalizationKey = "";
            if (Config.ConfigManager.Instance.Config.language == "Chinese")
            {
                config.language = "English";
                // need fix
                LocalizationKey = "Main_Settings_Language_English";
            }
            else
            {
                config.language = "Chinese";
                LocalizationKey = "Main_Settings_Language_Chinese";
            }
            Config.ConfigManager.Instance.SaveConfig(config);
            Localization.LocalizationManager.Instance.SetLanguage();
            this.languageText.text = Localization.LocalizationManager.Instance.GetLocalizationContent(LocalizationKey);
        }

        public void SetScreenWidth(float diff)
        {
            /*
            if (Config.ConfigManager.Instance.Config.screenWidth + diff < 0f || 
                Config.ConfigManager.Instance.Config.screenWidth + diff > 150f)
            {
                return;
            }
            var config = new Config.GraphScanConfig();
            config.language = Config.ConfigManager.Instance.Config.language;
            config.screenWidth = Config.ConfigManager.Instance.Config.screenWidth + diff;
            config.screenHeight = Config.ConfigManager.Instance.Config.screenHeight;
            config.horizontalPartition = Config.ConfigManager.Instance.Config.horizontalPartition;
            config.verticalPartition = Config.ConfigManager.Instance.Config.verticalPartition;
            config.perGridTimeLimit = Config.ConfigManager.Instance.Config.perGridTimeLimit;
            Config.ConfigManager.Instance.SaveConfig(config);
            this.screenWidthText.text = Config.ConfigManager.Instance.Config.screenWidth.ToString() + "(cm)";
            */
        }

        public void SetScreenHeight(float diff)
        {
            /*
            if (Config.ConfigManager.Instance.Config.screenHeight + diff < 0f ||
                Config.ConfigManager.Instance.Config.screenHeight + diff > 150f)
            {
                return;
            }
            var config = new Config.GraphScanConfig();
            config.language = Config.ConfigManager.Instance.Config.language;
            config.screenWidth = Config.ConfigManager.Instance.Config.screenWidth;
            config.screenHeight = Config.ConfigManager.Instance.Config.screenHeight + diff;
            config.horizontalPartition = Config.ConfigManager.Instance.Config.horizontalPartition;
            config.verticalPartition = Config.ConfigManager.Instance.Config.verticalPartition;
            config.perGridTimeLimit = Config.ConfigManager.Instance.Config.perGridTimeLimit;
            Config.ConfigManager.Instance.SaveConfig(config);
            this.screenHeightText.text = Config.ConfigManager.Instance.Config.screenHeight.ToString() + "(cm)";
            */
        }

        public void SetHorizontalPartition(int diff)
        {
            if (Config.ConfigManager.Instance.Config.horizontalPartition + diff < 0 ||
                Config.ConfigManager.Instance.Config.horizontalPartition + diff > 30)
            {
                return;
            }
            var config = new Config.GraphScanConfig();
            config.language = Config.ConfigManager.Instance.Config.language;
            // config.screenWidth = Config.ConfigManager.Instance.Config.screenWidth;
            // config.screenHeight = Config.ConfigManager.Instance.Config.screenHeight;
            config.horizontalPartition = Config.ConfigManager.Instance.Config.horizontalPartition + diff;
            config.verticalPartition = Config.ConfigManager.Instance.Config.verticalPartition;
            config.perGridTimeLimit = Config.ConfigManager.Instance.Config.perGridTimeLimit;
            Config.ConfigManager.Instance.SaveConfig(config);
            this.horizontalPartitionText.text = Config.ConfigManager.Instance.Config.horizontalPartition.ToString();
        }

        public void SetVerticalPartition(int diff)
        {
            if (Config.ConfigManager.Instance.Config.verticalPartition + diff < 0 ||
                Config.ConfigManager.Instance.Config.verticalPartition + diff > 30)
            {
                return;
            }
            var config = new Config.GraphScanConfig();
            config.language = Config.ConfigManager.Instance.Config.language;
            // config.screenWidth = Config.ConfigManager.Instance.Config.screenWidth;
            // config.screenHeight = Config.ConfigManager.Instance.Config.screenHeight;
            config.horizontalPartition = Config.ConfigManager.Instance.Config.horizontalPartition;
            config.verticalPartition = Config.ConfigManager.Instance.Config.verticalPartition + diff;
            config.perGridTimeLimit = Config.ConfigManager.Instance.Config.perGridTimeLimit;
            Config.ConfigManager.Instance.SaveConfig(config);
            this.verticalPartitionText.text = Config.ConfigManager.Instance.Config.verticalPartition.ToString();
        }

        public void SetPerGridTimeLimit(float diff)
        {
            if (Config.ConfigManager.Instance.Config.perGridTimeLimit + diff < 0f ||
                Config.ConfigManager.Instance.Config.perGridTimeLimit + diff > 1.5f)
            {
                return;
            }
            var config = new Config.GraphScanConfig();
            config.language = Config.ConfigManager.Instance.Config.language;
            // config.screenWidth = Config.ConfigManager.Instance.Config.screenWidth;
            // config.screenHeight = Config.ConfigManager.Instance.Config.screenHeight;
            config.horizontalPartition = Config.ConfigManager.Instance.Config.horizontalPartition;
            config.verticalPartition = Config.ConfigManager.Instance.Config.verticalPartition;
            config.perGridTimeLimit = Config.ConfigManager.Instance.Config.perGridTimeLimit + diff;
            Config.ConfigManager.Instance.SaveConfig(config);
            this.perGridTimeLimitText.text = Config.ConfigManager.Instance.Config.perGridTimeLimit.ToString("#0.00") + "(sec)";
        }
    }
}