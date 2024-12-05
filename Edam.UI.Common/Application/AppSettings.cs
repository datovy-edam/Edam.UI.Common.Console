using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// -----------------------------------------------------------------------------
using Edam.Serialization;
using app = Edam.Application;
using Edam.DataObjects.ReferenceData;
using data = Edam.Data;
using Edam.Data;
using Edam.Application;

namespace Edam.Application.Settings
{

    /// <summary>
    /// Manage App and UI-Settings...
    /// </summary>
    public class AppSettings : app.IAppSettings
    {
        private static EdamSettings EdamSettings = null;

        public const string TYPE_POSTFIX = "_Type";
        public const string DOCUMENT_POSTFIX = "_Document";

        public const string EDAM_SETTINGS_FILENAME_KEY = "EdamSettingsFileName";
        public const string EDAM_DATA_PATH = "AssetDataPath";

        public const string CONSOLE_PATH_KEY = "ConsolePath";
        public const string CONSOLE_DATA_PATH_KEY = "ConsoleDataPath";

        static AppSettings()
        {
            FromJson();
        }

        /// <summary>
        /// Get Type Postfix default or as found in settings.
        /// </summary>
        /// <returns>Postfix value is returned</returns>
        public string GetTypePostfix()
        {
            if (EdamSettings != null)
            {
                return EdamSettings.Schema.Defaults.TypePostfix;
            }
            return TYPE_POSTFIX;
        }

        /// <summary>
        /// Get Document Postfix default or as found in settings.
        /// </summary>
        /// <returns>Postfix value is returned</returns>
        public string GetDocumentPostfix()
        {
            if (EdamSettings != null)
            {
                return EdamSettings.Schema.Defaults.DocumentPostfix;
            }
            return DOCUMENT_POSTFIX;
        }

        public static void SetDataSource(DataSourceInfo dataSource)
        {
            SetConnectionString(dataSource.ConnectionString);
        }

        /// <summary>
        /// Set Connection string and store it on the Settings files as possible.
        /// </summary>
        /// <param name="connectionString">connection string</param>
        public static void SetConnectionString(string connectionString)
        {
            if (EdamSettings == null)
            {
                EdamSettings = FromJson(null);
                if (EdamSettings == null)
                {
                    // file don't exist so create an instance of the settings
                    EdamSettings = new EdamSettings();
                }
            }
            EdamSettings.DataSource.DefaultConnectionString = connectionString;
            ToJson(EdamSettings);
        }

        /// <summary>
        /// Get Console Path where application data is found.
        /// </summary>
        /// <returns>path is returned</returns>
        public static string GetConsolePath()
        {
            if (EdamSettings != null)
            {
                return EdamSettings.App.ConsolePath;
            }

            return app.AppSettings.GetSectionString(CONSOLE_PATH_KEY);
            //return Project.GetProjectsPath(false, true);
        }

        /// <summary>
        /// Get Settings Path.
        /// </summary>
        /// <returns>Settings Paht is returned</returns>
        public static string GetSettingsPath()
        {
            var path = app.AppSettings.GetSectionString(CONSOLE_DATA_PATH_KEY);
            string fname = app.AppSettings.GetSectionString(
               EDAM_SETTINGS_FILENAME_KEY);
            return path + fname;
        }

        /// <summary>
        /// Get URI list for all entries of given type.
        /// </summary>
        /// <param name="type">URI type to search</param>
        /// <returns>uri list of given type is returned</returns>
        public static List<UriItemInfo> GetUriList(UriType type)
        {
            VerifySetConnectionString();
            if (EdamSettings.App.UriList == null)
            {
                return new List<UriItemInfo>();
            }
            return EdamSettings.App.UriList.FindAll((x) => x.Type == type);
        }

        /// <summary>
        /// Set Reference Data connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        public static void SetReferenceDataConnectionString(
           string connectionString)
        {
            string kstring = ReferenceDataHelper.GetConnectionStringKey();
            if (!String.IsNullOrWhiteSpace(kstring))
            {
                var cstring = app.AppSettings.GetConnectionString(kstring);
                if (String.IsNullOrWhiteSpace(cstring))
                {
                    DataSources.AddDefaultConnectionString(
                       connectionString, kstring);
                }
            }
        }

        /// <summary>
        /// Set Default Connection String.
        /// </summary>
        /// <param name="connectionString">connection string</param>
        public static void SetDefaultConnectionString(string connectionString)
        {
            SetConnectionString(connectionString);
            var defaultKey = DataSources.GetDefaultDatabaseKey();
            DataSources.AddDefaultConnectionString(
               connectionString, defaultKey);

            // check reference data...
            SetReferenceDataConnectionString(connectionString);
        }

        /// <summary>
        /// Verify that a Connection String associated with default db keys are 
        /// available.  If not try setting up those.
        /// </summary>
        public static void VerifySetConnectionString()
        {
            if (EdamSettings == null)
            {
                EdamSettings = FromJson(null);
                if (EdamSettings == null)
                {
                    EdamSettings = new EdamSettings();
                }
            }

            // get or set default connection string if available at this time...
            string cstring = String.IsNullOrWhiteSpace(
               EdamSettings.DataSource.DefaultConnectionString) ?
                  DataSources.DEFAULT_CONNECTION_STRING :
                  EdamSettings.DataSource.DefaultConnectionString;

            // check "Default" key
            var key = DataSources.GetDefaultDatabaseKey();
            if (!String.IsNullOrWhiteSpace(key))
            {
                var dstring = Session.DataSourceCollection.Find(key);
                if (dstring == null)
                {
                    SetDefaultConnectionString(cstring);
                }
            }

            // check "ReferenceData" key
            key = ReferenceDataHelper.GetConnectionStringKey();
            if (!String.IsNullOrWhiteSpace(key))
            {
                var dstring = Session.DataSourceCollection.Find(key);
                if (dstring == null)
                {
                    SetReferenceDataConnectionString(cstring);
                }
            }
        }

        /// <summary>
        /// Get EdamSettings from the JSON file.
        /// </summary>
        /// <param name="jsonFilePath">JSON file path</param>
        /// <returns>instance of EdamSettings is returned</returns>
        public static EdamSettings FromJson(string jsonFilePath = null)
        {
            string fpath = String.IsNullOrWhiteSpace(jsonFilePath) ?
               GetSettingsPath() : jsonFilePath;
            EdamSettings setts = null;
            if (File.Exists(fpath))
            {
                string jsonText = File.ReadAllText(fpath);
                setts = JsonSerializer.Deserialize<EdamSettings>(jsonText);

                // reset values as needed
                if (String.IsNullOrWhiteSpace(setts.App.ConsolePath))
                {
                    setts.App.ConsolePath = GetConsolePath();
                }

                foreach (var i in setts.App.UriList)
                {
                    // TODO: remove hardcoded value...
                    if (String.IsNullOrWhiteSpace(i.UriText) &&
                       i.Type == UriType.ConsolePath && i.Name == "Default")
                    {
                        i.UriText = "";
                    }
                }
            }
            EdamSettings = setts;
            return setts;
        }

        /// <summary>
        /// To JSON file.
        /// </summary>
        /// <param name="settings">settings to store as JSON</param>
        /// <param name="jsonFilePath">file path for the JSON file</param>
        public static void ToJson(
           object settings, string jsonFilePath = null)
        {
            string fpath = String.IsNullOrWhiteSpace(jsonFilePath) ?
               GetSettingsPath() : jsonFilePath;

            EdamSettings edamSettings = settings as EdamSettings;
            if (edamSettings == null)
            {
                return;
            }

            var jsonText = JsonSerializer.Serialize<EdamSettings>(edamSettings);
            if (jsonText != null)
            {
                File.WriteAllText(fpath, jsonText);
            }
        }

    }

}
