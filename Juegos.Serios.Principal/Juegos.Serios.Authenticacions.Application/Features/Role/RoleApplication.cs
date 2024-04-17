using Juegos.Serios.Authenticacions.Domain.Entities.Rol;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol.Interfaces;

namespace Juegos.Serios.Authenticacions.Application.Features.Role
{
    public class RoleApplication
    {
        private readonly IRolService<RolEntity> _rolService;
        public RoleApplication(IRolService<RolEntity> rolService)
        {
            _rolService = rolService;
        }       
    }
}
