using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuizWPF.Model;
using QuizWPF.ViewModel;

namespace QuizWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();

        // tillåt fönstermove
        ContentGrid.MouseDown += ContentGrid_MouseDown;
    }

    private void ContentGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        try
        {
            DragMove();
        }
        catch
        {
            Console.WriteLine("only action on left click!");
        }
        
    }
    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        // Collapse the FocusableMenuView when clicking anywhere in the window
        var viewModel = (MainWindowViewModel)DataContext;
        viewModel.CollapsibleMenuVisibility = Visibility.Collapsed;
    }
    private void CollapsibleMenuView_MouseDown(object sender, MouseButtonEventArgs e)
    {
        // Prevent the FocusableMenuView from collapsing when clicked
        e.Handled = true; // Mark the event as handled
    }
}