using FaceDetection.Core;
namespace FaceDetection.Core
{
    /// <summary>
    /// Модуль-"ядро" приложения
    /// </summary>
    public class CoreModule : ServiceModule
    {
        /// <summary>
        /// Регистрация сервисов модуля
        /// </summary>
        /// <param name="container"></param>
        public override void RegistServices(ServiceContainer container)
        {
            container.RegisterService(new FaceRecognizerService());
            container.RegisterService(new VoiceAssistantService());
            container.RegisterService(new DatabaseService());
            container.RegisterService(new HumanService());
        }
    }
}