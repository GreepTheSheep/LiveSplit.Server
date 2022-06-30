using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.UI.Components
{
    public partial class Settings : UserControl
    {
        public ushort Port { get; set; }

        public string LocalIP { get; set; }

        public ServerComponent ServerComponent { get; set; }


        public string GetIP()
        {
            IPAddress[] ipv4Addresses = Array.FindAll(
                Dns.GetHostEntry(string.Empty).AddressList,
                a => a.AddressFamily == AddressFamily.InterNetwork);
            
            return String.Join(",", Array.ConvertAll(ipv4Addresses, x => x.ToString()));
        }

        public string PortString
        {
            get { return Port.ToString(); }
            set
            {
                try
                {
                    Port = ushort.Parse(value);
                }
                catch (Exception)
                {
                    Port = 16934;
                }
            }
        }

        public Settings(ServerComponent component)
        {
            InitializeComponent();
            ServerComponent = component;
            Port = 16934;
            LocalIP = GetIP();
            label3.Text = LocalIP;
            label6.Text = ServerComponent.ServerFactory.Version.ToString();

            if (ServerComponent.Server != null) SetServerOn();
            else SetServerOff();

            txtPort.DataBindings.Add("Text", this, "PortString", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void SetServerOn()
        {
            labelStatus.Text = "Server is enabled";
            labelStatus.ForeColor = System.Drawing.Color.Green;
            buttonStatus.Text = "Stop Server";
        }

        public void SetServerOff()
        {
            labelStatus.Text = "Server is disabled";
            labelStatus.ForeColor = System.Drawing.Color.Red;
            buttonStatus.Text = "Start Server";
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            var parent = document.CreateElement("Settings");
            CreateSettingsNode(document, parent);
            return parent;
        }

        public int GetSettingsHashCode()
        {
            return CreateSettingsNode(null, null);
        }

        private int CreateSettingsNode(XmlDocument document, XmlElement parent)
        {
            return SettingsHelper.CreateSetting(document, parent, "Port", PortString);
        }

        public void SetSettings(XmlNode settings)
        {
            PortString = SettingsHelper.ParseString(settings["Port"]);
        }

        private void buttonOpenplanet_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://openplanet.dev/plugin/speedrun");
        }

        private void buttonStatus_Click(object sender, EventArgs e)
        {
            if (ServerComponent.Server != null)
            {
                // if server on
                ServerComponent.Stop();
            }
            else
            {
                ServerComponent.Start();
            }
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
