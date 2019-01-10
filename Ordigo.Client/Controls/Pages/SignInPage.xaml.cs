using System.Windows.Controls;
using Ordigo.Client.Core.ViewModels;

namespace Ordigo.Client.Controls.Pages
{
    /// <summary>
    /// Логика взаимодействия для SignInPage.xaml
    /// </summary>
    public partial class SignInPage : UserControl
    {
        public SignInPage(SignInViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}