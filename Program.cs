using Microsoft.EntityFrameworkCore;
using BibliotecaApi.Data;
using BibliotecaApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configura o DbContext para usar SQLite
builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseSqlite("Data Source=biblioteca.db"));

// Adiciona serviços para Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ativa o Swagger se o ambiente for de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Mapeia os endpoints da Biblioteca
app.MapGet("/api/biblioteca", async (BibliotecaContext context) =>
    await context.Bibliotecas.ToListAsync())
    .WithName("GetBibliotecas");

app.MapGet("/api/biblioteca/{id}", async (int id, BibliotecaContext context) =>
    await context.Bibliotecas.FindAsync(id) is Biblioteca biblioteca ? Results.Ok(biblioteca) : Results.NotFound())
    .WithName("GetBiblioteca");

app.MapPost("/api/biblioteca", async (Biblioteca biblioteca, BibliotecaContext context) =>
{
    context.Bibliotecas.Add(biblioteca);
    await context.SaveChangesAsync();
    return Results.Created($"/api/biblioteca/{biblioteca.Id}", biblioteca);
})
.WithName("PostBiblioteca");

app.MapPut("/api/biblioteca/{id}", async (int id, Biblioteca updatedBiblioteca, BibliotecaContext context) =>
{
    var biblioteca = await context.Bibliotecas.FindAsync(id);
    if (biblioteca is null) return Results.NotFound();

    biblioteca.Nome = updatedBiblioteca.Nome;
    biblioteca.InicioFuncionamento = updatedBiblioteca.InicioFuncionamento;
    biblioteca.FimFuncionamento = updatedBiblioteca.FimFuncionamento;
    biblioteca.Inauguracao = updatedBiblioteca.Inauguracao;
    biblioteca.Contato = updatedBiblioteca.Contato;

    await context.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("PutBiblioteca");

app.MapDelete("/api/biblioteca/{id}", async (int id, BibliotecaContext context) =>
{
    var biblioteca = await context.Bibliotecas.FindAsync(id);
    if (biblioteca is null) return Results.NotFound();

    context.Bibliotecas.Remove(biblioteca);
    await context.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteBiblioteca");

app.Run();