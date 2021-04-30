using System;
using System.IO;

public class FileLoader : IFileLoader
{
    public void RequestSomeData(string path, Action<string> onResponse)
    {
        using (var reader = new StreamReader(path))
        {
            onResponse?.Invoke(reader.ReadToEnd());
        }
    }
}

public class FileLoader2 : IFileLoader
{
    public void RequestSomeData(string path, Action<string> onResponse)
    {
        using (var reader = new StreamReader(path))
        {
            onResponse?.Invoke(nameof(FileLoader2) + ":\r\n" + reader.ReadToEnd());
        }
    }
}

public interface IFileLoader
{
    void RequestSomeData(string path, Action<string> onResponse);
}