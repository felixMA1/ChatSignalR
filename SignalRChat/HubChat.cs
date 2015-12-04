using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRChat.Modelo;

namespace SignalRChat
{
    //Hub: clase concentradora reune la informacion de todas las personas que estan conectadas
    public class HubChat : Hub
    {
        public List<Usuario> Usuarios = new List<Usuario>();
        public List<Mensaje> Mensajes = new List<Mensaje>();

        /*
        Saber que usuarios estan conectados o no.
        Al metodo Conectar se le llama desde el cliente.
        Automaticamente guarda a un usuario dentro de la lista de usuarios conectados.
        Decirle a todos los usuarios menos a el mismo que fulanito se a conectado al chat
        */
        public void Conectar(String nombre)
        {
            var usuario = new Usuario()
            {
                //Context: Contexto de ejecucion del server
                Id = Context.ConnectionId,
                Nombre = nombre
            };
            if (Usuarios.All(o => o.Id != usuario.Id))
            {
                Usuarios.Add(usuario);
                //Clientes.Caller : onConnected es un nombre que podria ser Federico. Igual que onNewUserConnected.
                Clients.Caller.onConnected(usuario.Id, nombre, Usuarios, Mensajes);
                Clients.AllExcept(usuario.Id).onNewUserConnected(usuario.Id, nombre);
            }
        }

        public void EnviarMensaje(String usuario, String mensaje)
        {
            Mensajes.Add(new Mensaje() {Contenido = mensaje,Usuario = usuario});
            if (Mensajes.Count > 30)
            {
                Mensajes.RemoveAt(0);
            }
            Clients.All.enviadoMensaje(usuario, mensaje);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var item = Usuarios.FirstOrDefault(o => o.Id == Context.ConnectionId);
            if (item != null)
            {
                Usuarios.Remove(item);
                Clients.All.usuarioDesconectado(item.Id, item.Nombre);
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}