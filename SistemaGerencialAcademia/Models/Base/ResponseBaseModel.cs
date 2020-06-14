using System.Collections.Generic;

namespace SistemaGerencialAcademia.Models.Base
{
    public class ResponseBaseModel
    {
        public bool Sucesso { get; set; }
        public List<string> Mensagens { get; set; }

        public ResponseBaseModel()
        {
            Sucesso = true;
            Mensagens = new List<string>();
        }
    }
}
