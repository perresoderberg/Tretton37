using Core.Domain;
using Infrastructure.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Shared
{
    public class IOService : IIOService
    {
        private string startDirForFolderCreation;
        private readonly ILogger<IOService> _logger;

        public IOService(ILogger<IOService> logger, string startDir)
        {
            startDirForFolderCreation = startDir;
            _logger = logger;

        }

        public async Task ClearDirectory() 
        {
            if (!string.IsNullOrWhiteSpace(startDirForFolderCreation)) { 
                if (Directory.Exists(startDirForFolderCreation))
                    Directory.Delete(startDirForFolderCreation, true);
            }

        }

        public async Task StoreHtmlPageInFilePathAsync(string url, string htmlPage)
        {
            StreamWriter writer = null;

            try
            {
                if (string.IsNullOrWhiteSpace(url))
                    return;

                // url = url.Replace("#", "/");
                string newPath = startDirForFolderCreation + url.Replace("#", "/");

                if (!Directory.Exists(newPath))
                    Directory.CreateDirectory(newPath);

                string fileName = GetFileName(newPath);
                if (fileName == null)
                    return;

                var completeFilename = $"{newPath}/{fileName}.html";
               
                _logger.LogTrace($"Write file name: {fileName} to path: {newPath}");

                using (writer = new StreamWriter(completeFilename))
                {
                    writer.WriteLine(htmlPage);
                }

            }
            catch (IOException e)
            {
                _logger.LogError("StoreHtmlPageInFilePathAsync in GetHtmlPageAsync");
                Debugger.Break();
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError("StoreHtmlPageInFilePathAsync in GetHtmlPageAsync");
                Debugger.Break();
                throw;
            }
            finally
            {
                if(writer != null)
                    writer.Dispose();
            }


        }



        private string GetFileName(string path)
        {
            var pathSplit = path.Split(new[] { '#', '/' });
            if (pathSplit.Length < 1)
                return null;

            var fileName = pathSplit[pathSplit.Length - 1];
            return fileName;
        }
    }
}
