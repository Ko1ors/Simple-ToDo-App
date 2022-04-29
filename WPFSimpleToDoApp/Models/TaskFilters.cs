namespace ToDoApp.Models
{
    public class TaskFilters
    {
        public int Id { get; set; }
        
        public bool? CompletedOnly { get; set; }

        public string OrderBy { get; set; }
        
        public bool Ascending { get; set; }
    }
    
}
