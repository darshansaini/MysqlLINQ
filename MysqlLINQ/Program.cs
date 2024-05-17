using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class TodoContext : DbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        try
        {
            optionsBuilder.UseMySql("Server=localhost;Database=darshan;User=root;Password=Old9928671555@;",
                new MySqlServerVersion(new Version(8, 0, 26)));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while configuring database connection: {ex.Message}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        try
        {
            using (var context = new TodoContext())
            {
                // Create
                var newItem = new TodoItem
                {
                    Title = "Manish",
                    Description = "It's only for testing",
                    Priority = 1,
                    DueDate = DateTime.Now.AddDays(7),
                    IsCompleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                context.TodoItems.Add(newItem);
                context.SaveChanges();
                Console.WriteLine("New todo item added.");

                // Read
                Console.WriteLine("All TodoItems in the database:");
                var todoItems = context.TodoItems.ToList();
                foreach (var item in todoItems)
                {
                    Console.WriteLine($"Id: {item.Id}, Title: {item.Title}, Description: {item.Description}, Priority: {item.Priority}, DueDate: {item.DueDate}, IsCompleted: {item.IsCompleted}, CreatedAt: {item.CreatedAt}, UpdatedAt: {item.UpdatedAt}");
                }

                // Update
                var itemToUpdate = context.TodoItems.FirstOrDefault();
                if (itemToUpdate != null)
                {
                    Console.WriteLine($"Todo item {itemToUpdate.Id} Updating.");
                    itemToUpdate.Title = "Updated Todo";
                    itemToUpdate.Description = "This todo item has been updated";
                    context.SaveChanges();
                    Console.WriteLine("Todo item updated.");
                    Console.WriteLine($"Id: {itemToUpdate.Id}, Title: {itemToUpdate.Title}, Description: {itemToUpdate.Description}, Priority: {itemToUpdate.Priority}, DueDate: {itemToUpdate.DueDate}, IsCompleted: {itemToUpdate.IsCompleted}, CreatedAt: {itemToUpdate.CreatedAt}, UpdatedAt: {itemToUpdate.UpdatedAt}");
                }

                // Delete
                var itemToDelete = context.TodoItems.FirstOrDefault();
                if (itemToDelete != null)
                {
                    Console.WriteLine($"Todo item {itemToDelete.Id} deleting.");
                    context.TodoItems.Remove(itemToDelete);
                    context.SaveChanges();
                    Console.WriteLine("Todo item deleted.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
