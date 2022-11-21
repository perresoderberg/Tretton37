using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Shared.Interfaces
{
    public interface IHTTPClientService
    {
        Task<string> GetHtmlPageAsync(string currentUrl);
        List<string> RetreiveHyperLinksFromHtml(string html);
    }
}
