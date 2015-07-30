using System;
using FaceDetection.Core;

namespace FaceDetection.CommandProcessor
{
    public class TrainCommand : ICommand
    {
        public string Name => "train";
        public string Description => "- обучение программы";

        public void Execute()
        {
            try
            {
                ServicesWorker.GetInstance<FaceRecognizerService>().TrainFaceRecognizer();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}