using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace WinSize4
{
    public class ClsSettings
    {
        //public ClsSettingsList SettingsList = new ClsSettingsList();
        public string HotKeyLeft = "alt";
        public string HotKeyRight = "ctrl";
        public string HotKeyCharacter = "Z";
        public bool showAllWindows = true;
        public bool resetIfNewScreen = false;
        public bool runAtLogin = false;
        private string _path = Environment.GetEnvironmentVariable("LocalAppData") + "\\WinSize4";
        private string _fileNameWindows = "Settings.json";
        public bool Debug = false;
        public int Interval = 500;
        public bool isPaused = false;

        //**********************************************
        /// <summary> Saves data to disk </summary>
        //**********************************************
        public void SaveToFile()
        {
            ClsSettingsList saveList = new ClsSettingsList();
            saveList.HotKeyCharacter = this.HotKeyCharacter;
            saveList.HotKeyLeft = this.HotKeyLeft;
            saveList.HotKeyRight = this.HotKeyRight;
            saveList.showAllWindows = this.showAllWindows;
            saveList.resetIfNewScreen = this.resetIfNewScreen;
            saveList.runAtLogin = this.runAtLogin;
            saveList.Debug = ClsDebug.Debug;
            saveList.Interval = this.Interval;
            saveList.isPaused = this.isPaused;

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            Directory.CreateDirectory(_path);
            using (var writer = new StreamWriter(_path + "\\" + _fileNameWindows))
            {
                String _json = JsonSerializer.Serialize(saveList, options);
                writer.Write(_json);
            }
        }

        //**********************************************
        /// <summary> Loads data from disk </summary>
        //**********************************************
        private void CopyAllFiles(string sourceDir, string destDir)
        {
            BackupExistingFiles(destDir); // Back up the destination directory BEFORE copying new files into it.

            Directory.CreateDirectory(destDir);
            var filesToCopy = Directory.GetFiles(sourceDir, "*.json");

            foreach (var sourceFile in filesToCopy)
            {
                string fileName = Path.GetFileName(sourceFile);
                string destFile = Path.Combine(destDir, fileName);
                File.Copy(sourceFile, destFile, true); // 'true' means overwrite if exists
            }
        }

        //**********************************************
        /// <summary>
        /// Safely checks for write permissions in a directory by attempting to create and delete a temporary file.
        /// </summary>
        /// <param name="path">The directory to check.</param>
        /// <returns>True if write access is granted, otherwise false.</returns>
        //**********************************************
        private bool HasWriteAccess(string path)
        {
            try
            {
                // Ensure the directory exists for an accurate test.
                Directory.CreateDirectory(path);
                // Use a unique file name to avoid collisions.
                string tempFile = Path.Combine(path, Guid.NewGuid().ToString() + ".tmp");
                File.WriteAllText(tempFile, "test");
                File.Delete(tempFile);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                // This is the specific exception we are looking for.
                return false;
            }
            catch (Exception)
            {
                // Any other exception also indicates a problem.
                return false;
            }
        }

        //**********************************************
        /// <summary>
        /// Implements the "Portable is King" startup logic.
        /// Determines the correct path at startup and loads settings.
        /// This replaces the direct call to LoadFromFile() in frmMain.
        /// </summary>
        //**********************************************
        public void InitializeAndLoad()
        {
            string portableConfigFile = Path.Combine(_portablePath, _fileNameSettings);
            string localConfigFile = Path.Combine(_localDataPath, _fileNameSettings);
            string sourceOfTruthPath = null;

            // 1. ESTABLISH THE SOURCE OF TRUTH (The Golden Rule)
            if (File.Exists(portableConfigFile))
            {
                sourceOfTruthPath = _portablePath;
            }
            else if (File.Exists(localConfigFile))
            {
                sourceOfTruthPath = _localDataPath;
            }
            else
            {
                // First-ever run. Default to portable mode.
                _activePath = _portablePath;
                this.PortableMode = true;
                return; // Nothing to load or copy.
            }

            // 2. LOAD the settings from the determined source of truth.
            LoadSettingsFromSpecificPath(sourceOfTruthPath); // This populates 'this.PortableMode' correctly.

            // 3. DETERMINE the correct active path based on the loaded setting.
            _activePath = this.PortableMode ? _portablePath : _localDataPath;

            // 4. SYNCHRONIZE: Copy all config files from the source of truth to the target active path if they are different.
            if (sourceOfTruthPath != _activePath)
            {
                CopyAllFiles(sourceOfTruthPath, _activePath);
            }
        }

        //**********************************************
        /// <summary>
        /// This is called by the UI when the checkbox is changed by the user.
        /// </summary>
        //**********************************************
        public void UpdateSettingsLocation()
        {
            string newPath = this.PortableMode ? _portablePath : _localDataPath;

            // If the new path is different from the current one, copy files.
            if (newPath != _activePath)
            {
                // The source is the CURRENT active path.
                CopyAllFiles(_activePath, newPath);

                // Update the active path for the rest of the session.
                _activePath = newPath;
            }
        }

        //**********************************************
        /// <summary>
        /// Loads settings from a specific file path and populates the class properties.
        /// </summary>
        //**********************************************
        private void LoadSettingsFromSpecificPath(string settingsDirPath)
        {
            try
            {
                string settingsFile = Path.Combine(settingsDirPath, _fileNameSettings);
                if (File.Exists(settingsFile))
                {
                    using (StreamReader r = new StreamReader(settingsFile))
                    {
                        String json = r.ReadToEnd();
                        var saveList = JsonSerializer.Deserialize<ClsSettingsList>(json);

                        // Populate all properties from the loaded list
                        this.PortableMode = saveList.PortableMode;
                        this.HotKeyCharacter = saveList.HotKeyCharacter;
                        this.HotKeyLeft = saveList.HotKeyLeft;
                        // ... (and so on for all your properties)
                        this.HotKeyRight = saveList.HotKeyRight;
                        this.showAllWindows = saveList.showAllWindows;
                        this.resetIfNewScreen = saveList.resetIfNewScreen;
                        this.runAtLogin = saveList.runAtLogin;
                        ClsDebug.Debug = saveList.Debug;
                        if (saveList.Interval > 0) this.Interval = saveList.Interval;
                        this.isPaused = saveList.isPaused;
                        this.ResetOnMinimize = saveList.ResetOnMinimize;
                        this.ListViewColumnWidths = saveList.ListViewColumnWidths;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "WinSize4", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public int GetHotKeyNumber(string HotKey)
        {
            switch (HotKey)
            {
                case "Alt":
                    return 1;
                case "Ctrl":
                    return 2;
                case "Shift":
                    return 4;
                default:
                    return 0;
            }
        }
        public string GetHotKeyText(int HotKey)
        {
            switch (HotKey)
            {
                case 1:
                    return "Alt";
                case 2:
                    return "Ctrl";
                case 4:
                    return "Shift";
                default:
                    return "None";
            }
        }
    }

    public class ClsSettingsList
    {
        public string HotKeyLeft
        { set; get; }
        public string HotKeyRight
        { set; get; }
        public string HotKeyCharacter
        { set; get; }
        public bool showAllWindows
        { set; get; }
        public bool resetIfNewScreen
        { set; get; }
        public bool runAtLogin
        { set; get; }
        public bool Debug
        { set; get; }
        public int Interval
        { set; get; }
        public bool isPaused
        { set; get; }
    }
}
