using ModbusExaminer.helpers;
using System;
using System.Collections.Generic;
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

namespace ModbusExaminer.Pages
{
    /// <summary>
    /// Interaction logic for Logs.xaml
    /// </summary>
    public partial class Logs : UserControl
    {
        private ModbusExaminerLogger logger = ModbusExaminerLogger.Instance;
        public Logs()
        {
            InitializeComponent();
            var logList = logger.GetLogList();
            BindingOperations.EnableCollectionSynchronization(logList, logger.GetLogListSyncObject());
            logsListBox.ItemsSource = logList;
        }
    }
}
