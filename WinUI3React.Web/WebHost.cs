using MimeTypes;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace WinUI3React.Web;

public class WebHost
{
    const int DEFAULT_PORT = 8728;
    static readonly string[] HOSTS = new[] {
        "http://localhost",
        "http://127.0.0.1",
    };

    private HttpListener _server;
    private int _port;
    private string _baseDir;
    private bool _isStopping;

    public WebHost(int port = DEFAULT_PORT)
    {
        _port = port;
        _baseDir = Path.Combine(AppContext.BaseDirectory, "webapp");
    }
    public void Start()
    {
        _server = new HttpListener();
        foreach (var host in HOSTS)
        {
            _server.Prefixes.Add($"{host}:{_port}/");
        }
        _isStopping = false;
        _server.Start();
        Receive();
    }

    public void Stop()
    {
        _isStopping = true;
        _server?.Stop();
        _server = null;
    }

    private void Receive()
    {
        _server?.BeginGetContext(HandleRequest, null);
    }
    private void HandleRequest(IAsyncResult result)
    {
        if (_server is null) return;
        if (!_server.IsListening) return;
        if (_isStopping) return;

        var ctx = _server.EndGetContext(result);
        var localPath = ctx.Request.Url?.LocalPath.TrimStart('/') ?? string.Empty;
        var filePath = GetResourcePath(localPath);
        if (string.IsNullOrEmpty(localPath) || !File.Exists(filePath))
        {
            filePath = GetResourcePath("index.html");
        }

        var resp = ctx.Response;
        if (File.Exists(filePath))
        {
            var mimeType = MimeTypeMap.GetMimeType(Path.GetExtension(filePath).TrimStart('.'));
            resp.Headers.Set("Content-Type", mimeType);
            var buf = File.ReadAllBytes(filePath);
            resp.ContentLength64 = buf.Length;
            resp.OutputStream.Write(buf);
        }
        else
        {
            resp.StatusCode = (int)HttpStatusCode.NotFound;
            resp.Headers.Set("Content-Type", "text/plain");
            resp.OutputStream.Write(Encoding.UTF8.GetBytes("Resource not found."));
        }
        resp.Close();

        Receive();
    }
    private string GetResourcePath(string relPath)
    {
        return Path.Combine(_baseDir, relPath);
    }
}