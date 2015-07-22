﻿using System;
using System.Windows.Forms;
using FaceDetection.Core;

namespace FaceDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitFlag = false;
            ServicesWorker.Registration(new CoreModule());

            //Попросим у контейнера сервис 
            var ts = ServicesWorker.GetInstance<FaceRecognizerService>();
            //Дальше можно работать как просто с объектом
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

                        //Зарегистрируем(создадим) модуль-"ядро"
                        
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