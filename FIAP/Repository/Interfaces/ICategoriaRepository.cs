using FIAP.Models;

namespace FIAP.Repository.Interfaces
{
     public interface ICategoriaRepository
    {
        public Task<IList<CategoriaModel>> FindAllAsync();

        public Task<CategoriaModel> FindByIdAsync(int id);

        public Task<int> InsertAsync(CategoriaModel categoriaModel);

        public Task UpdateAsync(CategoriaModel categoriaModel);

        public Task DeleteAsync(int id);

    }
}
