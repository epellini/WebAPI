using Microsoft.EntityFrameworkCore;
using QuotesWebApi.Data;
using QuotesWebApi.Services;
using System.Text.Json.Serialization; // Import this namespace

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(options =>
{
   // options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
   // options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // Handle circular references
     options.JsonSerializerOptions.WriteIndented = true; // Optional: Makes the JSON output easy to read
});



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IQuotesService, QuotesService>();

builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


// Seed the database with default data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate(); // Ensure the database is created and migrated
    context.SeedData(); // Seed the data
}

app.Run();
