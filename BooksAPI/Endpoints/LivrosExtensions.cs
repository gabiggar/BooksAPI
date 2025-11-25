using Books.Shared.Dados.Banco;
using Books.Shared.Modelos;
using BooksAPI.Request;
using BooksAPI.Response;
using Microsoft.AspNetCore.Mvc;

namespace BooksAPI.Endpoints
{
    public static class LivrosExtensions
    {
        public static void AddEndpointLivros(this WebApplication app)
        {
            app.MapGet("/Livros", ([FromServices] DAL<Livro> dal) =>
            {
                var listaDeLivros = dal.Listar();
                if (listaDeLivros is null)
                {
                    return Results.NotFound();
                }
                var listaDeLivrosResponse = EntityListToResponseList(listaDeLivros);
                return Results.Ok(listaDeLivrosResponse);
            });

            app.MapGet("/Livros/{id}", ([FromServices] DAL<Livro> dal, int id) =>
            {
                var livro = dal.RecuperarPor(l => l.Id == id);
                if(livro is null)
                {
                    return Results.NotFound("Livro não encontrado.");
                }
                return Results.Ok(EntityToResponse(livro));
            });

            app.MapPost ("/Livros", ([FromServices] DAL<Livro> dal, [FromBody] LivroRequest livroReq) =>
            {
                var livro = RequestToEntity(livroReq);
                dal.Adicionar(livro);
                return Results.Created($"/Livros/{livro.Id}",EntityToResponse(livro));
            });

            app.MapPut("/Livros", ([FromServices] DAL<Livro> dal, [FromBody] LivroRequestEdit livroReq) =>
            {
                var livroAAtualizar = dal.RecuperarPor(l => l.Id == livroReq.Id);
                if(livroAAtualizar is null)
                {
                    return Results.NotFound("Livro não encontrado.");
                }

                livroAAtualizar.Titulo = livroReq.Titulo;
                livroAAtualizar.Autor = livroReq.Autor;
                livroAAtualizar.Editora = livroReq.Editora;
                dal.Atualizar(livroAAtualizar);
                return Results.Ok(EntityToResponse(livroAAtualizar));
            });

            app.MapDelete("/Livros/{id}", ([FromServices] DAL<Livro> dal, int id) =>
            {
                var livro = dal.RecuperarPor(l => l.Id == id);
                if(livro is null)
                {
                    return Results.NotFound("Livro não encontrado");
                }
                dal.Deletar(livro);
                return Results.NoContent();
            });
        }

        private static Livro RequestToEntity(LivroRequest livroReq)
        {
            return new Livro
            {
                Titulo = livroReq.Titulo,
                Autor = livroReq.Autor,
                Editora = livroReq.Editora,
            };
        }

        private static ICollection<LivroResponse> EntityListToResponseList(IEnumerable<Livro> listaDeLivros)
        {
            return listaDeLivros.Select(l => EntityToResponse(l)).ToList();
        }

        private static LivroResponse EntityToResponse(Livro livro)
        {
            return new LivroResponse(livro.Id, livro.Titulo, livro.Autor, livro.Editora);
        }
    }
}
