using SistemaGerencialAcademia.Dominio.DTOs.Base;
using System;

namespace SistemaGerencialAcademia.Dominio.DTOs.Cliente
{
    public class NovoClienteResponseDto : ResponseDto
    {
        public Guid ClienteId { get; set; }
    }
}
