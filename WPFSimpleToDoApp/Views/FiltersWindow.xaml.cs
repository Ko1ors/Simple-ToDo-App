using System;
using System.Windows;
using ToDoApp.ViewModels;

namespace ToDoApp.Views
{
    /// <summary>
    /// Interaction logic for FiltersWindow.xaml
    /// </summary>
    public partial class FiltersWindow : Window
    {
        private FiltersViewModel FiltersViewModel { get; set; }
        public FiltersWindow()
        {
            InitializeComponent();
            FiltersViewModel = new FiltersViewModel();
            DataContext = FiltersViewModel;
            
            var mainWindow = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.Manual;
            Left = mainWindow.Left + mainWindow.Width;
            Top = mainWindow.Top;
        }

        public void OnFiltersChanged(EventHandler handler)
        {
            FiltersViewModel.FiltersChanged += handler;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
