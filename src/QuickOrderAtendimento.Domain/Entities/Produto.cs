namespace QuickOrderAtendimento.Domain.Entities
{
    public class Produto
    {
        public Produto(string categoriaProduto, string nomeProduto, int idProduto, int quantidade, double valorProduto, List<ProdutoItens>? produtosItens)
        {
            CategoriaProduto = categoriaProduto;
            NomeProduto = nomeProduto;
            IdProduto = idProduto;
            Quantidade = quantidade;
            ValorProduto = valorProduto;
            ProdutosItens = produtosItens;
        }

        public string CategoriaProduto { get; set; }
        public string NomeProduto { get; set; }
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
        public double ValorProduto { get; set; }
        public List<ProdutoItens>? ProdutosItens { get; set; }

    }
}
