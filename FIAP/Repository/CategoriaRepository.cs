using FIAP.Data;
using FIAP.Models;
using FIAP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FIAP.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly DataContext _dataContext;

        public CategoriaRepository(DataContext ctx)
        {
            _dataContext = ctx;
        }

        public async Task DeleteAsync(int id)
        {
            var categoria = new CategoriaModel() { CategoriaId = id };

            _dataContext.Categorias
                .Remove(categoria);
            await _dataContext
                .SaveChangesAsync();
        }

        public async Task<IList<CategoriaModel>> FindAllAsync()
        {
            return await _dataContext.Categorias
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CategoriaModel> FindByIdAsync(int id)
        {
            return await _dataContext.Categorias
                .AsNoTracking().
                FirstOrDefaultAsync(c => c.CategoriaId == id);
        }

        public async Task <int> InsertAsync(CategoriaModel categoriaModel)
        {
            _dataContext.Categorias.Add(categoriaModel);
            await _dataContext.SaveChangesAsync();

            return categoriaModel.CategoriaId;
        }

        public async Task UpdateAsync(CategoriaModel categoriaModel)
        {
            _dataContext.Categorias.Update(categoriaModel);
            await _dataContext.SaveChangesAsync();
        }

    }
}