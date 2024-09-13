using Microsoft.AspNetCore.Http;

namespace Subscribers.Contracts.Subscribers;
public class FileImportViewModel
{
    public IFormFile FormFile { get; set; }
}
