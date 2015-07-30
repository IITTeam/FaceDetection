using System;
using System.IO;
using System.Collections.Generic;
using FaceDetection.Core;

namespace FaceDetection.CommandProcessor
{
    public class AddFaceCommand : ICommand
    {
        public string Name
        {
            get { return "add"; }
        }

        public string Description
        {
            get { return "- добавляет человека"; }
        }

        public void Execute()
        {
            try
            {
                Console.WriteLine(@"Введите имя человека и встаньте перед камерой.");
                Console.WriteLine(@"Пожалуйста, во время записи постарайтесь" + "\n" +
                                  @"продемонстрировать разные выражения лица");
                var name = Console.ReadLine();
                Directory.CreateDirectory("Images\\" + name);
                var images = ServicesWorker.GetInstance<FaceRecognizerService>().AddImagesToHuman(name);
                foreach (var img in images)
                    img.Save("Images\\" + name + "\\" + images.IndexOf(img) + ".jpg");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}