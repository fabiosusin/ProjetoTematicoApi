namespace DTO.General.Base.Output
{

    public class BaseInfoOutput
    {
        public BaseInfoOutput() { }
        public BaseInfoOutput(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
