using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using FaceDetection.Core;

namespace FaceDetection.CommandProcessor
{
    internal class TrainGenderCommand : ICommand
    {
        public string Name => "trainGender";
        public string Description => "- обучение для распознавания пола";

        public void Execute()
        {
            try
            {
                ServicesWorker.GetInstance<FaceRecognizerService>()
                    .TrainGender(GetSamples("DetMale"), GetSamples("DetFemale"));
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine(@"Нет изображений для обучения распознаванию пола!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static List<Image<Gray, byte>> GetSamples(string pathName)
        {
            var fileNames = Directory.GetFiles("Images\\" + pathName + "\\", "*.jpg");
            return fileNames.Select(fileName => new Image<Gray, byte>(fileName)).ToList();
        }
    }
}