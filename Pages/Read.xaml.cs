using Microsoft.Win32;
using ModbusExaminer.helpers;
using ModbusExaminer.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
    /// Interaction logic for Read.xaml
    /// </summary>
    public partial class Read : UserControl
    {
        ObservableCollection<ConnectionHelper> observableConns = new ObservableCollection<ConnectionHelper>();    
        ConcurrentDictionary<string, Window> _registersWindows = new ConcurrentDictionary<string, Window>(StringComparer.OrdinalIgnoreCase);
        private ModbusExaminerLogger logger = ModbusExaminerLogger.Instance;

        public int Count { get; set; } = 1;
        public string IPAddress { get; set; } = "localhost";
        public int Port { get; set; } = 502;
        public short DeviceId { get; set; } = 1;
        public int Register { get; set; }
        public bool? OneBased { get; set; } = false;

        private int currentDeviceComboxSelection = 0;

        public Read()
        {
            InitializeComponent();
            ConnectionsGrid.ItemsSource = observableConns;
            DataContext = this;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            logger.AddLogLine($"Adding new connection: {IPAddress}:{port} with device id {DeviceId}.");
            this.IPAddress = this.IPAddress?.ToLower();
            var key = (this.IPAddress + ":" + this.Port + ":" + this.DeviceId).ToLower();
            var devicetype = devicetypeCmbox.Text;
            var cHelper = observableConns.Where(ch => ch.FullAddress == key).FirstOrDefault();
            if (cHelper==null)
            {
                cHelper = new ConnectionHelper() {
                    IPAddress = this.IPAddress,
                    FullAddress = key,
                    Port = this.Port,
                    Id = this.DeviceId,
                    DeviceType = devicetype,
                    OneBased = this.OneBased};
                observableConns.Add(cHelper);
                cHelper.EstablishConnection(3000);
            }

            cHelper.UpdateRegisters(this.Register, this.Count,devicetype);
        }

        private void RegisterNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {          
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var item = (ConnectionHelper)ConnectionsGrid.SelectedItem;
            item.Close();
            observableConns.Remove(item);
            logger.AddLogLine($"Removing connection: {item.IPAddress}:{item.Port} with device id {item.Id}.");
        }

        private void ConnectionGrid_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as DataGrid)?.SelectedItem;
            if (item != null)
            {
                var resultitem = ((ConnectionHelper)item);
               
                var RWindow = _registersWindows.GetOrAdd(resultitem.FullAddress + ":" + resultitem.Id, new ResultsWindow(resultitem));
                RWindow?.Show();
                RWindow?.Focus();
                RWindow.Closing += (object s, System.ComponentModel.CancelEventArgs ev) =>
                {
                    Window w;
                    _registersWindows.TryRemove(resultitem.FullAddress + ":" + resultitem.Id, out w);
                };
            }
        }     

        private void View_Click(object sender, RoutedEventArgs e)
        {
            var item = (ConnectionHelper)ConnectionsGrid.SelectedItem;
            if (item != null)
            {
                var resultitem = ((ConnectionHelper)item);
                var RWindow = _registersWindows.GetOrAdd(resultitem.FullAddress + ":" + resultitem.Id, new ResultsWindow(resultitem));
                RWindow?.Show();
                RWindow?.Focus();
                RWindow.Closing += (object s, System.ComponentModel.CancelEventArgs ev) =>
                {
                    Window w;
                    _registersWindows.TryRemove(resultitem.FullAddress + ":" + resultitem.Id, out w);
                };
            }
        }

        private void ConnectionsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ConnectionHelper)ConnectionsGrid.SelectedItem;
            IP.Text = (item!=null)?item.IPAddress:"localhost";
            port.Text = (item!=null)?item.Port.ToString():"502";
            slave_id.Text = (item!=null)?item.Id.ToString():"1";
            devicetypeCmbox.SelectedIndex = 0;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (devicetypeCmbox.Text == "")
            {

                devicetypeCmbox.SelectedIndex = currentDeviceComboxSelection;
            }
            else
            {
                currentDeviceComboxSelection = devicetypeCmbox.SelectedIndex;
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                var path = openFileDialog.FileName;
                var connList = JsonConvert.DeserializeObject<ObservableCollection<ConnectionHelper>>(File.ReadAllText(path));
                //  var fileContent = File.ReadAllText(path);
                var diff = connList.Where(ci => !observableConns.Any(oc => oc.FullAddress.Equals(ci.FullAddress, StringComparison.OrdinalIgnoreCase))).ToList();
                foreach(var item in diff)
                {
                    item.EstablishConnection(3000);
                    foreach(var r in item.Registers)
                    {
                        r.connectionHelper = item;
                        r.InitializePeriodicReads();
                    }
                    observableConns.Add(item);
                }                
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON files|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                var path = saveFileDialog.FileName;
                File.WriteAllText(path, JsonConvert.SerializeObject(ConnectionsGrid.ItemsSource));
            }

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void onebased_click(object sender, RoutedEventArgs e)
        {
            OneBased = oneBasedCheckBox.IsChecked;
            if (OneBased == true)
            {
                if(int.TryParse(start_address.Text, out int v) && v <= 0)
                {
                    start_address.Text = "1";
                    Register = 1;

                }
            }
        }

        private void start_address_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OneBased ==true && start_address.Text == "0")
            {
                start_address.Text = "1";
                Register = 1;
            }

        }

        private void end_address_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (count_reg.Text == "0")
            {
                count_reg.Text = "1";
                Count = 1;
            }
        }
    }
}
