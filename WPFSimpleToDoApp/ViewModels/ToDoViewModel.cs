using System.Collections.ObjectModel;
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

        public ToDoViewModel()
        {
            _ = Init();   
        }

        private async Task Init()
        {
            using (var context = new ToDoContext())
            {
                await context.Database.EnsureCreatedAsync();
                foreach (var task in context.Tasks)
                {
                    Tasks.Add(task);
                }
            }

            AddTaskCommand = new RelayCommand((obj) => AddTask(), (obj) => CanAddTask());
            RemoveTaskCommand = new RelayCommand((obj) => RemoveTask(obj as Models.Task));
            ChangeTaskStatusCommand = new RelayCommand((obj) => ChangeTaskStatus(obj as Models.Task));
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
