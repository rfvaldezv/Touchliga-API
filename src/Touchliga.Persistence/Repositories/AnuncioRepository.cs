using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

public sealed class AnuncioRepository : GenericRepository<Anuncio>, IAnuncioRepository
{
    public AnuncioRepository(TouchligaDbContext context) : base(context)
    {
    }
}
