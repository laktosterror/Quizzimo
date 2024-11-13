using System.Windows.Controls;
using QuizWPF.ViewModel;

namespace QuizWPF.View;

/// <summary>
///     Interaction logic for ResultsView.xaml
/// </summary>
public partial class ResultsView : UserControl
{
    public ResultsView(PlayerViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}