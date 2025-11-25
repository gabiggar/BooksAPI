using Books.Shared.Dados.Banco;
using Books.Shared.Modelos;
using BooksAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LivrosContext>();
builder.Services.AddTransient<DAL<Livro>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddEndpointLivros();

app.Run();
