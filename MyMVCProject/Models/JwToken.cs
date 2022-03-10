using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMVCProject.Models
{
    public class JwToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("expiresAt")]
        public DateTime ExpireAt { get; set; }
    }
}
