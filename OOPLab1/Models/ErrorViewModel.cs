using System.Diagnostics.CodeAnalysis;

namespace OOPLab1.Models
{
    #region Attribues
    [ExcludeFromCodeCoverage]
    #endregion
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}