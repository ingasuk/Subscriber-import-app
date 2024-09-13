using AutoMapper;
using Subscribers.Contracts.Subscribers;
using Subscribers.Models.Subscribers;
using Subscribers.Repositories.Entities;

namespace Subscribers.WebApi.Profiles;

public class SubscriberProfile : Profile
{
    public SubscriberProfile()
    {
        CreateMap<SubscriberSearchViewModel, SubscriberSearch>();

        CreateMap<SubscriberModel, Subscriber>().ReverseMap();

        CreateMap<SubscriberModel, SubscriberViewModel>();
    }
}
