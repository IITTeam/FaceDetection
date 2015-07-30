using System;
using FaceDetection.Core;

namespace FaceDetection.CommandProcessor
{
    public class LoadCommand : ICommand
    {
        public string Name => "load";
        public string Description => "- загружает обученный сервис";

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