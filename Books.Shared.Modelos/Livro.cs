namespace Books.Shared.Modelos
{
    public class Livro
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public string? Editora { get; set; }
        public int AnoLancamento { get; set; }
        public int Versao { get; set; }

    }
}
