using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Shared.Interfaces
{
    public interface IHTTPClientService
    {
        Task<List<string>> FetchURLForHyperLinks(string url);
    }
}
