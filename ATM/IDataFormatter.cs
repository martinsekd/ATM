namespace ATM.System
{
    public interface IDataFormatter
    {
        TransponderData StringToTransponderData(string s);
    }
}