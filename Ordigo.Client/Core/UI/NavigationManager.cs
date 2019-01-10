using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Ordigo.Client.Core.UI
{
    /// <summary>
    /// Класс, отвечающий за переход между различными экрана приложения
    /// </summary>
    public sealed class NavigationManager
    {
        #region Fields

        /// <summary>
        /// Статистичесий указатель на текущий экземпляр класса (паттерн Singleton)
        /// </summary>
        private static NavigationManager mInstance;

        /// <summary>
        /// Кэш экранов приложения (например экрана входа, регистрации и т.д.)
        /// По названию возвращает тип, который позже будет создан контейнером
        /// </summary>
        private readonly Dictionary<string, Type> mViewCache;

        /// <summary>
        /// Главное окно приложения куда будет устанавливаться содержимое (<see cref="UserControl"/>) 
        /// </summary>
        private IContentControl mRootControl;

        /// <summary>
        /// Контейнер DI, который содержит все классы и их зависимости
        /// </summary>
        private Container mViewResolver;

        #endregion

        #region Properties

        /// <summary>
        /// Текущий экземляр класса <see cref="NavigationManager"/>
        /// </summary>
        public static NavigationManager Current
        {
            get
            {
                if (mInstance == null)
                    mInstance = new NavigationManager();

                return mInstance;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Конструктор класса <see cref="NavigationManager"/>
        /// </summary>
        private NavigationManager()
        {
            mViewCache = new Dictionary<string, Type>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Захватывает главное окно в котором будет меняться содержимое
        /// </summary>
        /// <param name="root">Контрол, реализующий <see cref="IContentControl"/></param>
        public void SetRoot(IContentControl root)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            mRootControl = root;
        }

        /// <summary>
        /// Устанавливает DI контейнер, разрещающий зависимости
        /// </summary>
        /// <param name="container">DI контейнер</param>
        public void SetResolver(Container container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            mViewResolver = container;

            RefreshCache();
        }

        private void RefreshCache()
        {
            if (mViewResolver == null)
                throw new InvalidOperationException("View resolver is null");

            mViewCache.Clear();

            var views = mViewResolver.GetCurrentRegistrations()
                .Select(r => r.ServiceType)
                .Where(t => t.IsSubclassOf(typeof(UserControl)))
                .ToArray();

            foreach (var view in views)
            {
                mViewCache.Add(view.Name, view);
            }
        }

        public void GoTo(string viewName)
        {
            if(!mViewCache.ContainsKey(viewName))
                throw new ArgumentException($"View {viewName} doesn't exist!");

            var type = mViewCache[viewName];
            var instance = mViewResolver.GetInstance(type);

            mRootControl.CurrentContent = instance;
        }

        #endregion
    }
}
