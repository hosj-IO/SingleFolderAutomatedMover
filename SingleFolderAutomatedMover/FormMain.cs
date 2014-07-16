using System;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;
using System.Configuration;

using SingleFolderAutomatedMover.Properties;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using System.Timers;
using System.Linq;// DllImport
//using System.Security.Permissions; // PermissionSetAttribute


namespace SingleFolderAutomatedMover
{

    public partial class FormMain : Form
    {
        private FileSystemWatcher _fsw;
        private bool isRunning = false;
        private List<string> fileQueue;
        //private const int LOGON_TYPE_NEW_CREDENTIALS = 9;
        //private const int LOGON32_PROVIDER_WINNT50 = 3;

        public MoveRule MoveRule { get; set; }

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Text = Resources.FormMain_FormMain_Load_Automated_Mover;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            buttonStop.Enabled = false;
            try
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection.Count == 0)
                {
                    MessageBox.Show(Resources.FormMain_FormMain_Load_Configuration_not_found__opening_the_settings_windows_);
                    var formConfig = new FormConfiguration();
                    if (formConfig.ShowDialog() != DialogResult.OK)
                    {
                        MessageBox.Show(Resources.FormMain_FormMain_Load_No_settings_saved__stopping_program_);
                        Application.Exit();
                    }
                    else
                    {
                        LoadConfig(confCollection);
                        ReloadConfigInfo(confCollection);
                    }
                }
                else
                {
                    LoadConfig(confCollection);
                    ReloadConfigInfo(confCollection);

                }
            }
            catch (Exception ex)
            {
                Application.Exit();
            }


            notifyIconMain.Icon = Icon;
            notifyIconMain.Text = Resources.FormMain_FormMain_Load_Simple_AutoMover;
        }

        private void ReloadConfigInfo(KeyValueConfigurationCollection confCollection)
        {
            textBoxFromInfo.Text = ConfigurationManager.AppSettings["Path From"];
            textBoxToInfo.Text = ConfigurationManager.AppSettings["Path to"];
            if (ConfigurationManager.AppSettings["RequiresDifferentCredentials"] != "true")
            {
                labelConfUserForm.Visible = false;
                textBoxUserInfo.Visible = false;
            }
            else
            {
                textBoxUserInfo.Text = ConfigurationManager.AppSettings["Username"];
                labelConfUserForm.Visible = true;
                textBoxUserInfo.Visible = true;
            }
        }

        private void LoadConfig(KeyValueConfigurationCollection confCollection)
        {
            //Load configuration
            Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (ConfigurationManager.AppSettings["RequiresDifferentCredentials"] != "true")
            {
                if (ConfigurationManager.AppSettings["Path From"] == ConfigurationManager.AppSettings["Path to"] &&
                    Core.IsSubfolder(ConfigurationManager.AppSettings["Path From"], ConfigurationManager.AppSettings["Path To"]))
                {
                    MessageBox.Show("Path from is the same as path to. Deleting the config, restart the application.");
                    confCollection.Clear();
                    configManager.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
                    Application.Exit();
                }
                MoveRule = new MoveRule(ConfigurationManager.AppSettings["Path From"],
                    ConfigurationManager.AppSettings["Path To"], false);
            }
            else
            {
                MoveRule = new MoveRule(ConfigurationManager.AppSettings["Path From"],
                    ConfigurationManager.AppSettings["Path To"],
                    ConfigurationManager.AppSettings["Username"],
                    Crypto.DecryptStringAES(ConfigurationManager.AppSettings["Password"], Core.Salt()), true);
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {

            _fsw = new FileSystemWatcher(MoveRule.PathFrom)
                {
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    IncludeSubdirectories = true,
                    Filter = "*.*",
                    EnableRaisingEvents = true
                };
            _fsw.Changed += fsw_Changed;
            _fsw.Created += fsw_Changed;
            _fsw.Renamed += fsw_Changed;

            buttonStop.Enabled = true;
            buttonStart.Enabled = false;
            listBoxLogging.Items.Insert(0, Resources.FormMain_button_ServiceHasStarted);
            isRunning = true;
            fileQueue = new List<string>();

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (fileQueue != null && fileQueue.Count != 0)
            {
                var queueClone = fileQueue.Distinct().ToList();
                queueClone = RemoveFolders(queueClone);
                
                foreach (var file in queueClone)
                {
                    if (MoveRule.RequiresDifferentCredentials)
                    {
                        DoWorkUnderImpersonation(file);
                    }
                    else
                    {
                        DoWork(file);
                    }
                }
                fileQueue.Clear();
            }
        }

        private List<string> RemoveFolders(List<string> queueClone)
        {
            List<string> directoryPaths = new List<string>();
            foreach (var item in queueClone)
            {
                if((File.GetAttributes(item) & FileAttributes.Directory)
                 == FileAttributes.Directory)
                {
                    directoryPaths.Add(item);
                }
            }
            foreach (var item in directoryPaths)
            {
                queueClone.Remove(item);
            }

            return queueClone;
        }

        private void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {

                //_fsw.EnableRaisingEvents = false;
                //MoveFile(e.FullPath);
                fileQueue.Add(e.FullPath);



            }
            catch (Exception ex)
            {

            }
            /*finally
            {
                _fsw.EnableRaisingEvents = true;
            }*/
        }

        private void LogtoListBox(string textToLog)
        {
            Invoke((MethodInvoker)(() =>
                                       listBoxLogging.Items.Insert(0, textToLog)));
        }

        private void LogtoBalloon(string title, string text)
        {
            notifyIconMain.ShowBalloonTip(3000, title, text, ToolTipIcon.Info);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                _fsw.Dispose();
            }
            catch (Exception ex)
            {
                //Error is thrown because fsw wasnt running
            }
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            listBoxLogging.Items.Insert(0, Resources.FormMain_buttonStop_Click_);
            isRunning = false;
            fileQueue.Clear();
        }

        public void DoWorkUnderImpersonation(string fullpath)
        {
            //elevate privileges before doing file copy to handle domain security
            WindowsImpersonationContext impersonationContext = null;
            IntPtr userHandle = IntPtr.Zero;
            const int LOGON32_PROVIDER_DEFAULT = 0;
            const int LOGON32_LOGON_INTERACTIVE = 2;
            string domain = "";
            string user = MoveRule.Username;
            string password = MoveRule.Password;//"LBhaUrOFIRm3Wi7";

            try
            {
                var windowsIdentity = WindowsIdentity.GetCurrent();
                if (windowsIdentity != null)
                    LogtoListBox("windows identify before impersonation: " + windowsIdentity.Name);

                // if domain name was blank, assume local machine
                if (domain == "")
                    domain = Environment.MachineName;

                // Call LogonUser to get a token for the user
                bool loggedOn = LogonUser(user,
                                            domain,
                                            password,
                                            LOGON32_LOGON_INTERACTIVE,
                                            LOGON32_PROVIDER_DEFAULT,
                                            ref userHandle);

                if (!loggedOn)
                {
                    LogtoListBox("Exception impersonating user, error code: " + Marshal.GetLastWin32Error());
                    return;
                }

                // Begin impersonating the user
                impersonationContext = WindowsIdentity.Impersonate(userHandle);

                var identity = WindowsIdentity.GetCurrent();
                if (identity != null)
                    LogtoListBox("Main() windows identify after impersonation: " + identity.Name);

                //run the program with elevated privileges (like file copying from a domain server)
                DoWork(fullpath);

            }
            catch (Exception ex)
            {
                LogtoListBox("Exception impersonating user: " + ex.Message);
            }
            finally
            {
                // Clean up
                if (impersonationContext != null)
                {
                    impersonationContext.Undo();
                }

                if (userHandle != IntPtr.Zero)
                {
                    CloseHandle(userHandle);
                }
            }
        }

        private void DoWork(string fullPath)
        {
            var toPath = MoveRule.PathTo + "\\" + Path.GetFileName(fullPath);
            //everything in here has elevated privileges
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(fullPath, toPath, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs);
                LogtoListBox(fullPath + " has been moved to remote location.");
                LogtoBalloon("File has been moved.", fullPath + " has been moved.");
            }
            catch (Exception ex)
            {
                LogtoListBox("Failed to move file: " + fullPath);
            }
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                notifyIconMain.Visible = true;
                Hide();
            }

            else if (FormWindowState.Normal == WindowState)
            {
                notifyIconMain.Visible = false;
            }
        }

        private void notifyIconMain_DoubleClick(object sender, EventArgs e)
        {
            Show();
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            notifyIconMain.Visible = false;
        }

        private void buttonConfiguration_Click(object sender, EventArgs e)
        {
            //Not so cool
            LogtoListBox("Shutting down service, opening configuration.");
            if (isRunning)
                buttonStop_Click(null, null);
            Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

            var formConfig = new FormConfiguration();
            if (formConfig.ShowDialog() == DialogResult.OK)
            {
                LoadConfig(confCollection);
                ReloadConfigInfo(confCollection);
            }
        }

    }
}

