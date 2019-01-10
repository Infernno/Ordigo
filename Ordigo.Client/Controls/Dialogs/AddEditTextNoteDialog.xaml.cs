using System.Windows.Controls;
using Ordigo.Client.Core.ViewModels;

namespace Ordigo.Client.Controls.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для NewNoteDialog.xaml
    /// </summary>
    public partial class AddEditTextNoteDialog : UserControl
    {
        public AddEditTextNoteDialog()
        {
            InitializeComponent();
        }

        public AddEditTextNoteDialog(TextNoteViewModel model)
        {
            InitializeComponent();

            SetModel(model);
        }

        public void UpdateViewModel(TextNoteViewModel viewModel)
        {
            viewModel.Title = TitleBox.Text;
            viewModel.Content = ContentBox.Text;
        }

        public void SetModel(TextNoteViewModel model)
        {
            TitleBox.Text = model.Title;
            ContentBox.Text = model.Content;
        }
    }
}
