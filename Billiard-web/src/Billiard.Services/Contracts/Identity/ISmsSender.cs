using System.Threading.Tasks;

namespace Billiard.Services.Contracts.Identity
{
    public interface ISmsSender
    {
        #region BaseClass

        Task SendSmsAsync(int TemplateId,string number, string message);

        #endregion

        #region CustomMethods

        #endregion
    }
}