namespace FIAP.ViewModel
{
    public class ProdutoPaginacaoResponseVM
    {
        public int TotalGeral { get; set; }

        public int TotalPaginas { get; set; }

        public string LinkProximo { get; set; }

        public string LinkAnterior { get; set; }

        public IList<ProdutoResponseVM> Produtos { get; set; }

    }
}
