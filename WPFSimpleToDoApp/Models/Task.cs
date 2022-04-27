using System;

namespace ToDoApp.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsDone { get; set; }

        public Task(string description)
        {
            Description = description;
        }
    }
}
