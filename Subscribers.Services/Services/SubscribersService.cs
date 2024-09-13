using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Subscribers.Models.Subscribers;
using Subscribers.Repositories.Entities;
using Subscribers.Repositories.Repositories;
using Subscribers.Services.Services.Interfaces;
using FluentValidation;
using ValidationException = FluentValidation.ValidationException;

namespace Subscribers.Services.Services;
public class SubscribersService : ISubscribersService
{
    private readonly IBaseRepository<Subscriber> _subscriberRepository;
    private readonly IBaseCsvReaderService<SubscriberModel> _baseCsvReaderService;
    private readonly IMapper _mapper;
    private readonly IValidator<SubscriberModel> _validator;

    public SubscribersService(
        IBaseRepository<Subscriber> subscriberRepository,
        IBaseCsvReaderService<SubscriberModel> baseCsvReaderService,
        IMapper mapper,
        IValidator<SubscriberModel> validator)
    {
        _subscriberRepository = subscriberRepository;
        _baseCsvReaderService = baseCsvReaderService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task ImportSubscribers(IFormFile subscribersCsv)
    {
        var readSubscribers = _baseCsvReaderService.ReadCsvFile(subscribersCsv);

        try
        {
            foreach (var subscriber in readSubscribers)
            {
                var validationResult = await _validator.ValidateAsync(subscriber);
                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors);

                await Put(subscriber);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to import subscribers.", ex);
        }
    }

    public async Task<List<SubscriberModel>> Search(SubscriberSearch search)
    {
        var query = _subscriberRepository.GetAll();

        if (!string.IsNullOrEmpty(search.Email))
            query = query.Where(subscriber => subscriber.Email == search.Email);

        if (search.ExpirationDateFrom.HasValue)
            query = query.Where(subscriber => subscriber.ExpirationDate >= search.ExpirationDateFrom);

        if (search.ExpirationDateTo.HasValue)
            query = query.Where(subscriber => subscriber.ExpirationDate <= search.ExpirationDateTo);

        var subscribers = await query.ToListAsync();

        return _mapper.Map<List<SubscriberModel>>(subscribers);
    }

    private async Task<SubscriberModel> Put(SubscriberModel model)
    {
        var subscriber = await _subscriberRepository.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email);

        subscriber = subscriber == null
            ? await _subscriberRepository.Insert(_mapper.Map<Subscriber>(model))
            : await _subscriberRepository.Update(_mapper.Map(model, subscriber));

        return _mapper.Map<SubscriberModel>(subscriber);
    }
}
