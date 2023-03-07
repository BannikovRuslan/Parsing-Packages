
namespace ParsingPackages.Persistence.AzureDevOps
{
    public class AzureDevOpsRepository
    {
        public List<AzureDevOpsItem> items { set; get; } = new List<AzureDevOpsItem>();
        public string repositoryId { set; get; }
        public AzureDevOpsRepository(string repositoryId) 
        {
            this.repositoryId = repositoryId;
        }
        public AzureDevOpsRepository(string repositoryId, List<AzureDevOpsItem> items) 
        {
            this.repositoryId = repositoryId;
            this.items = items;
        }
    }
}
