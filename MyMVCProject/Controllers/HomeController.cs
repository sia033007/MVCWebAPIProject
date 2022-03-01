using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyMVCProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyMVCProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllPlayers()
        {
            List<Player> players = new List<Player>();
            //var client = _clientFactory.CreateClient("MyClient");
            //players = await client.GetFromJsonAsync<List<Player>>("player");
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:42045/api/player");
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var apiString = await response.Content.ReadAsStringAsync();
                players = JsonConvert.DeserializeObject<List<Player>>(apiString);
            }
            return View(players);
        }
        public ViewResult GetPlayer() => View();
        [HttpPost]
        public async Task<IActionResult> GetPlayer(int id)
        {
            Player player = new Player();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:42045/api/player/" + id);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var apiString = await response.Content.ReadAsStringAsync();
                player = JsonConvert.DeserializeObject<Player>(apiString);
                TempData["success"] = "Got Player!";
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["notfound"] = "Not Found Such a Player";
            }
            return View(player);
        }
        public ViewResult AddPlayer() => View();
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPlayer(Player player)
        {
            //var client = _clientFactory.CreateClient("MyClient");
            //var response = await client.PostAsJsonAsync("player", player);
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:42045/api/player/");
            if(player != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(player),
                    System.Text.Encoding.UTF8, "application/json");
            }
            else
            {
                return BadRequest();
            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if(response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var apiString = await response.Content.ReadAsStringAsync();
                player = JsonConvert.DeserializeObject<Player>(apiString);
                TempData["success"] = "Player Added Successfully!";
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["badrequest"] = "Player with the same name already exists";
            }
            return View(player);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "http://localhost:42045/api/player/" + id);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                TempData["success"] = "Player Deleted Successfully!";
                return RedirectToAction("GetAllPlayers");
                
            }
            else
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> UpdatePlayer(int id)
        {
            Player player = new Player();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:42045/api/player/" + id);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var apiString = await response.Content.ReadAsStringAsync();
                player = JsonConvert.DeserializeObject<Player>(apiString);
            }
            return View(player);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdatePlayer(Player player)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, "http://localhost:42045/api/player/" + player.Id);
            if(player != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(player),
                    System.Text.Encoding.UTF8, "application/json");

            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var apiString = await response.Content.ReadAsStringAsync();
                player = JsonConvert.DeserializeObject<Player>(apiString);
                TempData["success"] = "Player Updated Successfully!";
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["badrequest"] = "Player with the same name already exists";
                return View(player);
            }
            return View(player);
        }
        public async Task<IActionResult> GetPlayerByPosition(string positionName)
        {
            List<Player> players = new List<Player>();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:42045/api/player/" + positionName);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request , HttpCompletionOption.ResponseHeadersRead);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var apiString = await response.Content.ReadAsStringAsync();
                players = JsonConvert.DeserializeObject<List<Player>>(apiString);
            }
            return View(players);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult LoginRequired()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
