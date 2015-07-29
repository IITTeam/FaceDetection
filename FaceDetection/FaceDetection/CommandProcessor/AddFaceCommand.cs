using System;
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

        public void Execute(List<string> parameters)
        {
            try
            {
                Console.WriteLine(@"Введите имя человека и встаньте перед камерой.");
                Console.WriteLine(@"Пожалуйста, во время записи постарайтесь" + @"" +
                                  @"продемонстрировать разные выражения лица");
                var name = Console.ReadLine();
                ServicesWorker.GetInstance<FaceRecognizerService>().AddFaces(name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}