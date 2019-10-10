namespace ATM
{
    public interface IDataFormatter
    {
        TransponderData StringToTransponderData(string s);
    }
}