using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Commands;
using ToDoApp.DB;

namespace ToDoApp.ViewModels
{
    public class ToDoViewModel : ViewModelBase
    {
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
            }

            RefreshTasks();

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
            RefreshTasks();
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
            RefreshTasks();
        }

        private void RefreshTasks()
        {
            using (var context = new ToDoContext())
            {
                Tasks.Clear();
                var filters = context.TaskFilters.FirstOrDefault();
                List<Models.Task> tasks;
                if (filters is null)
                    tasks = context.Tasks.ToList();
                else if (filters.Ascending)
                    tasks = context.Tasks.OrderByProperty(filters.OrderBy).ToList();
                else
                    tasks = context.Tasks.OrderByPropertyDescending(filters.OrderBy).ToList();

                foreach (var task in tasks)
                    Tasks.Add(task);
            }
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
