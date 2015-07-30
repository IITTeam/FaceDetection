using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaceDetection.CommandProcessor
{
    public class HelpCommand : ICommand
    {
        public string Name => "help";

        public string Description => "- вводит список доступных команд;";
        private readonly Application _app;

        public HelpCommand(Application app)
        {
            _app = app;
        }

        public void Execute()
        {
            Console.WriteLine(@"=====================================================================");
            foreach (var item in _app.AvailableCommands)
            {
                Console.WriteLine(item.Name + " " + item.Description);
            }
            Console.WriteLine(@"=====================================================================");
        }
    }
}
