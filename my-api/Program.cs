// --- Nossas adições ---
using Microsoft.EntityFrameworkCore;
using MinhaApi.Data;
// ----------------------

var builder = WebApplication.CreateBuilder(args);

// --- Nossas adições ---
// 1. Configura o DbContext com a string de conexão
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MeuDbContext>(options =>
    options.UseSqlServer(connectionString));
// ----------------------

// 2. Serviços que o Visual Studio já adicionou
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Configuração do Pipeline (Swagger)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 4. (Opcional) Se você DESMARCOU "HTTPS" na criação,
// esta linha "app.UseHttpsRedirection();" NÃO deve estar aqui.
// Se estiver, delete-a.

// --- Nossas adições ---
// 5. Bloco para rodar as Migrations na inicialização
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MeuDbContext>();
    db.Database.Migrate(); // Aplica as migrations
}
// ----------------------

// 6. Linhas que o Visual Studio já adicionou
app.UseAuthorization();

app.MapControllers(); // Esta linha é crucial!

// 7. Inicia a API
app.Run();