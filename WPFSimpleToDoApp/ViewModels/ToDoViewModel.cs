using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ToDoApp.Commands;
using ToDoApp.DB;

namespace ToDoApp.ViewModels
{
    public class ToDoViewModel : ViewModelBase
    {
        public ICollectionView TasksCollectionView
        {
            get { return CollectionViewSource.GetDefaultView(Tasks); }
        }

        public ObservableCollection<Models.Task> Tasks { get; set; } = new ObservableCollection<Models.Task>();

        private string _newTaskDescription;

        public string NewTaskDescription
        {
            get { return _newTaskDescription; }
            set
            {
                _newTaskDescription = value;
                OnPropertyChanged("NewTaskDescription");
            }
        }

        public Visibility LogoVisibility => TasksCollectionView.IsEmpty ? Visibility.Visible : Visibility.Collapsed;

        public RelayCommand AddTaskCommand { get; set; }

        public RelayCommand RemoveTaskCommand { get; set; }

        public RelayCommand ChangeTaskStatusCommand { get; set; }

        public RelayCommand OpenFiltersCommand { get; set; }

        public Views.FiltersWindow? FiltersWindow { get; set; }

        public ToDoViewModel()
        {
            _ = Init();
        }

        private async Task Init()
        {
            using (var context = new ToDoContext())
            {
                await context.Database.EnsureCreatedAsync();
                var tasks = context.Tasks.ToList();
                foreach (var task in tasks)
                {
                    Tasks.Add(task);
                }
            }
            FilterTasks();

            AddTaskCommand = new RelayCommand((obj) => AddTask(), (obj) => CanAddTask());
            RemoveTaskCommand = new RelayCommand((obj) => RemoveTask(obj as Models.Task));
            ChangeTaskStatusCommand = new RelayCommand((obj) => ChangeTaskStatus(obj as Models.Task));
            OpenFiltersCommand = new RelayCommand((obj) => OpenFilters(), (obj) => CanOpenFilters);
        }

        public bool CanAddTask()
        {
            return !string.IsNullOrWhiteSpace(NewTaskDescription);
        }

        public async void AddTask()
        {
            var newTask = new Models.Task(NewTaskDescription);
            using (var context = new ToDoContext())
            {
                context.Tasks.Add(newTask);
                await context.SaveChangesAsync();
            }
            Tasks.Add(newTask);
            FilterTasks();
            NewTaskDescription = string.Empty;
        }

        public async void RemoveTask(Models.Task task)
        {
            using (var context = new ToDoContext())
            {
                context.Tasks.Remove(task);
                await context.SaveChangesAsync();
            }
            Tasks.Remove(task);
            OnPropertyChanged("LogoVisibility");
        }

        public bool CanOpenFilters => FiltersWindow is null;

        private void OpenFilters()
        {
            FiltersWindow = new Views.FiltersWindow();
            FiltersWindow.OnFiltersChanged(RefreshTasks);
            FiltersWindow.Closed += (sender, args) => FiltersWindow = null;
            FiltersWindow.Show();
        }

        private void RefreshTasks(object? sender, EventArgs e)
        {
            FilterTasks();
        }

        private void FilterTasks()
        {
            using (var context = new ToDoContext())
            {
                var filters = context.TaskFilters.FirstOrDefault();
                if(filters is null)
                {
                    TasksCollectionView.Filter = null;
                    return;
                }
                TasksCollectionView.SortDescriptions.Clear();
                TasksCollectionView.SortDescriptions.Add(new SortDescription(filters.OrderBy, filters.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending));
                TasksCollectionView.Filter = (obj) =>
                {
                    var task = obj as Models.Task;
                    if (!filters.CompletedOnly.HasValue)
                        return true;
                    if (filters.CompletedOnly.Value && task.IsDone)
                        return true;
                    if (!filters.CompletedOnly.Value && !task.IsDone)
                        return true;
                    return false;
                };
            }
            OnPropertyChanged("LogoVisibility");
        }


        public async void ChangeTaskStatus(Models.Task task)
        {
            using (var context = new ToDoContext())
            {
                context.Update(task);
                await context.SaveChangesAsync();
            }
        }

    }
}
