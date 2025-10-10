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

            // 1. Establish the source of truth (Portable is King).
            if (File.Exists(portableConfigFile))
            {
                sourceOfTruthPath = _portablePath;
            }
            else if (File.Exists(localConfigFile))
            {
                sourceOfTruthPath = _localDataPath;
            }

            // 2. Load settings from the source if found, otherwise use defaults.
            if (sourceOfTruthPath != null)
            {
                LoadSettingsFromSpecificPath(sourceOfTruthPath);
            }
            else
            {
                // This is a first-ever run. 'this.PortableMode' is already 'true' by default.
            }

            // 3. Determine the INTENDED path based on settings.
            string intendedPath = this.PortableMode ? _portablePath : _localDataPath;

            // 4. Test the INTENDED path for write access.
            if (HasWriteAccess(intendedPath))
            {
                // --- SUCCESS: The intended path is writable. ---
                _activePath = intendedPath;
                // Synchronize files if the source is different from our active directory.
                if (sourceOfTruthPath != null && sourceOfTruthPath != _activePath)
                {
                    CopyAllFiles(sourceOfTruthPath, _activePath);
                }
            }
            else
            {
                // --- FAILURE: The intended path is NOT writable. We must try to fall back. ---
                if (this.PortableMode) // The failed path was the portable directory.
                {
                    // Inform the user we are attempting to fall back to LocalAppData.
                    MessageBox.Show(
                        "WinSize4 does not have permission to save settings in its current location.\n\n" +
                        "It will now attempt to use the safe 'LocalAppData' folder as a fallback.",
                        "Permission Issue Detected",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    // Now, test the fallback path.
                    if (HasWriteAccess(_localDataPath))
                    {
                        // --- FALLBACK SUCCESS ---
                        this.PortableMode = false; // Force non-portable mode.
                        _activePath = _localDataPath;
                        // Copy the original files to the new, writable location.
                        if (sourceOfTruthPath != null)
                        {
                            CopyAllFiles(sourceOfTruthPath, _activePath);
                        }
                    }
                    else
                    {
                        // --- FATAL ERROR: Both paths are unwritable. ---
                        ShowFatalErrorAndExit();
                    }
                }
                else // The failed path was LocalAppData itself.
                {
                    // --- FATAL ERROR: The primary non-portable path is unwritable. There is no other fallback. ---
                    ShowFatalErrorAndExit();
                }
            }
        }

        //**********************************************
        /// <summary>
        /// Displays a final error message and terminates the application when no writable path is found.
        /// </summary>
        //**********************************************
        private void ShowFatalErrorAndExit()
        {
            MessageBox.Show(
                "WinSize4 could not find a writable location to save its settings.\n\n" +
                "It has checked both its application folder and the LocalAppData folder and was denied access to both.\n\n" +
                "Please check your system's permissions or run the application from a user-writable folder.\nThe application will now close.",
                "Fatal Error: Cannot Save Settings",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            Environment.Exit(1); // Force the application to terminate.
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
