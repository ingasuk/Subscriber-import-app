using Microsoft.AspNetCore.Http;
using Subscribers.Models.Subscribers;

namespace Subscribers.Services.Services.Interfaces;
public interface ISubscribersService
{
    Task ImportSubscribers(IFormFile subscribersCsv);
    Task<List<SubscriberModel>> Search(SubscriberSearch search);
}