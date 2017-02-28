using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using OpenHardwareMonitor.Hardware;

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
                Computer computerHardware = new Computer();
                computerHardware.CPUEnabled = true;
                computerHardware.Open();

                List<Temperature> result = new List<Temperature>();

                foreach (var hardware in computerHardware.Hardware)
                {
                    if(hardware.HardwareType == HardwareType.CPU)
                    {
                        hardware.Update();
                        foreach (IHardware subHardware in hardware.SubHardware)
                            subHardware.Update();
                        foreach(var sensor in hardware.Sensors)
                        {
                            if(sensor.SensorType == SensorType.Temperature)
                            {
                                if (sensor.Value != null)
                                {
                                    result.Add(new Temperature { CurrentValue = sensor.Value.Value, InstanceName = sensor.Name });
                                }
                            }
                        }
                    }
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
