var builder = WebApplication.CreateBuilder(args);

// 🔹 Adiciona suporte a controllers
builder.Services.AddControllers();

// 🔹 HttpClient para consumir APIs externas
builder.Services.AddHttpClient();

// 🔹 Seus serviços
builder.Services.AddScoped<CidadeService>();
builder.Services.AddScoped<ClimaService>();
builder.Services.AddOpenApi();

var app = builder.Build();

// Swagger/OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// 🔹 IMPORTANTE: mapear controllers
app.MapControllers();

app.UseHttpsRedirection();



// 🔹 Rodar na porta 3000 
app.Run("http://localhost:3000");
