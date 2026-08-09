using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

public sealed class PatrocinadorRepository : GenericRepository<Patrocinador>, IPatrocinadorRepository
{
    public PatrocinadorRepository(TouchligaDbContext context) : base(context)
    {
    }
}
