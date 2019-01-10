using System.Windows;
using Ordigo.Client.Core.UI;

namespace Ordigo.Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IContentControl
    {
        public object CurrentContent
        {
            get => ContentControl.Content;
            set => ContentControl.Content = value;
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
