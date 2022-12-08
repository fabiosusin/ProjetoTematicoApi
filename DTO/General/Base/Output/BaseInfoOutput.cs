namespace DTO.General.Base.Output
{

    public class BaseInfoOutput
    {
        public BaseInfoOutput() { }
        public BaseInfoOutput(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
