using Nedelko.Polyclinic.ViewModels;
using System.Threading.Tasks;

namespace Nedelko.Polyclinic.Interfaces
{
    public interface ISmsManager
    {
        Task<SmsViewModel> SendAsync(string message, string phone);
    }
}
