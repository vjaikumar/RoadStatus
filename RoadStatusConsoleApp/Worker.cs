using Microsoft.Extensions.Configuration;
using RoadStatusService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadStatusConsoleApp
{
   
    internal class Worker
    {
        private readonly IConfiguration configuration;

        public Worker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void DoWork()
        {

            var appId = configuration["AppId"];
            var appKey = configuration["AppKey"];

            Console.Write("Enter the Road Name.");

            var roadId = Console.ReadLine();

            var tflClient = new TflClient();
            var roadStatusService = new RoadStatusService.RoadStatusService(tflClient, appId, appKey);
            var result = roadStatusService.GetRoadStatusAsync(roadId).GetAwaiter().GetResult();
            if (result.ErrorCode == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("==============================================");
                Console.WriteLine($"The status of the road '{roadId}' is as follows");
                Console.WriteLine($"Road status is '{result.RoadStatus.StatusSeverity}'");
                Console.WriteLine($"Road status description is '{result.RoadStatus.StatusSeverityDescription}'");
                Console.WriteLine("==============================================");
                Console.ResetColor();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine($"Road '{roadId}' is not a valid road");
                Environment.Exit(result.ErrorCode);
            }
        }
    }
}
