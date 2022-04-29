using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Commands;
using ToDoApp.DB;
using ToDoApp.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoApp.ViewModels
{
    public class FiltersViewModel : ViewModelBase
    {
        public event EventHandler FiltersChanged;

        public TaskFilters Filters { get; set; }

        public List<string> OrderByOptions { get; set; }

        public string OrderIcon => Filters.Ascending ? "ArrowCircleUp" : "ArrowCircleDown";

        public RelayCommand ChangeOrderCommand { get; set; }
        
        public RelayCommand ChangeOrderByCommand { get; set; }

        public FiltersViewModel()
        {
            _ = Init();
        }

        private async Task Init()
        {
            OrderByOptions = Models.Task.GetFilterableProperties().ToList();
            using (var context = new ToDoContext())
            {
                await context.Database.EnsureCreatedAsync();
                Filters = context.TaskFilters.FirstOrDefault();
                if (Filters is null)
                {
                    Filters = new TaskFilters();
                    Filters.OrderBy = OrderByOptions.First();
                    context.TaskFilters.Add(Filters);
                    await context.SaveChangesAsync();
                }
            }

            ChangeOrderCommand = new RelayCommand((obj)=> ChangeOrder());
            ChangeOrderByCommand = new RelayCommand((obj) => UpdateFilters());
        }

        private void ChangeOrder()
        {
            Filters.Ascending = !Filters.Ascending;
            UpdateFilters();
            OnPropertyChanged("OrderIcon");
        }

        public void UpdateFilters()
        {
            using var context = new ToDoContext();
            context.TaskFilters.Update(Filters);
            context.SaveChanges();
            FiltersChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
