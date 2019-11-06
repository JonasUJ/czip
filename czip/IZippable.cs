namespace czip
{
    public interface IZippable
    {
        string Name { get; set; }
        SerializedData Serialize();
    }
}
