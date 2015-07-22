using System;
using System.Windows.Forms;
using FaceDetection.Core;

namespace FaceDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Для завершения нажмите любую клавишу!");
            //Зарегистрируем(создадим) модуль-"ядро"
            ServicesWorker.Registration(new CoreModule());

            //Попросим у контейнера сервис 
            var ts = ServicesWorker.GetInstance<FaceRecognizerService>();
            //Дальше можно работать как просто с объектом
            ts.StartCapture();

            Console.ReadKey();

        }
    }
}
