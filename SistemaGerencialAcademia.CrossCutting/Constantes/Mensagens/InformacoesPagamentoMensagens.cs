namespace SistemaGerencialAcademia.CrossCutting.Constantes.Mensagens
{
    public static class InformacoesPagamentoMensagens
    {
        #region Mensagens de Sucesso

        public const string PAGAMENTO_SALVO = @"Pagamento salvo com sucesso.";

        #endregion

        #region Mensagens de Falha

        public const string PAGAMENTO_JA_REALIZADO = @"Não se pode fazer dois novos pagamentos no mesmo período corrente.";

        #endregion
    }
}
