namespace OysterVPNLibrary
{
    using System;
    using System.IO;

    public class Storage
    {
        private static string CheckDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        public static string UserDataFolder
        {
            get
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return CheckDir($"{folderPath}\\{"OysterVPN"}\\");
            }
        }

        public static string UserRoamingDataFolder
        {
            get
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return CheckDir($"{folderPath}\\{"OysterVPN"}\\");
            }
        }

        public static string AllUsersDataFolder
        {
            get
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                return CheckDir($"{folderPath}\\{"OysterVPN"}\\");
            }
        }
    }
}

