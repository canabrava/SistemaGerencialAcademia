using System.Text.RegularExpressions;

namespace SistemaGerencialAcademia.CrossCutting.Util
{
    public static class OperacoesString
    {
        public static string RemoverDigitosNaoNumericos(this string campoNumerico)
        {
            if (string.IsNullOrEmpty(campoNumerico))
                return string.Empty;

            Regex regexObj = new Regex(@"[^\d]");
            return regexObj.Replace(campoNumerico, "");
        }
    }
}
