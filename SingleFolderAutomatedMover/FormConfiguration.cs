using System;
using System.Configuration;
using System.Windows.Forms;
using SingleFolderAutomatedMover.Properties;
using System.Security;

namespace SingleFolderAutomatedMover
{
    public partial class FormConfiguration : Form
    {

        private bool reusePassword = false;
        public FormConfiguration()
        {
            InitializeComponent();
            buttonTo.Click += buttonFrom_Click;
        }

        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Text = Resources.FormConfiguration_FormConfiguration_Load_Set_Configuration;

            labelPasswordChange.Visible = false;
            labelUsername.Visible = false;
            labelPassword.Visible = false;
            textBoxUsername.Visible = false;
            textBoxPassword.Visible = false;

            Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

            if (confCollection.Count != 0)
                FillInForm();
        }

        private void FillInForm()
        {
            textBoxFrom.Text = ConfigurationManager.AppSettings["Path From"];
            textBoxTo.Text = ConfigurationManager.AppSettings["Path To"];
            if (ConfigurationManager.AppSettings["RequiresDifferentCredentials"] == "true")
            {
                labelUsername.Visible = true;
                labelPassword.Visible = true;
                textBoxUsername.Visible = true;
                textBoxPassword.Visible = true;

                checkBoxCred.Checked = true;

                textBoxUsername.Text = ConfigurationManager.AppSettings["Username"];
                textBoxPassword.Text = "******";
                labelPasswordChange.Visible = true;
                reusePassword = true;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxFrom.Text != "" &&
                textBoxTo.Text != "" &&
                textBoxFrom.Text != textBoxTo.Text &&
                !Core.IsSubfolder(textBoxFrom.Text, textBoxTo.Text))
            {
                //Always required fields are good.
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (checkBoxCred.Checked)
                {
                    if (textBoxUsername.Text != "" &&
                    textBoxPassword.Text != "")
                    {
                        //Save username and password if wanted
                        if (reusePassword)
                        {
                            var username = ConfigurationManager.AppSettings["Username"];
                            var password = ConfigurationManager.AppSettings["Password"];
                            confCollection.Clear();
                            confCollection.Add("Username", username);
                            confCollection.Add("Password", password);
                        }
                        else
                        {
                            //Clear all previous configurations
                            confCollection.Clear();
                            //Checkbox is checked, Password and Username have to filled in.
                            confCollection.Add("Username", textBoxUsername.Text);
                            confCollection.Add("Password", Crypto.EncryptStringAES(textBoxPassword.Text, Core.Salt()));
                        }

                        //configManager.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("Password",textBoxPassword.Text));
                        confCollection.Add("RequiresDifferentCredentials", "true");
                    }
                    else
                    {
                        MessageBox.Show("Please fill in the username and password.");
                        return;
                    }

                }
                else
                {
                    //Clear all previous configurations
                    confCollection.Clear();
                    confCollection.Add("RequiresDifferentCredentials", "false");
                }




                confCollection.Add("Path From", textBoxFrom.Text);
                confCollection.Add("Path To", textBoxTo.Text);



                configManager.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(Resources.FormConfiguration_buttonSave_Click_Please_fill_in_all_the_fields);
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void buttonFrom_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (((Button)sender).Name == "buttonFrom")
                {
                    textBoxFrom.Text = folderBrowserDialog1.SelectedPath;
                }
                else
                {
                    textBoxTo.Text = folderBrowserDialog1.SelectedPath;
                }
            }


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBoxCred.Checked)
            {
                labelUsername.Visible = true;
                labelPassword.Visible = true;
                textBoxUsername.Visible = true;
                textBoxPassword.Visible = true;
            }
            else
            {
                labelUsername.Visible = false;
                labelPassword.Visible = false;
                textBoxUsername.Visible = false;
                textBoxPassword.Visible = false;
            }
        }

    }
}
