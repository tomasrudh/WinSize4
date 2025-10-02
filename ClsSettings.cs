using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace WinSize4
{
    public class ClsSettings
    {
        private readonly string _portablePath = Path.GetDirectoryName(Application.ExecutablePath);
        private readonly string _localDataPath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "WinSize4");
        private readonly string _fileNameSettings = "Settings.json";

        private string _activePath; // This will be set at startup and used by the app
        public string ActivePath => _activePath; // It provides controlled, read-only access to the path from outside the class.

        public bool PortableMode { get; set; } = true;
        public string HotKeyLeft { get; set; } = "alt";
        public string HotKeyRight { get; set; } = "ctrl";
        public string HotKeyCharacter { get; set; } = "Z";
        public bool showAllWindows { get; set; } = true;
        public bool resetIfNewScreen { get; set; } = false;
        public bool runAtLogin { get; set; } = false;
        public bool Debug { get; set; } = false;
        public int Interval { get; set; } = 500;
        public bool isPaused { get; set; } = false;
        public bool ResetOnMinimize { get; set; } = false;
        public Dictionary<string, int> ListViewColumnWidths { get; set; }

        //**********************************************
        /// <summary>
        /// Backs up existing .json files in a directory to a timestamped subfolder.
        /// </summary>
        /// <param name="directoryToBackup">The folder containing files to be backed up.</param>
        //**********************************************
        private void BackupExistingFiles(string directoryToBackup)
        {
            try
            {
                if (!Directory.Exists(directoryToBackup)) return;

                var filesToBackup = Directory.GetFiles(directoryToBackup, "*.json");

                // If there are no config files, there's nothing to do.
                if (filesToBackup.Length == 0) return;

                // Create a unique, timestamped backup folder.
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
                string backupSubfolder = Path.Combine(directoryToBackup, $"_backup_{timestamp}");
                Directory.CreateDirectory(backupSubfolder);

                // Move each config file into the new backup folder.
                foreach (var sourceFile in filesToBackup)
                {
                    string fileName = Path.GetFileName(sourceFile);
                    string backupDestFile = Path.Combine(backupSubfolder, fileName);
                    File.Move(sourceFile, backupDestFile);
                }
            }
            catch (Exception ex)
            {
                // Inform the user if the backup failed, but don't crash the app.
                MessageBox.Show($"Could not create a backup of the old settings.\nError: {ex.Message}", "WinSize4 Backup Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //**********************************************
        /// <summary>
        /// Copies all .json files from a source to a destination, overwriting them.
        /// </summary>
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
        /// Implements the "Portable is King" startup logic.
        /// Determines the correct path at startup and loads settings.
        /// This replaces the direct call to LoadFromFile() in frmMain.
        /// </summary>
        //**********************************************
        public void InitializeAndLoad()
        {
            string portableConfigFile = Path.Combine(_portablePath, _fileNameSettings);
            string localConfigFile = Path.Combine(_localDataPath, _fileNameSettings);
            string sourceOfTruthPath;

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
                MessageBox.Show($"Error loading settings from {settingsDirPath}:\n{e.Message}", "WinSize4", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //**********************************************
        /// <summary> Saves data to disk </summary>
        //**********************************************
        public void SaveToFile()
        {
            // Ensure the active path is set if it hasn't been already (for first run).
            if (string.IsNullOrEmpty(_activePath))
            {
                _activePath = this.PortableMode ? _portablePath : _localDataPath;
            }

            ClsSettingsList saveList = new ClsSettingsList();
            saveList.PortableMode = this.PortableMode;
            saveList.HotKeyCharacter = this.HotKeyCharacter;
            saveList.HotKeyLeft = this.HotKeyLeft;
            saveList.HotKeyRight = this.HotKeyRight;
            saveList.showAllWindows = this.showAllWindows;
            saveList.resetIfNewScreen = this.resetIfNewScreen;
            saveList.runAtLogin = this.runAtLogin;
            saveList.Debug = ClsDebug.Debug;
            saveList.Interval = this.Interval;
            saveList.isPaused = this.isPaused;
            saveList.ResetOnMinimize = this.ResetOnMinimize;
            saveList.ListViewColumnWidths = this.ListViewColumnWidths;

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            // Use the active path
            Directory.CreateDirectory(_activePath);
            using (var writer = new StreamWriter(Path.Combine(_activePath, _fileNameSettings)))
            {
                String _json = JsonSerializer.Serialize(saveList, options);
                writer.Write(_json);
            }
        }

        public int GetHotKeyNumber(string HotKey)
        {
            switch (HotKey)
            {
                case "Alt": return 1;
                case "Ctrl": return 2;
                case "Shift": return 4;
                default: return 0;
            }
        }
        public string GetHotKeyText(int HotKey)
        {
            switch (HotKey)
            {
                case 1: return "Alt";
                case 2: return "Ctrl";
                case 4: return "Shift";
                default: return "None";
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
        public bool ResetOnMinimize
        { get; set; }
        public Dictionary<string, int> ListViewColumnWidths
        { get; set; }
        public bool PortableMode
        { get; set; } = true;
    }
}
