var chatHub;
var miNombre;

$(document).ready(function () {
    //hubChat es el nombre de la clase que hemos creado en el servidor
    chatHub = $.connection.hubChat;
    registrarEventos();

    $.connection.hub.start().done(function () {
        bootbox.prompt("¿Como te llamas?", function (res) {
            if (res != null) {
                miNombre = res;
                chatHub.server.conectar(miNombre);
                registrarLlamadas();
            }
        });
    });
});

function registrarLlamadas() {
    $("#btnEnv").click(function () {
        var txt = $("#txtMen").val();
        chatHub.server.enviarMensaje(miNombre, txt);
    });
}

function registrarEventos() {
    chatHub.client.onConnected = function (id, nombre, usuarios, mensajes) {
        $.each(usuarios, function (key, obj) {
            var elem = "<li id='us-" + obj.Id + "'>" + obj.Nombre + "</li>";
            $("#conectados").append(elem);
        });
        $.each(mensajes, function (key, obj) {
            var elem = "<p>" + obj.Usuario + " dice " + obj.Contenido + "</p>";
            $("#mensajes").append(elem);
        });
    }

    chatHub.client.onNewUserConnected = function (id, nombre) {
        var elem = "<li id='us-" + id + "'>" + nombre + "</li>";
        $("#conectados").append(elem);
        $("#mensajes").append("<p>Se ha conectado " + nombre + "</p>");
    };

    chatHub.client.enviadoMensaje = function (usuario, mensaje) {
        var elem = "<p>" + usuario + " dice " + mensaje + "</p>";
        $("#mensajes").append(elem);
    };

    chatHub.client.usuarioDesconectado = function (id, nombre) {
        var item = $("#us-" + id);
        $("#conectados").remove(item);
        var elem = "<p>" + nombre + " se ha desconectado</p>";
        $("#mensajes").append(elem);
    };
}