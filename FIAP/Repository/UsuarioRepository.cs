using FIAP.Data;
using FIAP.Models;
using FIAP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FIAP.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DataContext _dataContext;

        public UsuarioRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IList<UsuarioModel>> FindAllAsync()
        {
            return await _dataContext.Usuarios.AsNoTracking().ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = new UsuarioModel();
            usuario.UsuarioId = id;

            _dataContext.Usuarios.Remove(usuario);
            await _dataContext.SaveChangesAsync();
        }



        public async Task< UsuarioModel> FindByEmailAndSenhaAsync(string email, string senha)
        {
            var usuario = await _dataContext.Usuarios.AsNoTracking().FirstOrDefaultAsync(
                                 u => u.EmailUsuario == email &&
                                     u.Senha == senha
                );
            return usuario;
        }

        public async Task <UsuarioModel> FindByIdAsync(int id)
        {
            var usuario = await _dataContext.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.UsuarioId == id);

            return usuario;
        }

        public async Task <int> InsertAsync(UsuarioModel usuarioModel)
        {
            _dataContext.Usuarios.Add(usuarioModel);
            await _dataContext.SaveChangesAsync();

            return usuarioModel.UsuarioId;
        }


        public async Task UpdateAsync(UsuarioModel usuarioModel)
        {
            _dataContext.Usuarios.Update(usuarioModel);
           await _dataContext.SaveChangesAsync();
        }
    }
}
