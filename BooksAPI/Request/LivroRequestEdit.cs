namespace BooksAPI.Request
{
    public record LivroRequestEdit(int Id, string Titulo, string Autor, string Editora) : LivroRequest(Titulo, Autor, Editora);
}
