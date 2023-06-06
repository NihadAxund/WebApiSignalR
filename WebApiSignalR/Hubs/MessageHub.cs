using Microsoft.AspNetCore.SignalR;

namespace WebApiSignalR.Hubs
{
    public class MessageHub:Hub
    {
        private static Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>()
        {       
            { "Chevrolet", new List<string>()},
            { "Mercedes", new List<string>()}
        
        };
        private const int MaxUser = 3;

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var disconId = Context.ConnectionId;
            Parallel.ForEach(groups.Values, groupList =>
            {
                lock (groupList)
                {
                    
                    if (groupList.Contains(disconId))
                    {
                        groupList.Remove(disconId);
                        return;
                    }
                }
               
            });

            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage",message+"'s Offer : ",FileHelper.Read());
        }
        public async Task ChatSend(string room,string Message,string user)
        {
            await Clients.OthersInGroup(room).SendAsync("ChatMessage", Message,user);
        }

        public async Task SendWinnerMessage(string message)
        {
            await Clients.Others.SendAsync("ReceiveInfo", message, FileHelper.Read());
        }

    
        public async Task<double> JoinRoom(string room,string user)
        {
            //if (!groups.ContainsKey(room))
            //    groups[room] = new List<string>();

            if (groups[room].Count >= MaxUser)
            {
                Context.Abort();
                return -1;
            }
            else
            {
                groups[room].Add(Context.ConnectionId);
                await Groups.AddToGroupAsync(Context.ConnectionId, room);
                var retuntValue = FileHelper.Read(room);
                await Clients.OthersInGroup(room).SendAsync("ReceiveJoinInfo", user);
                return retuntValue;
            }


        }

        public async Task< List<GroupType>> GetRooms() {
            List<GroupType> list = new();
            foreach (var data in groups)
            {
                list.Add(new GroupType(data.Key, data.Value.Count,FileHelper.Read(data.Key)));
            }
            return list;
        
        }

        public async Task SendMessageRoom(string room,string message)
        {
            await Clients.OthersInGroup(room).SendAsync("ReceiveMessageRoom", message + "'s Offer : ", FileHelper.Read(room));
        }

        public async Task SendWinnerMessageRoom(string room,string message)
        {
            await Clients.OthersInGroup(room).SendAsync("ReceiveInfoRoom", message, FileHelper.Read(room));
        }
    }
}
