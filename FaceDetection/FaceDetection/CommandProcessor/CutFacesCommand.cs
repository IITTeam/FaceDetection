﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using FaceDetection.Core;

namespace FaceDetection.CommandProcessor
{
    public class CutFacesCommand : ICommand
    {
        public string Name
        {
            get { return "cutFaces"; }
        }

        public string Description
        {
            get { return "- вырезает лица из фотографий"; }
        }

        public void Execute(List<string> parameters)
        {
            ServicesWorker.GetInstance<FaceRecognizerService>().DetectGender(GetSamples("Male"), GetSamples("Female"));
        }

        private static List<Image<Gray, byte>> GetSamples(string pathName)
        {
            var fileNames = Directory.GetFiles("Images\\" + pathName + "\\", "*.jpg");
            return fileNames.Select(fileName => new Image<Gray, byte>(fileName)).ToList();
        }
    }
}