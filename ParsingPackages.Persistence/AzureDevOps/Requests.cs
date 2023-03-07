using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ParsingPackages.Persistence.AzureDevOps
{
    public class Requests
    {
        private AccessConfig accessConfig;
        public Requests(AccessConfig accessConfig) 
        {
            this.accessConfig = accessConfig;
        }
        public async Task<string> GetProjects()
        {
            string apiString = $"/_apis/projects";
            return await new Request(accessConfig).Send(apiString);
        }

        public async Task<string> GetRepositories(string projectName)
        {
            string apiString = $"/{projectName}/_apis/git/repositories";
            return await new Request(accessConfig).Send(apiString);
        }

        public async Task<string> GetItems(string projectName, string repositoryId)
        {
            const string recursionLevel = "Full";
            string apiString = $"/{projectName}/_apis/git/repositories/{repositoryId}/items?recursionLevel={recursionLevel}";
            return await new Request(accessConfig).Send(apiString);
        }
        public async Task<string> GetFile(string projectName, string repositoryId, string objectId)
        {
            const string isDownload = "true";
            string apiString = $"/{projectName}/_apis/git/repositories/{repositoryId}/blobs/{objectId}?download={isDownload}";
            string resp = await new Request(accessConfig).Send(apiString);
            return resp;
        }
    }
}
