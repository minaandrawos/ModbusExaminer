using Modbus.Device;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ModbusExaminer.helpers
{
    public class ConnectionHelper : INotifyPropertyChanged
    {
        [JsonIgnore]
        public ModbusIpMaster modbusMaster;
        private TcpClient tcpClient;

        public string FullAddress { get; set; }
        public string IPAddress { get; set; }
        public string AddressPort {
            get => IPAddress + ":" + Port;
        }
        public int Id { get; set; }
        public int Port { get; set; }
        public string DeviceType { get; set; }
        public bool? OneBased { get; set; }
        public string Status {
            get => statusString ;
            set
            {
                if (value != statusString)
                {
                    this.statusString = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }
        private string statusString;
        public ObservableCollection<RegistersHelper> Registers { get; set; } = new ObservableCollection<RegistersHelper>();

        private ModbusExaminerLogger logger = ModbusExaminerLogger.Instance;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void UpdateRegisters(int startAddress, int count, string rt)
        {
            for(int i = startAddress; i < startAddress + count; i++)
            {
                if (!Registers.Any(reg => reg.Register == i && string.Equals(rt,reg.RegisterType,StringComparison.CurrentCultureIgnoreCase)))
                {
                    var r = new RegistersHelper()
                    {
                        Register = i,
                        RegisterType = rt,
                        connectionHelper = this
                    };
                    Registers.Add(r);
                   // logger.AddLogLine($"Adding register {r.Register}");
                    Task.Run(() =>
                    {                   
                        r.InitializePeriodicReads();
                    });
                    
                }
            }
        }

        public void removeRegister(RegistersHelper rh)
        {
            logger.AddLogLine($"Removing register {rh.Register}");
            rh.IsActive = false;
            Registers.Remove(rh);
        }

        public void EstablishConnection(int i)
        {
            ConnectToModbus(i);
            ConnectionChecker(i);
        }

        public void Close()
        {           
            tcpClient?.Close();
            modbusMaster?.Dispose();
        }

        private void ConnectToModbus(int i)
        {
            try
            {
                logger.AddLogLine("Establishing connection to IP {0} , and port {1}",IPAddress,Port);                   
                tcpClient = new TcpClient(IPAddress, Port);
                modbusMaster = ModbusIpMaster.CreateIp(tcpClient);
                this.Status = "Connected";
            }catch(Exception ex)
            {
                this.Status = "Not Connected";
                //log exception
                logger.AddLogLine($"Error establishing connection to address {IPAddress}:{Port}, with error {ex.Message}");
            } 
        }

        private void ConnectionChecker(int i)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(i).ConfigureAwait(false);
                    if (tcpClient == null || !tcpClient.Connected)
                    {
                        logger.AddLogLine($"Connection to address {IPAddress}:{Port} seems to have failed, retrying to connect...");
                        this.Status = "Not Connected";
                        ConnectToModbus(i);
                    }                  
                }
            });
        }
    }
}
