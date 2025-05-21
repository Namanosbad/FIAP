using FIAP.Models;

namespace FIAP.Repository.Interfaces
{
    public interface IUsuarioRepository
    {
        public Task <IList<UsuarioModel>> FindAllAsync();

        public Task <UsuarioModel> FindByIdAsync(int id);

        public Task<UsuarioModel> FindByEmailAndSenhaAsync(string email, string senha);

        public Task<int> InsertAsync(UsuarioModel usuarioModel);

        public Task UpdateAsync(UsuarioModel usuarioModel);

        public Task DeleteAsync(int id);

    }
}
