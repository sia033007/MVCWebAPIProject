using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWebAPIProject.Data;

namespace MyWebAPIProject.Model
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PlayerRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Player> AddPlayer(Player player)
        {
            await _applicationDbContext.Players.AddAsync(player);
            await _applicationDbContext.SaveChangesAsync();
            return player;
        }

        public async Task DeletePlayer(int id)
        {
            var playerToDelete = await _applicationDbContext.Players.FirstOrDefaultAsync(p => p.Id == id);
            _applicationDbContext.Players.Remove(playerToDelete);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Player>> GetAllPlayers()
        {
            return await _applicationDbContext.Players.ToListAsync();
        }

        public async Task<Player> GetPlayerById(int id)
        {
            return await _applicationDbContext.Players.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Player> UpdatePlayer(Player player)
        {
            _applicationDbContext.Players.Update(player);
            await _applicationDbContext.SaveChangesAsync();
            return player;
        }

        public async Task<bool> SamePlayerExists(string playerName)
        {
            return await _applicationDbContext.Players.AnyAsync(p => p.Name == playerName);
        }
    }
}
