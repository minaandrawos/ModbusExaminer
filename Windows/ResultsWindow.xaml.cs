using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
using ModbusExaminer.Pages;
using ModbusExaminer.helpers;
using Modbus.Device;
using System.Net.Sockets;

namespace ModbusExaminer.Windows
{
    /// <summary>
    /// Interaction logic for ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : ModernWindow
    {
        ConnectionHelper connHelper;

        public ResultsWindow()
        {
            InitializeComponent();
        }

        public ResultsWindow(ConnectionHelper ch) : this()
        {
            ResultsText.Text = "Connection: " + ch.IPAddress + ":" + ch.Port + " Slave ID: " + ch.Id;
            if (ch.OneBased==true)
            {
                ResultsText.Text += " One based addressing..";
            }
            ResultsDataGrid.ItemsSource = ch.Registers;
            connHelper = ch;
        }

   

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rh = ResultsDataGrid.SelectedItem as RegistersHelper;
            if (rh == null)
                return;
            connHelper.removeRegister(rh);
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
