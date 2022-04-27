using System.Collections.ObjectModel;
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
            AddTaskCommand = new RelayCommand((obj) => AddTask(), (obj) => CanAddTask());
            RemoveTaskCommand = new RelayCommand((obj) => RemoveTask(obj as Models.Task));
            ChangeTaskStatusCommand = new RelayCommand((obj) => ChangeTaskStatus(obj as Models.Task));
            
            using (var context = new ToDoContext())
            {
                context.Database.EnsureCreated();
                foreach (var task in context.Tasks)
                {
                    Tasks.Add(task);
                }
            }
        }

        public bool CanAddTask()
        {
            return !string.IsNullOrWhiteSpace(NewTaskDescription); 
        }
      
        public void AddTask()
        {
            var newTask = new Models.Task(NewTaskDescription);
            using (var context = new ToDoContext())
            {
                context.Tasks.Add(newTask);
                context.SaveChanges();
            }
            Tasks.Add(newTask);
            NewTaskDescription = string.Empty;
        }

        public void RemoveTask(Models.Task task)
        {
            using (var context = new ToDoContext())
            {
                context.Tasks.Remove(task);
                context.SaveChanges();
            }
            Tasks.Remove(task);
        }

        public void ChangeTaskStatus(Models.Task task)
        {
            using (var context = new ToDoContext())
            {
                context.Update(task);
                context.SaveChanges();
            }
        }

    }
}
