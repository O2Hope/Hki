using Microsoft.AspNetCore.SignalR;

namespace hki.web.Hubs
{
    public class NotificacionesHub : Hub
    {

        
        public void Notificaciones(bool notificaciones)
        {
            //Clients.All.clientListener($"notificaciones");
           
        }
        
    }
}