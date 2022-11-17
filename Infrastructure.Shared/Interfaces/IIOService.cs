using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Shared.Interfaces
{
    public interface IIOService
    {

        Task CreateFilePath(string filePath);

    }
}
