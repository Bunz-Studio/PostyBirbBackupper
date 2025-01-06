using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace PostyBirbBackupper
{
    public static class PostyBirb
    {
        public static string BrowserProfilesPath 
        { 
            get {
                switch (Environment.OSVersion.Platform) {
                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                        return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Roaming\\postybirb-plus";
                    case PlatformID.Unix:
                        return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.config/postybirb-plus";
                    default:
                        // Other versions are not supported yet, if you have a mac, please contribute...
                        return string.Empty;
                }
            }
        }

        public static string ConfigPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/PostyBirb";
            }
        }

        public static bool IsPostyBirbDataAvailable()
        {
            return Directory.Exists(BrowserProfilesPath) && Directory.Exists(ConfigPath);
        }

        public static FastZip CreateBackupFile(string path)
        {
            // Copy the files to a cache folder
            var cacheFolder = Path.Combine(Path.GetTempPath(), "postybirb-backupper");
            if (Directory.Exists(cacheFolder))
                Directory.Delete(cacheFolder, true);

            Directory.CreateDirectory(cacheFolder);

            DirectoryCopy(ConfigPath, Path.Combine(cacheFolder, "PostyBirb"), true);
            DirectoryCopy(BrowserProfilesPath, Path.Combine(cacheFolder, "postybirb-plus"), true);

            var zip = new FastZip();
            zip.CreateZip(path, cacheFolder, true, null);

            Directory.Delete(cacheFolder, true);
            return zip;
        }

        public static FastZip RestoreFromBackupFile(string path)
        {
            // Copy the files to a cache folder
            var cacheFolder = Path.Combine(Path.GetTempPath(), "postybirb-backupper");
            if (Directory.Exists(cacheFolder))
                Directory.Delete(cacheFolder, true);

            Directory.CreateDirectory(cacheFolder);

            var zip = new FastZip();
            zip.ExtractZip(path, cacheFolder, null);

            if (Directory.Exists(ConfigPath))
                Directory.Delete(ConfigPath, true);
            DirectoryCopy(Path.Combine(cacheFolder, "PostyBirb"), ConfigPath, true);

            if (Directory.Exists(BrowserProfilesPath))
                Directory.Delete(BrowserProfilesPath, true);
            DirectoryCopy(Path.Combine(cacheFolder, "postybirb-plus"), BrowserProfilesPath, true);
            return zip;
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                try
                {
                    if (file.Exists)
                        file.CopyTo(temppath, false);
                }
                catch (IOException) { Console.WriteLine("Unable to copy file: {0}", temppath); }
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}