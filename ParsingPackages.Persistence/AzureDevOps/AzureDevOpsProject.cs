
namespace ParsingPackages.Persistence.AzureDevOps
{
    public class AzureDevOpsProject
    {
        public List<AzureDevOpsRepository> repositories { set; get; } = new List<AzureDevOpsRepository>();
        public string projectId { set; get; }
        public AzureDevOpsProject(string projectId)
        {
            this.projectId = projectId;
        }
        public AzureDevOpsProject(string projectId, List<AzureDevOpsRepository> repositories) 
        { 
            this.projectId = projectId;
            this.repositories = repositories;
        }
    }
}
