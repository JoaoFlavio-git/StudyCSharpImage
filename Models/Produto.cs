namespace Imagens.Models
{
    public class Produto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public byte[]? Foto { get; set; }
    }
}

