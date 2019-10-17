namespace BaseScripts
{
    public abstract class BaseController
    {
        public bool IsActive { get; private set; }

        /// <summary>
        /// Включаем контроллер
        /// </summary>
        public virtual void On()
        {
            IsActive = true;
        }

        /// <summary>
        /// Выключаем контроллер
        /// </summary>
        public virtual void Off()
        {
            IsActive = false;
        }

        /// <summary>
        /// Переключаем контроллер
        /// </summary>
        public virtual void Switch()
        {
            IsActive = !IsActive;
        }

        public virtual void ControllerUpdate() { }

        public virtual void ControllerLateUpdate(float time) { }
    }
}
