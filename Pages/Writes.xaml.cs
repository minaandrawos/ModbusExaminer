using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModbusExaminer.Pages
{
    /// <summary>
    /// Interaction logic for Writes.xaml
    /// </summary>
    public partial class Writes : UserControl
    {
        public bool? OneBased { get; private set; }

        public Writes()
        {
            InitializeComponent();
        }


        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async void button_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var tcpClient = new TcpClient(IPAddress.Text, int.Parse(Port.Text)))
                using (var modbusMaster = ModbusIpMaster.CreateIp(tcpClient))
                {
                    switch (devicetypeCmbox.Text)
                    {
                        case "Output Coils":
                            await modbusMaster.WriteSingleCoilAsync(byte.Parse(SlaveID.Text), getRegisterAddress(ushort.Parse(RegisterAddress.Text)),bool.Parse(NewValue.Text));
                            break;
                        case "Holding Registers":
                            await modbusMaster.WriteSingleRegisterAsync(byte.Parse(SlaveID.Text), getRegisterAddress(ushort.Parse(RegisterAddress.Text)), ushort.Parse(NewValue.Text));                         
                            break;
                    }
                   WriteResultLabel.Content = "Write sent... ";
                }
            }
            catch (Exception ex)
            {
                WriteResultLabel.Content = "Error occured: " + ex.Message;
            }
        }

        private ushort getRegisterAddress(ushort r)
        {
            if (OneBased==true)
            {
                return (ushort)(r - 1);
            }
            return r;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void onebased_click(object sender, RoutedEventArgs e)
        {
            OneBased = oneBasedCheckBox.IsChecked;
            if (OneBased == true)
            {
                RegisterAddress.Text = "1";
            }
        }

        private void RegisterAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OneBased == true && RegisterAddress.Text=="0")
            {
                RegisterAddress.Text = "1";
            }
        }
    }
}
