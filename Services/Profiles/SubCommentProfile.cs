using System.Linq;
using AutoMapper;
using DAL.Entities.Comments;
using Model.SubComment.Inputs;
using Model.SubComment.Outputs;

namespace Services.Profiles
{
    public class SubCommentProfile : Profile
    {
        public SubCommentProfile()
        {
            CreateMap<SubCommentCreateInput, SubComment>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.CommentText))
                .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.BaseCommentId));
            CreateMap<SubCommentUpdateInput, SubComment>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.CommentText))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<SubComment, SubCommentOutput>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CommentText, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.DateComment)) 
                .ForMember(dest => dest.IsEdited, opt => opt.MapFrom(src => src.IsUpdated))
                .ForMember(dest => dest.UserDisplayName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserPictureUrl, opt => opt.MapFrom(src => src.User.PictureUrl));
        }
    }
}