using System;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;
using System.Configuration;

using SingleFolderAutomatedMover.Properties;
using System.Runtime.InteropServices; // DllImport
//using System.Security.Permissions; // PermissionSetAttribute


namespace SingleFolderAutomatedMover
{

    public partial class FormMain : Form
    {
        private FileSystemWatcher _fsw;
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
                }
            }
            else
            {
                LoadConfig(confCollection);

            }

            notifyIconMain.Icon = Icon;
            notifyIconMain.Text = Resources.FormMain_FormMain_Load_Simple_AutoMover;
        }

        private void LoadConfig(KeyValueConfigurationCollection confCollection)
        {
            //Load configuration
            if (ConfigurationManager.AppSettings["RequiresDifferentCredentials"] != "true")
            {
                if (ConfigurationManager.AppSettings["Path From"] == ConfigurationManager.AppSettings["Path to"] && 
                    Core.IsSubfolder(ConfigurationManager.AppSettings["Path From"],ConfigurationManager.AppSettings["Path To"]  ))
                {
                    MessageBox.Show("Path from is the same as path to. Deleting the config, restart the application.");
                    confCollection.Clear();
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
                    Crypto.DecryptStringAES(ConfigurationManager.AppSettings["Password"],"s"), true);
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

        }

        private void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {

                _fsw.EnableRaisingEvents = false;
                //MoveFile(e.FullPath);
                if (MoveRule.RequiresDifferentCredentials)
                {
                    DoWorkUnderImpersonation(e.FullPath);
                }
                else
                {
                    DoWork(e.FullPath);
                }

                

            }
            finally
            {
                _fsw.EnableRaisingEvents = true;
            }
        }

        private void LogtoListBox(string p)
        {
            Invoke((MethodInvoker)(() =>
                                       listBoxLogging.Items.Insert(0, p)));
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            _fsw.Dispose();
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            listBoxLogging.Items.Insert(0, Resources.FormMain_buttonStop_Click_);
        }

        public void DoWorkUnderImpersonation(string fullpath)
        {
            //elevate privileges before doing file copy to handle domain security
            WindowsImpersonationContext impersonationContext = null;
            IntPtr userHandle = IntPtr.Zero;
            // ReSharper disable InconsistentNaming
            const int LOGON32_PROVIDER_DEFAULT = 0;
            const int LOGON32_LOGON_INTERACTIVE = 2;
            // ReSharper restore InconsistentNaming
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

            Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(fullPath, toPath, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs);
            LogtoListBox(fullPath + " has been moved to remote location."); 

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

    }
}

/*
 * TODO
 * Disable using from and to as the same.
 * Show balloon when something is moved.
 * Jump open on error.
*/