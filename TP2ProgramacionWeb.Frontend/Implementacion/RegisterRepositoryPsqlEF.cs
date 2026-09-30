using TP2ProgramacionWeb.Frontend.Data;
using TP2ProgramacionWeb.Frontend.Models;
using TP2ProgramacionWeb.Frontend.Repositories;

namespace TP2ProgramacionWeb.Frontend.Implementacion
{
    public class RegisterRepositoryPsqlEF : IRegisterRepository
    {
        private readonly DataContext _dataContext;
        public RegisterRepositoryPsqlEF(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public async Task Registrarse (Usuario usuario)
        {
            _dataContext.Usuarios.Add(usuario);

            await _dataContext.SaveChangesAsync();
        }
    }
}
