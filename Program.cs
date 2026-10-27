
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// CWE-22: Path Traversal
app.MapGet("/read", (string fileName) =>
{
    string path = Path.Combine(@"C:\Temp\Reports", fileName);

    string content = File.ReadAllText(path);

    return Results.Ok(content);
});

// CWE-78: OS Command Injection
app.MapGet("/ping", (string host) =>
{
    var process = new Process();

    process.StartInfo.FileName = "cmd.exe";
    process.StartInfo.Arguments = "/c ping " + host;
    process.StartInfo.UseShellExecute = false;

    process.Start();

    return Results.Ok("Command executed");
});

// CWE-404: Resource Leak
app.MapGet("/file", (string fileName) =>
{
    string path = Path.Combine(@"C:\Temp\Reports", fileName);

    FileStream stream = File.OpenRead(path);

    byte[] buffer = new byte[1024];

    stream.Read(buffer, 0, buffer.Length);

    return Results.Ok("File processed");
});

// CWE-476: Null Dereference
app.MapGet("/user", (bool isValid) =>
{
    string? username = null;

    if (isValid)
    {
        username = "admin";
    }

    int length = username.Length;

    return Results.Ok(length);
});

app.Run();
