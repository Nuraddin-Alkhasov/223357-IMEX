using HMI.Views.MessageBoxRegion;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;

namespace HMI.Services
{
    class DoBackup
    {
        string FolderName;
        string PathFolder;
        string[] activeDays;

        public DoBackup()
        {
            Application.Current.Dispatcher.InvokeAsync((Action)delegate
            {
                ApplicationService.SetView("TouchpadRegion", "WaitScreen", 0);
            });


            FolderName = DateTime.Now.Year.ToString() + "-" +
                (DateTime.Now.Month.ToString().Length == 2 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString()) + "-" +
                (DateTime.Now.Day.ToString().Length == 2 ? DateTime.Now.Day.ToString() : "0" + DateTime.Now.Day.ToString());
            PathFolder = @"D:\FP-HMI-Backup\";
            BackUP();
        }

        private void BackUP()
        {
            Task doTask = Task.Run(() => {
                try
                {
                    //prepare data
                    activeDays = new string[10];
                    for (int i = 0; i < 10; i++)
                    {
                        activeDays[i] = (DateTime.Now.AddDays(i * -1)).ToString("yyyy-MM-dd");
                    }
                    ClearData();

                    //create path
                    if (!Directory.Exists(PathFolder + FolderName))
                    {
                        System.IO.Directory.CreateDirectory(PathFolder + FolderName);
                    }
                    if (!Directory.Exists(PathFolder + FolderName + @"\Alarms"))
                    {
                        System.IO.Directory.CreateDirectory(PathFolder + FolderName + @"\Alarms");
                    }
                    for (int i = 1; i <= 7; i++) 
                    {
                        if (!Directory.Exists(PathFolder + FolderName + @"\Archive\ALO"+i.ToString()))
                        {
                            System.IO.Directory.CreateDirectory(PathFolder + FolderName + @"\Archive\ALO" + i.ToString());
                        }
                    }
                    for (int i = 1; i <= 3; i++)
                    {
                        if (!Directory.Exists(PathFolder + FolderName + @"\Archive\LGO" + i.ToString()))
                        {
                            System.IO.Directory.CreateDirectory(PathFolder + FolderName + @"\Archive\LGO" + i.ToString());
                        }
                    }
                    if (!Directory.Exists(PathFolder + FolderName + @"\DB"))
                    {
                        System.IO.Directory.CreateDirectory(PathFolder + FolderName + @"\DB");
                    }
                    if (!Directory.Exists(PathFolder + FolderName + @"\Logging"))
                    {
                        System.IO.Directory.CreateDirectory(PathFolder + FolderName + @"\Logging");
                    }
                    if (!Directory.Exists(PathFolder + FolderName + @"\Rezepte"))
                    {
                        System.IO.Directory.CreateDirectory(PathFolder + FolderName + @"\Rezepte");
                    }

                    //Alarms
                    string[] filePaths = Directory.GetFiles(@"D:\ForplanVisualization\Alarms");
                    foreach (var filename in filePaths)
                    {
                        if (filename.Contains(".VAA"))
                        {
                            string str = filename.Replace(@"D:\ForplanVisualization\Alarms", PathFolder + FolderName + @"\Alarms");
                            File.Copy(filename, str, true);
                        }
                    }
                    //Archive ALO
                    for (int i = 1; i <= 7; i++) 
                    {
                        filePaths = Directory.GetFiles(@"D:\ForplanVisualization\Archive\ALO" + i.ToString());
                        foreach (var filename in filePaths)
                        {
                            if (filename.Contains(".VWH"))
                            {
                                string str = filename.Replace(@"D:\ForplanVisualization\Archive\ALO" + i.ToString(), PathFolder + FolderName + @"\Archive\ALO" + i.ToString());
                                File.Copy(filename.ToString(), str, true);
                            }
                        }
                    }
                    //Archive LGO
                    for (int i = 1; i <= 3; i++)
                    {
                        filePaths = Directory.GetFiles(@"D:\ForplanVisualization\Archive\LGO" + i.ToString());
                        foreach (var filename in filePaths)
                        {
                            if (filename.Contains(".VWH"))
                            {
                                string str = filename.Replace(@"D:\ForplanVisualization\Archive\LGO" + i.ToString(), PathFolder + FolderName + @"\Archive\LGO" + i.ToString());
                                File.Copy(filename.ToString(), str, true);
                            }
                        }
                    }
                    //DB
                    filePaths = Directory.GetFiles(@"D:\ForplanVisualization\DB");
                    foreach (var filename in filePaths)
                    {
                        if (filename.Contains("localDB.db"))
                        {
                            string str = filename.Replace(@"D:\ForplanVisualization\DB", PathFolder + FolderName + @"\DB");
                            File.Copy(filename.ToString(), str, true);
                        }
                    }
                    //Logging
                    filePaths = Directory.GetFiles(@"D:\ForplanVisualization\Logging");
                    foreach (var filename in filePaths)
                    {
                        if (filename.Contains(".VWL"))
                        {
                            string str = filename.Replace(@"D:\ForplanVisualization\Logging", PathFolder + FolderName + @"\Logging");
                            File.Copy(filename.ToString(), str, true);
                        }
                    }
                    //Recipes
                    string[] fileDirectories = Directory.GetDirectories(@"D:\ForplanVisualization\Rezepte");
                    foreach (var fileDirectory in fileDirectories)
                    {
                        if (!Directory.Exists(PathFolder + FolderName + @"\Rezepte\" + fileDirectory.Replace(Path.GetDirectoryName(fileDirectory) + "\\", "")))
                        {
                            System.IO.Directory.CreateDirectory(PathFolder + FolderName + @"\Rezepte\" + fileDirectory.Replace(Path.GetDirectoryName(fileDirectory) + "\\", ""));
                        }

                        filePaths = Directory.GetFiles(fileDirectory);
                        foreach (var filename in filePaths)
                        {
                            string str = filename.Replace(fileDirectory, PathFolder + FolderName + @"\Rezepte\" + fileDirectory.Replace(Path.GetDirectoryName(fileDirectory) + "\\", ""));// PathFolder + FolderName + @"\Alarms\" + filename.ToString();
                            File.Copy(filename.ToString(), str, true);
                        }
                    }

                    if (File.Exists(PathFolder + FolderName + ".zip"))
                    {
                        File.Delete(PathFolder + FolderName + ".zip");
                    }

                    ZipFile.CreateFromDirectory(PathFolder + FolderName, PathFolder + FolderName + ".zip", CompressionLevel.Fastest, false);
                    System.IO.Directory.Delete(PathFolder + FolderName, true);
                }
               catch (Exception ex)
                {
                    new MessageBoxTask(ex, "* * * *", MessageBoxIcon.Exclamation);
                }

                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    ApplicationService.SetView("TouchpadRegion", "EmptyView");
                });
            });
        }

        private void ClearData()
        {
            //Arhive ALO
            string[] filePaths;

            for (int i = 1; i <= 7; i++)
            {
                filePaths = Directory.GetFiles(@"D:\ForplanVisualization\Archive\ALO" + i.ToString());
                foreach (var filename in filePaths)
                {
                    bool isInRange = false;
                    foreach (var day in activeDays)
                    {
                        if (filename.Contains(day))
                        {
                            isInRange = true;
                            break;
                        }
                    }
                    if (!isInRange)
                        File.Delete(filename);
                }
            }
            //Arhive LGO
            for (int i = 1; i <= 3; i++) 
            {
                filePaths = Directory.GetFiles(@"D:\ForplanVisualization\Archive\LGO" + i.ToString());
                foreach (var filename in filePaths)
                {
                    bool isInRange = false;
                    foreach (var day in activeDays)
                    {
                        if (filename.Contains(day))
                        {
                            isInRange = true;
                            break;
                        }
                    }
                    if (!isInRange)
                        File.Delete(filename);
                }
            }
              

            //Alarms
            filePaths = Directory.GetFiles(@"D:\ForplanVisualization\Alarms");
            foreach (var filename in filePaths)
            {
                bool isInRange = false;
                foreach (var day in activeDays)
                {
                    if (filename.Contains(day))
                    {
                        isInRange = true;
                        break;
                    }
                }
                if (!isInRange)
                    File.Delete(filename);
            }

            //Logging
            filePaths = Directory.GetFiles(@"D:\ForplanVisualization\Logging");
            foreach (var filename in filePaths)
            {
                bool isInRange = false;
                foreach (var day in activeDays)
                {
                    if (filename.Contains(day))
                    {
                        isInRange = true;
                        break;
                    }
                }
                if (!isInRange)
                    File.Delete(filename);
            }
            //Backups
            filePaths = Directory.GetFiles(PathFolder);
            foreach (var filename in filePaths)
            {
                bool isInRange = false;
                foreach (var day in activeDays)
                {
                    if (filename.Contains(day))
                    {
                        isInRange = true;
                        break;
                    }
                }
                if (!isInRange)
                    File.Delete(filename);
            }
        }
    }
}
