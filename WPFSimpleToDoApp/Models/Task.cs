using System;

namespace ToDoApp.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool IsDone { get; set; }

        public Task(string description)
        {
            Description = description;
            CreatedAt = DateTime.Now;
        }

        public Task()
        {
            CreatedAt = DateTime.Now;
        }

        public static System.Collections.Generic.IEnumerable<string> GetFilterableProperties()
        {
            yield return "CreatedAt";
            yield return "Name";
            yield return "Description";
            yield return "DueDate";
        }
    }
}
