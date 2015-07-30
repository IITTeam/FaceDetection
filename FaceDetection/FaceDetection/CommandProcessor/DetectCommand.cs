using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FaceDetection.Core;

namespace FaceDetection.CommandProcessor
{
    public class DetectCommand : ICommand
    {
        public string Name => "detect";

        public string Description => "- распознавание людей";

        public void Execute()
        {
            try
            {
                var t = new Thread(ServicesWorker.GetInstance<FaceRecognizerService>().DetectHuman);
                t.Start();

                if (Console.ReadKey() != null)
                {
                    t.Abort();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
