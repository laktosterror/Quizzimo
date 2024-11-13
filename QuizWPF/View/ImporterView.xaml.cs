using System.Windows.Controls;
using QuizWPF.ViewModel;

namespace QuizWPF.View;

/// <summary>
///     Interaction logic for ImporterView.xaml
/// </summary>
public partial class ImporterView : UserControl
{
    public ImporterView(ImporterViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}