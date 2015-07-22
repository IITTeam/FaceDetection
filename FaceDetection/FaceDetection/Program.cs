using System;
using System.Windows.Forms;
using FaceDetection.Core;

namespace FaceDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            //Зарегистрируем(создадим) модуль-"ядро"
            ServicesWorker.Registration(new CoreModule());

            var exitFlag = false;
            while (!exitFlag)
            {
                Console.Write(">> ");
                var inputCommand = Console.ReadLine();

                switch (inputCommand)
                {
                    case "train":
                        //TODO
                        break;
                    case "check":

                        //Попросим у контейнера сервис 
                        var ts = ServicesWorker.GetInstance<FaceRecognizerService>();
                        //Дальше можно работать как просто с объектом
                        ts.StartCapture();
                        //TODO
                        break;
                    case "exit":
                        exitFlag = true;
                        break;
                    case "help":
                        Console.WriteLine("train check exit");
                        break;
                    default:
                        Console.WriteLine("Неверная команда!");
                        break;
                }

            }

        }
    }
}
