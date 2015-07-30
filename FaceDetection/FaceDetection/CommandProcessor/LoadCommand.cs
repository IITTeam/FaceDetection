using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FaceDetection.Core;

namespace FaceDetection.CommandProcessor
{
    public class LoadCommand : ICommand
    {
        public string Name
        {
            get { return "load"; }
        }

        public string Description
        {
            get { return "- загружает обученный сервис"; }
        }

        public void Execute()
        {
            try
            {
                ServicesWorker.GetInstance<FaceRecognizerService>().Load();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
