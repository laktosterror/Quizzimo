using System.Windows;
using System.Windows.Controls;
using QuizWPF.ViewModel;

namespace QuizWPF.View;

public partial class PlayerView : UserControl
{
    public PlayerView(PlayerViewModel viewModel)
    {
        InitializeComponent();
        this.DataContext = viewModel;

    }
}