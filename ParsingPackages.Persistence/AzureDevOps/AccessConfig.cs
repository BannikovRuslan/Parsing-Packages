
namespace ParsingPackages.Persistence.AzureDevOps
{
    public class AccessConfig
    {
        public string userName { get; set; }
        public string accessToken { get; set; }
        public string server { get; set; }
        public string collection { get; set; }

        public AccessConfig()
        {
            userName = "";
            accessToken = "";
            server = "";
            collection = "";
        }
        public AccessConfig(string fileName) 
        {
            string[] file = getAccessConfigFromFile(fileName);
            userName = file[0].Trim();
            accessToken = file[1].Trim();
            server = file[2].Trim();
            collection = file[3].Trim();
        }

        private string[] getAccessConfigFromFile(string fileName)
        {
            return File.ReadAllLines(fileName);
        }
    }
}
