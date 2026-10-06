using TP2ProgramacionWeb.Frontend.Models;

namespace TP2ProgramacionWeb.Frontend.Utilidades
{
    public static class Roles
    {
        public const string Administrador = "Administrador";
        public const string Usuario = "Usuario";

        public static string De(Usuario usuario) => usuario.EsAdministrador ? Administrador : Usuario;
    }
}
