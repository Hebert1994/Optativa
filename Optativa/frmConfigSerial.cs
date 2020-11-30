using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Optativa
{
    public partial class frmConfigSerial : Form
    {
        public frmConfigSerial()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (string porta in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(porta);
            }
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            comboBox1.Text = settings["porta"].Value;
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            settings["porta"].Value = comboBox1.Text;

            configFile.Save();
            ConfigurationManager.RefreshSection("AppSettings");

            this.Close();

        }
    }
}
