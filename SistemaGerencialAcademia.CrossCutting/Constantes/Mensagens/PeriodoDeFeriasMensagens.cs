namespace SistemaGerencialAcademia.CrossCutting.Constantes.Mensagens
{
    public static class PeriodoDeFeriasMensagens
    {
        #region Mesnagens de sucesso

        public const string PERIODO_DE_FERIAS_SALVO = @"Período de ferias salvo com sucesso.";

        #endregion

        #region Mensagens de falha

        public const string DATA_PASSADA = @"A Data Início está no passado.";
        public const string DATAS_INVALIDAS = @"A Data Fim deve ser maior do que a Data Início.";
        public const string PERIODO_DE_FERIAS_INVALIDO = @"O período de férias não pode ser maior do que 30 dias.";
        public const string QUANDTIDADE_MAXIMA_DE_PERIODOS_DE_FERIAS = @"O cliente não pode tirar mais do que três períodos de férias no ano.";
        public const string QUANTIDADE_MAXIMA_DE_DIAS_DE_FERIAS = @"O cliente não pode tirar mais do que trinta dias de férias no ano.";
        public const string PERIODO_DE_FERIAS_FORA_DO_PERIODO_CORRENTE = @"O período de ferias precisa estar dentro do período corrente.";

        #endregion
    }
}
