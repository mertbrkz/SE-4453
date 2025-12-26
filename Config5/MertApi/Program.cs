using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure KeyVault
var keyVaultName = builder.Configuration["KeyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName))
{
    try
    {
        var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
        builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
        Console.WriteLine($"Successfully configured KeyVault: {keyVaultName}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"KeyVault access failed: {ex.Message}");
        // Continuing without KeyVault to avoid 503 Crash
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
// Enable Swagger for all environments for this project requirement
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/hello", () => "Hello World from MertApi!");

app.Run();
