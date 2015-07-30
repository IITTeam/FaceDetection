using System;
using System.Collections.Generic;
using System.Linq;

namespace FaceDetection.CommandProcessor
{
    public class Application
    {
        public List<ICommand> AvailableCommands = new List<ICommand>();
        public List<string> Parameters = new List<string>();

        public Application()
        {
            AvailableCommands.Add(new HelpCommand(this));
            AvailableCommands.Add(new AddFaceCommand());
            AvailableCommands.Add(new CutFacesCommand());
            AvailableCommands.Add(new TrainCommand());
            AvailableCommands.Add(new LoadCommand());
            AvailableCommands.Add(new DetectCommand());
            AvailableCommands.Add(new TrainGenderCommand());
            AvailableCommands.Add(new ExitCommand(this));
        }

        public void Run()
        {
            AvailableCommands.FirstOrDefault(command => command.Name == "help")?.Execute();
            //  Console.WriteLine(@"Для получения справки по допустимым командам введите help  и нажмите Enter");
            while (true)
            {
                Console.Write('>');
                var str = Console.ReadLine().ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (str.Count != 0)
                {
                    var selectedCommand = AvailableCommands.FirstOrDefault(command => command.Name.ToLower() == str[0]);

                    if (selectedCommand != null)
                    {
                        str.RemoveAt(0);
                        Parameters = str;
                        selectedCommand.Execute();
                    }
                    else
                        Console.WriteLine(@"Такой команды нет. Попробуйте еще раз.");
                }
                else
                {
                    Console.WriteLine(@"Ошибка ввода. Попробуйте еще раз.");
                }
            }
        }
    }
}