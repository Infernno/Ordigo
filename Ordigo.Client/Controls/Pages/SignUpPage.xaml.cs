using System.Windows.Controls;
using Ordigo.Client.Core.ViewModels;

namespace Ordigo.Client.Controls.Pages
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : UserControl
    {
        public SignUpPage(SignUpViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
