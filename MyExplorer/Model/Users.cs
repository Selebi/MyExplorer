using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.InteropServices;

namespace MyExplorer.Model
{
    public static class Users
    {
        static Services.FileLogger fl = Services.FileLogger.GetInstance(ViewModel.Settings.GetInstance().LogFile);
        static ViewModel.Settings Settings = ViewModel.Settings.GetInstance();
        public static List<Data.User> UsersMembersDomainGroup { get; private set; } = new List<Data.User>();
        public static List<Data.User> UsersLocalGroup { get; private set; } = new List<Data.User>();
        public static Data.User SessionUser { get; private set; }
        public static Data.User CurrentUser { get; private set; }

        public static void LoadAll()
        {
            GetSessionUserName();
            GetCurrentUser();
            GetUsersLocalGroup();
            GetUsersMembersDomainGroup();
        }

        public static bool IsDispatcherUser() => $"{CurrentUser.Domain.ToLower()}/{CurrentUser.Name.ToLower()}" == Settings.DispatcherUser.ToLower();

        public static bool IsAdmin()
        {
            foreach(var u in UsersLocalGroup)
            {
                if (u.Domain.ToLower().Contains(CurrentUser.Domain.ToLower()) && u.Name.ToLower() == CurrentUser.Name.ToLower())
                    return true;
            }
            foreach(var u in UsersMembersDomainGroup)
            {
                if (u.Domain.ToLower().Contains(CurrentUser.Domain.ToLower()) && u.Name.ToLower() == CurrentUser.Name.ToLower())
                    return true;
            }
            return false;
        }

        public static List<Data.User> GetUsersLocalGroup()
        {
            UsersLocalGroup.Clear();
            List<Data.User> result = new List<Data.User>();
            try
            {
                DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName);
                DirectoryEntry admGroup = localMachine.Children.Find(Settings.LocalAdminGroup, "group");
                object members = admGroup.Invoke("members", null);
                foreach (object groupMember in (IEnumerable)members)
                {
                    DirectoryEntry member = new DirectoryEntry(groupMember);
                    result.Add(new Data.User()
                    {
                        Name = member.Name,
                        Domain = member.Path.Split(new string[] { @"WinNT://" }, StringSplitOptions.None)[1].
                         Split(new string[] { $"/{member.Name}" }, StringSplitOptions.None)[0],
                        Type = member.SchemaClassName
                    });
                }
            }
            catch (Exception ex) 
            { fl.Write($"Ошибка при поиске пользователей в группе {Settings.LocalAdminGroup} - {ex.Message}", Enums.LogType.Fatal); }
            UsersLocalGroup = result;
            return result;
        }

        public static List<Data.User> GetUsersMembersDomainGroup()
        {
            UsersMembersDomainGroup.Clear();
            List<Data.User> result = new List<Data.User>();
            SearchResult myADSearchResults = null;
            Domain domain = null;
            try
            {
                DirectoryContext myDomainContext = new DirectoryContext(DirectoryContextType.Domain, Settings.DomainName, DesSecurity.Decrypt(Settings.DomainLogin),
                    DesSecurity.Decrypt(Settings.DomainPassword));
                domain = Domain.GetDomain(myDomainContext);
                DirectoryEntry myADEntry = new DirectoryEntry("LDAP://" + domain, DesSecurity.Decrypt(Settings.DomainLogin), 
                    DesSecurity.Decrypt(Settings.DomainPassword), AuthenticationTypes.Secure);       
                DirectorySearcher myADSearcher = new DirectorySearcher(myADEntry);
                myADSearcher.Filter = $"(&(objectClass=group)(name={Settings.DoaminAdminGroup}))";
                myADSearchResults = myADSearcher.FindOne();
            }
            catch(Exception ex) { fl.Write($"Ошибка при поиске пользователей в группе {Settings.DoaminAdminGroup} - {ex.Message}", Enums.LogType.Error); return null; }
            foreach(string userstr in myADSearchResults.Properties["member"])
            {
                try
                {
                    var user = new Data.User();
                    try { user.Type = new DirectoryEntry($@"LDAP://{domain}/{userstr}", DesSecurity.Decrypt(Settings.DomainLogin), 
                        DesSecurity.Decrypt(Settings.DomainPassword), AuthenticationTypes.Secure).SchemaClassName; } 
                    catch { user.Type = "undefined"; }
                    try { user.Name = userstr.Split(new string[] { "CN=" }, StringSplitOptions.None)[1].Split(',')[0]; }
                    catch { user.Name = userstr; result.Add(user); continue; }
                    try { user.Domain = userstr.Split(new string[] { "DC=" }, StringSplitOptions.None)[1].Split(',')[0]; }
                    catch { user.Domain = Settings.DomainName; }
                    result.Add(user);
                }
                catch(Exception ex) { fl.Write($"Ошибка при добавления пользователя {userstr} - {ex.Message}", Enums.LogType.Error); }
            }
            UsersMembersDomainGroup = result;
            return result;
        }

        public static Data.User GetSessionUserName()
        {
            int sessionId = Process.GetCurrentProcess().SessionId;
            IntPtr ptrBuffer;
            uint bytesReturned;
            Data.User result = new Data.User();
            if (WTSQuerySessionInformation(IntPtr.Zero, sessionId, WTSInfoClass.WTSUserName, out ptrBuffer, out bytesReturned) && ptrBuffer != IntPtr.Zero)
            {
                result.Name = Marshal.PtrToStringAnsi(ptrBuffer);
                WTSFreeMemory(ptrBuffer);
                if (WTSQuerySessionInformation(IntPtr.Zero, sessionId, WTSInfoClass.WTSDomainName, out ptrBuffer, out bytesReturned) && ptrBuffer != IntPtr.Zero)
                {
                    result.Domain = Marshal.PtrToStringAnsi(ptrBuffer);
                    WTSFreeMemory(ptrBuffer);
                }
                SessionUser = result;
                return result;
            }
            return null;
        }

        public static Data.User GetCurrentUser()
        {
            Data.User result = new Data.User();
            var usr = System.Security.Principal.WindowsIdentity.GetCurrent();
            result.Domain = usr.Name.Split(new char[] { '\\' })[0];
            result.Name = usr.Name.Split(new char[] { '\\' })[1];
            CurrentUser = result;
            return result;
        }

        [DllImport("Wtsapi32.dll")]
        static extern bool WTSQuerySessionInformation(IntPtr hServer, int sessionId, WTSInfoClass wtsInfoClass, out IntPtr ppBuffer, out uint bytesReturned);

        [DllImport("wtsapi32.dll", ExactSpelling = true, SetLastError = false)]
        static extern void WTSFreeMemory(IntPtr memory);

        enum WTSInfoClass
        {
            // Имя юзера
            WTSUserName = 5,
            // Имя домена
            WTSDomainName = 7
        }
    }
}
