var builder = WebApplication.CreateBuilder(args);

// 🔥 Füge GetKnbResponse zur DI hinzu
builder.Services.AddSingleton<ChatbotApi.Services.GetKnbResponse>();

// Registriere den OpenAiService mit HttpClient-Unterstützung
builder.Services.AddHttpClient<ChatbotApi.Services.OpenAiService>();

// Registriere Controller
builder.Services.AddControllers();

// CORS-Regeln festlegen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Setzt die URL für das Hosting
builder.WebHost.UseUrls("http://localhost:5000");

// Erstelle die App
var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Starte die App
app.Run();
