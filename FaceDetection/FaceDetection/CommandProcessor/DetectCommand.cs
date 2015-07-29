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
        public string Name
        {
            get { return "detect"; }
        }

        public string Description
        {
            get { return "- распознавание людей"; }
        }

        public void Execute(List<string> parameters)
        {
            try
            {
                var t = new Thread(ServicesWorker.GetInstance<FaceRecognizerService>().DetectFace);
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
