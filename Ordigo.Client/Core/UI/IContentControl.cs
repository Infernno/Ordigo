namespace Ordigo.Client.Core.UI
{
    /// <summary>
    /// Интерфейс для управления содержимым окна
    /// </summary>
    public interface IContentControl
    {
        /// <summary>
        /// Активный элемент управления
        /// </summary>
        object CurrentContent { get; set; }
    }
}
