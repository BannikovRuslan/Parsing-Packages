
namespace ParsingPackages.Persistence.AzureDevOps
{
    public class AzureDevOpsItem
    {
        public string objectId { get; set; }
        public string path { get; set; }
        public AzureDevOpsItem(string objectId, string path) 
        {
            this.objectId = objectId;
            this.path = path;
        }
    }
}
