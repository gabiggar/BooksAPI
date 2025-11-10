using Books.API.Endpoints;
using Books.Shared.Dados.Banco;
using Books.Shared.Modelos.Modelos;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<LivrosContext>();
builder.Services.AddTransient<DAL<Livro>>();

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

app.AddEndpointsLivros();

app.Run();
