using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ModbusExaminer.helpers
{
    public sealed class ModbusExaminerLogger
    {

        private static volatile ModbusExaminerLogger instance;
        private static object syncRoot = new Object();
        private static object listLock = new object();
        private volatile ObservableCollection<string> logList = new ObservableCollection<string>();
        private ModbusExaminerLogger(){}

        public static ModbusExaminerLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ModbusExaminerLogger();
                    }
                }

                return instance;
            }
        }

        public ObservableCollection<string> GetLogList()
        {
            return logList;
        }

        public Object GetLogListSyncObject()
        {
            return listLock;
        }

        public void AddLogLine(string log, params object[] args)
        {
            //Task.Run(() =>
            //{
                lock (listLock)
                    logList.Add(string.Format(log, args));
           // });
        }
    }
}
