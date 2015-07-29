using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using FaceDetection.Core;

namespace FaceDetection.CommandProcessor
{
    class TrainGenderCommand : ICommand
    {
        public string Name
        {
            get { return "trainGender"; }
        }

        public string Description
        {
            get { return "- обучение для распознавания пола"; }
        }

        public void Execute(List<string> parameters)
        {
            try
            {
                ServicesWorker.GetInstance<FaceRecognizerService>()
                        .TrainGender(GetSamples("DetMale"), GetSamples("DetFemale"));
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
