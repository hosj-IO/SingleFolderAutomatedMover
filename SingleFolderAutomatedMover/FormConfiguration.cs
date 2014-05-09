using System;
using System.Configuration;
using System.Windows.Forms;
using SingleFolderAutomatedMover.Properties;

namespace SingleFolderAutomatedMover
{
    public partial class FormConfiguration : Form
    {
        public FormConfiguration()
        {
            InitializeComponent();
            buttonTo.Click += buttonFrom_Click;
        }

        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Text = Resources.FormConfiguration_FormConfiguration_Load_Set_Configuration;

            labelUsername.Visible = false;
            labelPassword.Visible = false;
            textBoxUsername.Visible = false;
            textBoxPassword.Visible = false;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxFrom.Text != "" &&
                textBoxTo.Text != "" &&
                textBoxFrom.Text != textBoxTo.Text)
            {
                //Always required fields are good.
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;
                if (checkBoxCred.Checked)
                {
                    if (textBoxUsername.Text != "" &&
                    textBoxPassword.Text != "")
                    {
                        //Checkbox is checked, Password and Username have to filled in.
                        confCollection.Add("Username", textBoxUsername.Text);
                        confCollection.Add("Password", Crypto.EncryptStringAES(textBoxPassword.Text, "s"));
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
