using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

var factory = new CookbookContextFactory();
using var dbcontext = factory.CreateDbContext(args);

//Add
//Console.WriteLine("Add Porridge for breakfast");
//var porridge = new Dish { Title = "Breakfast Porridge", Notes = "This is so good", Stars = 4 };
//context.Dishes.Add(porridge);
//await context.SaveChangesAsync();
//Console.WriteLine("Added Porridge Successfully");

//Query
//Console.WriteLine("Reading Stars of porridge");
//var dishes = await context.Dishes.Where(d => d.Title.Contains("Porridge")).ToListAsync();
//if (dishes.Count != 1)
//    Console.WriteLine("Something really bad happend.Porridge disappeared");
//Console.WriteLine($"Porridge has {dishes[0].Stars} stars");
//Update
//Console.WriteLine("Change Porridge Stars");
//porridge.Stars = 5;
//await context.SaveChangesAsync();
//Console.WriteLine("Changed Porridge Stars Successfully!");

//Delete
//Console.WriteLine("Removing Porridge from database");
//context.Dishes.Remove(porridge);
//await context.SaveChangesAsync();
//Console.WriteLine("Deleted Porridge Successfully");

//An Experiment
var newDish = new Dish { Title = "Foo", Notes = "Bar" };
dbcontext.Dishes.Add(newDish);
await dbcontext.SaveChangesAsync();
newDish.Notes = "Baz";
await dbcontext.SaveChangesAsync();
await EntityStates(factory);
await ChangeTracking(factory);
static async Task EntityStates(CookbookContextFactory factory)
{
    using var dbcontext = factory.CreateDbContext();
    var newDish = new Dish { Title = "Foo", Notes = "Bar" };
    var state = dbcontext.Entry(newDish).State;//-->detached

    dbcontext.Dishes.Add(newDish);
    state = dbcontext.Entry(newDish).State;//-->Added
    await dbcontext.SaveChangesAsync();

    state = dbcontext.Entry(newDish).State;//-->Unchanged
    newDish.Notes = "Baz";
    state = dbcontext.Entry(newDish).State;//--Modified
    await dbcontext.SaveChangesAsync();


    dbcontext.Dishes.Remove(newDish);
    state = dbcontext.Entry(newDish).State;//-->Deleted
    await dbcontext.SaveChangesAsync();
    state = dbcontext.Entry(newDish).State;//-->Detached
}
static async Task ChangeTracking(CookbookContextFactory factory)
{
    using var dbcontext = factory.CreateDbContext();

    var newDish = new Dish { Title = "Foo", Notes = "Bar" };
    dbcontext.Dishes.Add(newDish);
    await dbcontext.SaveChangesAsync();
    newDish.Notes = "Bazs";

    var entry = dbcontext.Entry(newDish);
    var original = entry.OriginalValues[nameof(Dish.Notes)].ToString();
    var dishFromDataBase = await dbcontext.Dishes.SingleAsync(d => d.Id == newDish.Id);

    using var dbContext2 = factory.CreateDbContext();
    var dishFromDataBase2 = await dbContext2.Dishes.SingleAsync(d => d.Id == newDish.Id);
}
class Dish
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Title { get; set; }
    [MaxLength(1000)]
    public string? Notes { get; set; }
    public int Stars { get; set; }
    public List<DishIngridient> Ingridients = new();
}
class DishIngridient
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Description { get; set; }
    [MaxLength(50)]
    public string UnitOfMeasure { get; set; } = string.Empty;
    [Column(TypeName = "decimal(5,2)")]
    public decimal Amount { get; set; }
    public Dish? Dish { get; set; }
    public int DishId { get; set; }

}
class CookBookContext : DbContext
{

    public DbSet<Dish> Dishes { get; set; }
    public DbSet<DishIngridient> Ingridients { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CookBookContext(DbContextOptions<CookBookContext> options) :
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        base(options)
    { }
}
class CookbookContextFactory : IDesignTimeDbContextFactory<CookBookContext>
{
    public CookBookContext CreateDbContext(string[]? args = null)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var optionsBuilder = new DbContextOptionsBuilder<CookBookContext>();
        optionsBuilder
            // Uncomment the following line if you want to print generated
            // SQL statements on the console.
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

        return new CookBookContext(optionsBuilder.Options);
    }
}

