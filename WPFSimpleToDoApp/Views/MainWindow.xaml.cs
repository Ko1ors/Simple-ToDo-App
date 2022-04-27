using System.Windows;
using ToDoApp.ViewModels;

namespace ToDoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ToDoViewModel();
        }
    }
}
