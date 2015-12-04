using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalRChat.Startup))]

namespace SignalRChat
{
    public class Startup
    {
        //Cuando arrancamos el proyecto se ejecuta SignalR automaticamente.
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
