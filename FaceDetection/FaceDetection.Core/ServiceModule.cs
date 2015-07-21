namespace FaceDetection.Core
{
    /// <summary>
    /// Абстрактный класс для "модуля"
    /// </summary>
    public abstract class ServiceModule
    {
        public abstract void RegistServices(ServiceContainer container);
    }
}