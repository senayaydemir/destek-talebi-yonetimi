using Microsoft.AspNetCore.SignalR;

namespace DestekTalebiYonetimi.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            if (httpContext != null)
            {
                var rol = httpContext.Session.GetString("Rol");
                var kullanici = httpContext.Session.GetString("KullaniciAdi");

                if (!string.IsNullOrEmpty(rol))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, rol);
                }

                if (!string.IsNullOrEmpty(kullanici))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"USER_{kullanici}");
                }
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();

            if (httpContext != null)
            {
                var rol = httpContext.Session.GetString("Rol");
                var kullanici = httpContext.Session.GetString("KullaniciAdi");

                if (!string.IsNullOrEmpty(rol))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, rol);
                }

                if (!string.IsNullOrEmpty(kullanici))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"USER_{kullanici}");
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}