using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebSocketManager;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using Billiard.Entities.Identity;
using Billiard.Services.Contracts.Identity;
using Microsoft.Extensions.DependencyInjection;
using Billiard.Services.Identity;

namespace Billiard
{
    public class ChatMessageHandler : WebSocketHandler
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public ChatMessageHandler(ConnectionManager webSocketConnectionManager, IServiceScopeFactory serviceScopeFactory) : base(webSocketConnectionManager)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            // var socketId = WebSocketConnectionManager.GetId(socket);
            // await SendMessageToAllAsync($"{socketId.Split("_")[1]} is now connected");
        }


        public override async Task OnDisconnected(WebSocket socket)
        {
            // var socketId = WebSocketConnectionManager.GetId(socket);
            // await SendMessageToAllAsync($"{socketId.Split("_")[1]} is now Disconnected");
            await base.OnDisconnected(socket);

        }
        public class ClassMessage
        {

            public string Type { get; set; }
            public string Sender { get; set; }
            public string Receiver { get; set; }
            public string Content { get; set; }
            public bool? IsPrivate { get; set; }
            public string DateTime { get; set; } = DateTimeOffset.Now.ToLongPersianDateTimeString();
            public string UserDisplayName { get; set; }
            public string UserAvatar { get; set; }
        }
        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            var SenderUserId = int.Parse(socketId.Split("_")[1]);
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var msg = Newtonsoft.Json.JsonConvert.DeserializeObject<ClassMessage>(message);
            var _user = await GetUserAsync(SenderUserId);
            msg.UserDisplayName = _user.DisplayName;
            msg.UserAvatar = _user.PhotoFileName;
            msg.Sender = SenderUserId.ToString();
            var _message = Newtonsoft.Json.JsonConvert.SerializeObject(msg);

            await SendMessageToAllAsync(_message);
        }

        public async Task<User> GetUserAsync(int UserId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _userManager = scope.ServiceProvider.GetService<ApplicationUserManager>();
                var user = await _userManager.FindByIdIncludeUserRolesAsync(UserId);
                return user;
            }
        }
    }
}