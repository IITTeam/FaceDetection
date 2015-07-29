using System.Collections.Generic;

namespace FaceDetection.CommandProcessor
{
    public interface ICommand
    {
        string Name { get; }

        string Description { get; }

        void Execute(List<string> parameters);
    }
}
