using SistemaGerencialAcademia.Dominio.Entidades.Base;
using System;

namespace SistemaGerencialAcademia.Dominio.Entidades
{
    public class Endereco : Entititade
    {
        public Guid ClienteId { get; set; }
        public string Rua { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public Cliente Cliente { get; set; }
    }
}
