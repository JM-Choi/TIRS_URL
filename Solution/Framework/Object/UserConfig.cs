#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Reflection;
#endregion

#region Program
namespace TechFloor.Object
{
    // %AppData%\Local 위치
    // Environment.SpecialFolder.LocalApplicationData
    // %AppData%\Roaming 위치
    // Environment.SpecialFolder.ApplicationData
    // 저장위치 - C:\Users\사용자명\AppData\Local\프로젝트명\프로젝트버전\프로젝트명.config

    public class UserConfig
    {
        #region Fields
        // protected string projectName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        // 
        // protected string projectVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        // 
        // protected string configPath = 
        //     System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        //     projectName,
        //     projectVersion);
        // 
        // protected string configPathAndName = 
        //     System.IO.Path.Combine(configPath, projectName + ".config");
        #endregion
        
        // 설정 파일 열기
        public Configuration OpenConfiguration(string file)
        {
            Configuration config_ = null;

            if (String.IsNullOrWhiteSpace(file))
                config_ = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            else
            {
                ExeConfigurationFileMap map_ = new ExeConfigurationFileMap();
                map_.ExeConfigFilename = file;
                config_ = ConfigurationManager.OpenMappedExeConfiguration(map_, ConfigurationUserLevel.None);
            }

            return config_;
        }

        // 설정 추가하기
        public void SetAppConfig(string key, string val, string file)
        {
            try
            {
                Configuration config_ = this.OpenConfiguration(file);

                if (config_.AppSettings.Settings.AllKeys.Contains(key))
                    config_.AppSettings.Settings[key].Value = val;
                else
                    config_.AppSettings.Settings.Add(key, val);

                config_.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(config_.AppSettings.SectionInformation.Name);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        // 설정 가져오기
        public string GetAppConfig(string key, string file)
        {
            Configuration config_ = this.OpenConfiguration(file);
            string val_ = string.Empty;

            if (config_.AppSettings.Settings.AllKeys.Contains(key))
                val_ = config_.AppSettings.Settings[key].Value;

            return val_;
        }

        //  설정 삭제하기
        public void RemoveAppConfig(string key, string file)
        {
            Configuration config_ = this.OpenConfiguration(file);

            if (config_.AppSettings.Settings.AllKeys.Contains(key))
            {
                config_.AppSettings.Settings.Remove(key);
                config_.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(config_.AppSettings.SectionInformation.Name);
            }
        }
    }
}
#endregion