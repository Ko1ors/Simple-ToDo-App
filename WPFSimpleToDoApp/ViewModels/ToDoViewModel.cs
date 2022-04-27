using System.Collections.ObjectModel;
using ToDoApp.Commands;

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

        // add task command
        public RelayCommand AddTaskCommand { get; set; }

        public ToDoViewModel()
        {
            AddTaskCommand = new RelayCommand((obj) => AddTask(), (obj) => CanAddTask());
        }

        public bool CanAddTask()
        {
            return !string.IsNullOrWhiteSpace(NewTaskDescription); 
        }

        public void AddTask()
        {
            Tasks.Add(new Models.Task(NewTaskDescription));
            NewTaskDescription = string.Empty;
        }

        public void RemoveTask(Models.Task task)
        {
            Tasks.Remove(task);
        }

        public void ChangeTaskStatus(Models.Task task)
        {
            task.IsDone = !task.IsDone;
        }

    }
}
