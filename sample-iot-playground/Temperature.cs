using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace sample_iot_playground
{
    class Temperature
    {
        public double CurrentValue { get; set; }
        public string InstanceName { get; set; }
        public static List<Temperature> Temperatures
        {
            get
            {
                List<Temperature> result = new List<Temperature>();
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        Double temp = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                        temp = (temp - 2732) / 10.0;
                        result.Add(new Temperature { CurrentValue = temp, InstanceName = obj["InstanceName"].ToString() });
                    }
                }
                catch (Exception)
                {
                    result.Add(new Temperature { CurrentValue = -999, InstanceName = "Error" });
                }
                return result;
            }
        }

        public static string ToString(List<Temperature> temp)
        {
            var sb = new StringBuilder();
            foreach(var x in temp)
            {
                sb.AppendFormat("{0}: {1}", x.InstanceName, x.CurrentValue);
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
