using AutoMapper;
using DAL.Entities.Messages;
using Model.Message.Inputs;
using Model.Message.Outputs;

namespace Services.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageInput, Message>();
            CreateMap<UpdateMessageInput, Message>();
            CreateMap<Message, MessageOutput>();
        }
    }
}