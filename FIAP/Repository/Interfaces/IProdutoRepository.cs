using FIAP.Models;

namespace FIAP.Repository.Interfaces
{
    public interface IProdutoRepository
    {
        Task<IList<ProdutoModel>> FindAllAsync(int pagina, int tamanho);
        Task<IList<ProdutoModel>> FindAllByIdAsync(int IdProduto, int tamanho);
        Task<IList<ProdutoModel>> FindAllAsync();
        Task<IList<ProdutoModel>> FindByNomeAsync(string nome);
        Task<int> CountAsync();
        Task<ProdutoModel> FindByIdAsync(int id);
        Task<int> InsertAsync(ProdutoModel produtoModel);
        Task UpdateAsync(ProdutoModel produtoModel);
        Task DeleteAsync(ProdutoModel produtoModel);
        Task DeleteAsync(int id);
    }
}
