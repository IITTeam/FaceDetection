using System;
using System.IO;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using FaceDetection.Core;

namespace FaceDetection.CommandProcessor
{
    internal class GenderByPhotoCommand : ICommand
    {
        public string Name => "genderphoto";

        public string Description => "- распознавание пола человека на фотографии";

        public void Execute()
        {
            try
            {
                var fileNames = Directory.GetFiles("Images\\TestMale\\", "*.jpg");
                var maleImg = fileNames.Select(fileName => new Image<Gray, byte>(fileName)).ToList();
                var persentMale = ServicesWorker.GetInstance<FaceRecognizerService>().DetectGenderByPhoto(maleImg, true);
                Console.WriteLine(@"% правильно распознанных мужчин " + persentMale);

                fileNames = Directory.GetFiles("Images\\TestFemale\\", "*.jpg");
                var femaleImg = fileNames.Select(fileName => new Image<Gray, byte>(fileName)).ToList();
                var persentFemale = ServicesWorker.GetInstance<FaceRecognizerService>()
                    .DetectGenderByPhoto(femaleImg, false);
                Console.WriteLine(@"% правильно распознанных женщин " + persentFemale);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}