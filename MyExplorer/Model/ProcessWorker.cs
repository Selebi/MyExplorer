using MyExplorer.Data;
using MyExplorer.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Windows;

namespace MyExplorer.Model
{
    public static class ProcessWorker
    {
        static FileLogger fl = Services.FileLogger.GetInstance(ViewModel.Settings.GetInstance().LogFile);
        public static bool IsProcessWork(string name)
        {
            bool result = false;
            var l = new List<Process>(Process.GetProcesses());
            foreach(var p in l)
            {
                if(p.ProcessName.ToLower() == name.ToLower())
                {
                    var procOwner = GetProcessUser(p);
                    if (procOwner != null)
                    {
                        if (Users.CurrentUser.Name.ToLower() == procOwner.Split(new char[] { '\\' })[1].ToLower() && Users.CurrentUser.Domain.ToLower() == procOwner.Split(new char[] { '\\' })[0].ToLower())
                        {
                            result = true;
                            break;
                        }
                    }
                }
            };
            return result;
        }

        private static string GetProcessUser(Process process)
        {
            IntPtr processHandle = IntPtr.Zero;
            try
            {
                OpenProcessToken(process.Handle, 8, out processHandle);
                WindowsIdentity wi = new WindowsIdentity(processHandle);
                return wi.Name;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                {
                    CloseHandle(processHandle);
                }
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        public static bool IsStartedProcessWork(string file)
        {
            if (StartedProcesses.TryGetValue(file, out Process process))
            {
                return IsProcessWork(process.ProcessName);
            }
            return false;
        }

        public static void KillProcess(string name)
        {
            new List<Process>(Process.GetProcessesByName(name)).ForEach(p => { p.Kill(); });
        }

        public static string StartProcess(string file, string param = "")
        {
            if (file == "")
                return $"Файл не указан";
            try
            {
                if (StartedProcesses.ContainsKey(file))
                {
                    Process.Start(file, param);
                    //return "Копия";
                }
                else
                {
                    StartedProcesses.Add(file, Process.Start(file, param));
                }
            }
            catch (Exception ex)
            {
                return $"{ex.Message} : {file}";
            }
            return "";
        }

        public static void StartExplorer(bool forse = false)
        {
            try
            {
                if (forse)
                {
                    Process.Start(@"C:\Windows\explorer.exe");
                    return;
                }

                if (RegEditor.IsSintekExplorerRegistred())
                {
                    RegEditor.RegWindowsExplorer();
                    Process.Start(@"C:\Windows\explorer.exe");
                    Thread.Sleep(1000); // Костыли, переписать!
                    RegEditor.RegSintekExplorer();
                }
                else
                    Process.Start(@"C:\Windows\explorer.exe");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка запуска проводника", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static Dictionary<string, Process> StartedProcesses { get; private set; } = new Dictionary<string, Process>();

        public static bool RunSExplorerAsAdmin(string args = "")
        {
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);

            if (hasAdministrativeRight == false)
            {
                ProcessStartInfo processInfo = new ProcessStartInfo(); //создаем новый процесс
                processInfo.Verb = "runas";
                processInfo.FileName = ViewModel.Settings.GetInstance().CurrentFileName;
                processInfo.Arguments = args;
                try
                {
                    Process.Start(processInfo);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"SintekExplorerPath - \"{Directory.GetCurrentDirectory()}\". {ex.Message}", "Ошибка запроса привелегий", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }

        public static void RunProcessAdmin(string workingDirectory, string filename, string args = "")
        {
            ProcessStartInfo processInfo = new ProcessStartInfo(); //создаем новый процесс
            SecureString pass = new SecureString();
            AdminData data = ViewModel.Settings.GetInstance().AdminData;

            var p = Model.DesSecurity.Decrypt(data.Password);
            foreach(var c in p)
            {
                pass.AppendChar(c);
            }
            p = "";

            processInfo.Verb = "runas";
            processInfo.UseShellExecute = false;
            processInfo.WorkingDirectory = workingDirectory;
            processInfo.FileName = filename;
            processInfo.Arguments = args;
            processInfo.UserName = Model.DesSecurity.Decrypt(data.Name);
            processInfo.Domain = Model.DesSecurity.Decrypt(data.Domain);
            processInfo.Password = pass;

            try
            {
                Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SintekExplorerPath - \"{Directory.GetCurrentDirectory()}\". {ex.Message}", $"Ошибка запуска {filename}", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public static bool IsSExplorerAsAdmin()
        {
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return pricipal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
