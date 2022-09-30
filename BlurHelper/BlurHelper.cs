using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlurHelper.BlurHelper
{
    public static class BlurHelper
    {
        public const ulong SourceVideoFrameCount = 252433;
        public const uint SourceVideoWidth = 1920;
        public const uint SourceVideoHeight = 1080;

        public static string SourceVideoPath = "E:\\TITOAAOGMI99MOL.mp4";
        public static string SaveDataPath = "E:\\TITOAAOGMI99MOL.blurhelper";
        public static string AutosavesPath = "{Directory}\\AutoSaves\\{BackupID}.blurhelper";
        public static string FramePath = "E:\\Frames\\{FrameID}.png";

        public static string FrameIDMarker = "{FrameID}";
        public static string BackupIDMarker = "{BackupID}";
        public static string ExePathMarker = "{Path}";
        public static string ExeDirectoryMarker = "{Directory}";

        public static System.TimeSpan AutoSaveInterval = new System.TimeSpan(30 * 10000000);

        public static SaveData CurrentSaveData = new SaveData();

        public static long lastSaveTime = 0;
        public static string lastSaveString = "";

        [System.STAThread]
        public static void Main(string[] args)
        {

            BlurHelperGame blurHelperGame = new BlurHelperGame();
            blurHelperGame.Run();
            Save();
        }
        public static void Load()
        {
            if (!System.IO.File.Exists(SaveDataPath))
            {
                CurrentSaveData = new SaveData();

                return;
            }

            string serializedData = System.IO.File.ReadAllText(SaveDataPath);

            CurrentSaveData = new SaveData(serializedData);

            lastSaveTime = System.DateTime.Now.Ticks;
        }
        public static void Save()
        {
            string serializedData = CurrentSaveData.ToString();
            System.IO.File.WriteAllText(SaveDataPath, serializedData);
            return;
            if (serializedData != lastSaveString)
            {
                lastSaveString = serializedData;

                if (System.IO.File.Exists(SaveDataPath))
                {
                    throw null;
                    int backID = 0;
                    while (System.IO.File.Exists(ExePathMarker))
                    {
                        backID++;
                    }

                    //System.IO.File.Move(SaveDataPath, SaveDataFolder + $"\\Backup {backID}.txt");
                }

                System.IO.File.WriteAllText(SaveDataPath, serializedData);

                lastSaveTime = System.DateTime.Now.Ticks;

                System.Console.WriteLine("Saved!");
            }
        }
        public static void CheckSave()
        {
            if (System.DateTime.Now.Ticks - lastSaveTime > 60 * 10000000)
            {
                Save();
            }
        }
    }
}
