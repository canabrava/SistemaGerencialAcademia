using System;

namespace SistemaGerencialAcademia.Models.Cliente
{
    public class ClienteModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Identidade { get; set; }
        public string Endereco { get; set; }
        public bool PodeTerPeriodoDeFerias { get; set; }
    }
}
