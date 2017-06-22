using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace ModbusExaminer.helpers
{
    public class RegistersHelper : INotifyPropertyChanged
    {
        public int Register { get; set; }
        public string Alias { get; set; }
        [JsonIgnore]
        public string Value {
            get
            {
                return this.readValue;
            }
            set {
                if (value != this.readValue)
                {
                    this.readValue = value;
                    NotifyPropertyChanged("Value");
                }
            }
        }
        private string readValue;
        public string RegisterType { get; set; }
        public int SampleRate { get; set; } = 3000;
        // ignored
        [JsonIgnore]
        public ConnectionHelper connectionHelper { get; set; }
        public bool IsActive { get; set; } = true;
        private ModbusExaminerLogger logger = ModbusExaminerLogger.Instance;
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void InitializePeriodicReads()
        {
            logger.AddLogLine($"Starting periodic reads for register {Register}, with type {RegisterType}");
            Task.Run(async() =>
            {
                while (IsActive)
                {                  
                    try
                    {
                        switch (RegisterType)
                        {
                            case "Holding Registers":
                                Value = retrieveValue(connectionHelper.modbusMaster.ReadHoldingRegisters((byte)connectionHelper.Id, getRegisterAddress((ushort)Register), 1));
                                break;
                            case "Input Registers":
                                Value = retrieveValue(connectionHelper.modbusMaster.ReadInputRegisters((byte)connectionHelper.Id, getRegisterAddress((ushort)Register), 1));
                                break;
                            case "Input Coils":
                                Value = retrieveValue(connectionHelper.modbusMaster.ReadInputs((byte)connectionHelper.Id, getRegisterAddress((ushort)Register), 1));
                                break;
                            case "Output Coils":
                                Value = retrieveValue(connectionHelper.modbusMaster.ReadCoils((byte)connectionHelper.Id, getRegisterAddress((ushort)Register), 1));
                                break;
                        }
                    }catch(Exception ex)
                    {
                        logger.AddLogLine($"Error reading value from register address {Register} and register type {RegisterType}");

                    }
                    await Task.Delay(SampleRate).ConfigureAwait(false);
                }
            });
        }

        private string retrieveValue<T>(T[] list)
        {
            if (list.Length > 0)
            {
                return list[0].ToString();
            }
            return "";
        }

        private ushort getRegisterAddress(ushort r)
        {
            if (connectionHelper.OneBased ==true)
            {
                return (ushort)(r - 1);
            }
            return r;
        }
    }
}
