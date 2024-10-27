namespace BibliotecaApi.Models
{
    public class Biblioteca
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string InicioFuncionamento { get; set; }
        public string FimFuncionamento { get; set; }
        public int Inauguracao { get; set; }
        public string Contato { get; set; }
    }
}