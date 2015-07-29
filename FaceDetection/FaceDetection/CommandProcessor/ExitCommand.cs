using System;
using System.Collections.Generic;

namespace FaceDetection.CommandProcessor
{
    public class ExitCommand : ICommand
    {
        public string Name
        {
            get { return "exit"; }
        }

        public string Description
        {
            get { return "- завершает работу приложения."; }
        }
        private Application _app;

        public ExitCommand(Application app)
        {
            _app = app;
        }

        public void Execute(List<string> parameters)
        {
            Environment.Exit(0);
        }
    }
}