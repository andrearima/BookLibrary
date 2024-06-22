using BookLibrary.Royal.Setup;

await WebApplication.CreateBuilder(args)
    .Configure()
    .RunAsync();

// Make it testable
public partial class Program { }
