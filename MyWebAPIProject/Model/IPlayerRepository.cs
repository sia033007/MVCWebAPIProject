using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebAPIProject.Model
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> GetAllPlayers();
        Task<Player> GetPlayerById(int id);
        Task<Player> UpdatePlayer(Player player);
        Task<Player> AddPlayer(Player player);
        Task DeletePlayer(int id);
        Task<bool> SamePlayerExists(string playerName);
    }
}
