using Books.API.Requests;
using Books.API.Responses;
using Books.Shared.Dados.Banco;
using Books.Shared.Modelos.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Books.API.Endpoints
{
    public static class LivrosExtensions
    {
        public static void AddEndpointsLivros(this WebApplication app)
        {
            app.MapGet("/Livros", ([FromServices] DAL<Livro> dal) =>
            {
                return EntityListToResponseList(dal.Listar());
            });

            app.MapGet("/Livros/{nome}", ([FromServices] DAL<Livro> dal, string nome) =>
            {
                var livro = dal.RecuperarPor(l => l.Nome.ToUpper().Equals(nome.ToUpper()));
                if (livro == null)
                {
                    return Results.NotFound("Livro não encontrado");
                }
                return Results.Ok(EntityToResponse(livro));
            });

            app.MapPost("/Livros", ([FromServices] DAL<Livro> dal, [FromBody] LivrosRequest livrosReq) =>
            {
                var livro = RequestToEntity(livrosReq);
                dal.Adicionar(livro);
                return Results.Created($"/Livros/{livro.Id}", EntityToResponse(livro));
            });

            app.MapPut("/Livros/{id}", ([FromServices] DAL<Livro> dal, int id, [FromBody] LivrosRequest livroReq) =>
            {
                var livroExistente = dal.RecuperarPor(l => l.Id == id);
                if (livroExistente is null)
                {
                    return Results.NotFound("Livro não encontrado");
                }
                livroExistente.Nome = livroReq.Nome;
                livroExistente.Autor = livroReq.Autor;
                livroExistente.Editora = livroReq.Editora;

                dal.Atualizar(livroExistente);
                return Results.NoContent();
            });

            app.MapDelete("/Livros/{id}", ([FromServices] DAL<Livro> dal, int id) =>
            {
                var livro = dal.RecuperarPor(l => l.Id == id);
                if (livro is null)
                {
                    return Results.NotFound("Livro não encontrado");
                }
                dal.Deletar(livro);
                return Results.NoContent();
            });
        }

        private static Livro RequestToEntity(LivrosRequest livrosReq)
        {
            return new Livro
            {
                Nome = livrosReq.Nome,
                Autor = livrosReq.Autor,
                Editora = livrosReq.Editora
            };
        }

        private static ICollection<LivrosResponse> EntityListToResponseList(IEnumerable<Livro> livros)
        {
            return livros.Select(l => EntityToResponse(l)).ToList();
        }

        private static LivrosResponse EntityToResponse(Livro livros)
        {
            return new LivrosResponse(
                livros.Id,
                livros.Nome,
                livros.Autor,
                livros.Editora
            );
        }

    }
}
