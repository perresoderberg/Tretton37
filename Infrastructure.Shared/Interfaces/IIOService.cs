using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Shared.Interfaces
{
    public interface IIOService
    {
        Task ClearDirectory();
        Task StoreHtmlPageInFilePathAsync(string url, string htmlPage);
    }
}
