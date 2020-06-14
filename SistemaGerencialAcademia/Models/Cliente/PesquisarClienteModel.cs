using SistemaGerencialAcademia.Models.Base;
using System.Collections.Generic;
using System.ComponentModel;

namespace SistemaGerencialAcademia.Models.Cliente
{
    public class PesquisarClienteModel
    {
        [DisplayName("Nome:")]
        public string Nome { get; set; }
        [DisplayName("CPF:")]
        public string CPF { get; set; }
        public bool PesquisaRealizada { get; set; }
        public IEnumerable<ClienteModel> Clientes { get; set; }

    }
}
