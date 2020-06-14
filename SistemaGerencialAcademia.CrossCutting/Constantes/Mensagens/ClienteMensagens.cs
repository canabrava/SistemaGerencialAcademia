namespace SistemaGerencialAcademia.CrossCutting.Constantes.Mensagens
{
    public static class ClienteMensagens
    {
        #region Mensagens de falha

        public const string DOCUMENTO_JA_CADASTRADO = @"Documento já cadastrado.";
        public const string CLIENTE_NAO_ENCONTRADO = @"Cliente não encontrado.";
        public const string CPF_INVALIDO = @"CPF inserido é inválido.";
        public const string APENAS_VALORES_NUMERICOS = @"O campo Número só aceita valores numéricos.";
        public const string CAMPO_CPF_OBRIGATORIO = @"'CPF' deve ser informado.";
        public const string CAMPO_NUMERO_OBRIGADORIO = @"'Número' deve ser informado.";

        #endregion

        #region Mensagens de sucesso

        public const string CLIENTE_CADASTRADO = @"Cliente cadastrado com sucesso.";
        public const string PLANO_DE_PAGAMAENTO_ALTERADO = @"Plano de pagamento alterado com sucesso.";

        #endregion

    }
}
