namespace FaceDetection.Core
{
    public static class ServicesWorker
    {
        /// <summary>
        /// Контейнер для сервисов
        /// </summary>
        private static ServiceContainer ServiceContainer { get; set; }


        /// <summary>
        /// Метод регистрации сервисов
        /// </summary>
        public static void Registration(params ServiceModule[] modules)
        {
            ServiceContainer = new ServiceContainer();

            foreach (var module in modules)
            {
                module.RegistServices(ServiceContainer);
            }
        }

        /// <summary>
        /// Получение экземляра нужного типа
        /// </summary>
        /// <typeparam name="T"> Тип возвращаемого объекта </typeparam>
        /// <returns> Экземпляр нужного типа</returns>
        public static T GetInstance<T>() where T : class
        {
            return ServiceContainer.GetService(typeof(T)) as T;
        }
    }
}
