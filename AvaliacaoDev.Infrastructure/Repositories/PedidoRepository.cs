using AvaliacaoDev.Domain.Entities;
using AvaliacaoDev.Domain.Enums;
using AvaliacaoDev.Domain.Interfaces;
using AvaliacaoDev.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AvaliacaoDev.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<Pedido> GetById(Guid id)
        {
            return await _context.Pedidos
                .Include(p => p.Produtos)
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Pedido>> GetAll(int page, int pageSize, string status)
        {
            var query = _context.Pedidos
                .Include(p => p.Produtos)
                .Include(p => p.Cliente)
                .AsQueryable();

            // Filtro opcional
            if (!string.IsNullOrEmpty(status))
            {
                try
                {
                    var parsed = Enum.Parse<StatusPedido>(status, true);
                    query = query.Where(p => p.Status == parsed);
                }
                catch
                {
                    // Se status for inválido, retorna lista vazia
                    return new List<Pedido>();
                }
            }

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}