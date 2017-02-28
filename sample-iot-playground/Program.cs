using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Threading;

namespace sample_iot_playground
{
    class Program
    {
        static DeviceClient deviceClient;
        static string iotHubUri = Properties.Settings.Default.IOTHubUri;
        static string deviceKey = Properties.Settings.Default.DeviceKey;
        static string deviceName = Properties.Settings.Default.DeviceName;

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(
                iotHubUri,
                new DeviceAuthenticationWithRegistrySymmetricKey(deviceName, deviceKey), TransportType.Mqtt);

            SendDeviceToCloudMessagesAsync();

            Console.ReadLine();
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            Random rand = new Random();

            while (true)
            {
                var t = Temperature.Temperatures;
                Console.WriteLine(Temperature.ToString(t));
                double temp = t[0].CurrentValue;
                foreach(var obj in t)
                {
                    if(obj.InstanceName == "CPU Package")
                    {
                        temp = obj.CurrentValue;
                    }
                }

                var telemetryDataPoint = new
                {
                    deviceId = deviceName,
                    cpuTemperature = temp
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                Task.Delay(1000).Wait();
            }
        }
    }
}
