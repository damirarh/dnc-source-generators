using System;

namespace BuildInfo.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Timestamp: {Build.Info.Timestamp}");
            Console.WriteLine($"Git SHA: {Build.Info.GitSha}");
        }
    }
}
