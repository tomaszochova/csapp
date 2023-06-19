using System;
using System.Net;
using System.Threading;

namespace MyApp
{
    internal class Program
    {
        [Obsolete]
        static void GetIp() {
            try
            {
                using (var client = new WebClient())
                {
                    string ip = client.DownloadString("https://api.ipify.org");
                    Console.WriteLine("Your public IPv4 address is: " + ip);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

        }

        [Obsolete]
        static void Main(string[] args)
        {
            while (true) {
                
                GetIp();

                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }
    }
}