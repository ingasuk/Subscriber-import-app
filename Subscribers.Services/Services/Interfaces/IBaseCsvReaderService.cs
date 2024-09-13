using Microsoft.AspNetCore.Http;

namespace Subscribers.Services.Services.Interfaces;
public interface IBaseCsvReaderService<TModel> where TModel : class
{
    List<TModel> ReadCsvFile(IFormFile csvFile);
}