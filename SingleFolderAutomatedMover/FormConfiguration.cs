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
                textBoxUsername.Text != "" &&
                textBoxPassword.Text != "")
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;


                confCollection.Add("Path From", textBoxFrom.Text);
                confCollection.Add("Path To", textBoxTo.Text);
                if (checkBoxCred.Checked)
                {
                    confCollection.Add("Username", textBoxUsername.Text);
                    confCollection.Add("Password", Crypto.EncryptStringAes(textBoxPassword.Text, "sFo6SSZfUD7IFG2sRvCi3sPaqcANul5GdTlVroa4DgPZSqcqbejKVuenKHat1yr0"));
                    //configManager.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("Password",textBoxPassword.Text));
                    confCollection.Add("RequiresDifferentCredentials", "true");
                }
                else
                {
                    confCollection.Add("RequiresDifferentCredentials", "false");
                }

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
