using System.Net.Http.Json;
using Layout_Client.Model.DTO;
using Microsoft.AspNetCore.Components.Forms;

public class UploadService
{
    private readonly HttpClient _http;
    public UploadService(HttpClient http) => _http = http;

    public async Task<string?> UploadImageAsync(IBrowserFile file)
    {
        var content = new MultipartFormDataContent();
        var stream = file.OpenReadStream(10_000_000);
        content.Add(new StreamContent(stream)
        {
            Headers = { ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType) }
        }, "file", file.Name);

        var response = await _http.PostAsync("api/Upload", content);
        if (!response.IsSuccessStatusCode) return null;
        var result = await response.Content.ReadFromJsonAsync<UploadResponse>();
        return "http://localhost:5176" + result?.FilePath;
    }
}
