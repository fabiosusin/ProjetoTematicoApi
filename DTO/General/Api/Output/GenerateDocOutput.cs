namespace DTO.General.Api.Output
{
    public class GenerateDocOutput
    {
        public GenerateDocOutput(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public string Name { get; set; }
        public string Path { get; set; }
    }
}
