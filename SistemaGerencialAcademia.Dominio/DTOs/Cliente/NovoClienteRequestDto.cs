namespace SistemaGerencialAcademia.Dominio.DTOs.Cliente
{
    public class NovoClienteRequestDto
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Identidade { get; set; }
        public string Rua { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
    }
}
