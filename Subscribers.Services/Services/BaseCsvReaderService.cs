using CsvHelper;
using CsvHelper.TypeConversion;
using Microsoft.AspNetCore.Http;
using Subscribers.Services.Services.Interfaces;
using System.Globalization;
using CsvHelper.Configuration;

namespace Subscribers.Services.Services;
public class BaseCsvReaderService<TModel> : IBaseCsvReaderService<TModel> where TModel : class
{
    public BaseCsvReaderService()
    {
    }

    public List<TModel> ReadCsvFile(IFormFile csvFile)
    {
        try
        {
            if (!csvFile.FileName.EndsWith(".csv"))
                throw new Exception("File format should be CSV.");

            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                DetectDelimiter = true,
            };
            using var csvReader = new CsvReader(new StreamReader(csvFile.OpenReadStream()), csvConfiguration, leaveOpen: true);

            csvReader.Context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add(string.Empty);
            var subscribers = csvReader.GetRecords<TModel>().ToList();

            return subscribers;
        }
        catch (HeaderValidationException ex)
        {
            throw new ApplicationException("CSV file header is invalid.", ex);
        }
        catch (TypeConverterException ex)
        {
            throw new ApplicationException("CSV file contains invalid data format.", ex);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error reading CSV file", ex);
        }
    }
}
