using AutoMapper;
using Entities.Concrete;
using Entities.DTOs;

namespace Entities.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddPostDto, Post>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.PostTitle))
                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.PostContext))
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.CoverImageUrl))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
        CreateMap<Post, AddPostDto>()
            .ForMember(dest => dest.PostTitle, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.PostContext, opt => opt.MapFrom(src => src.Context))
            .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.CoverImageUrl))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

        CreateMap<UpdatePostDto, Post>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PostId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.PostTitle))
                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.PostContext))
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.CoverImageUrl));
        //.ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => DateTime.Now)) 
        /*  .ForMember(dest => dest.CreateDate, opt => opt.Ignore())  
          .ForMember(dest => dest.UserId, opt => opt.Ignore())      
          .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())*/
        CreateMap<CommentDto, Comment>()
            .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
            .ForMember(dest => dest.CommentText, opt => opt.MapFrom(src => src.CommentText));
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
            .ForMember(dest => dest.CommentText, opt => opt.MapFrom(src => src.CommentText));
        CreateMap<UpdateCommentDto, Comment>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CommentId))
            .ForMember(dest => dest.CommentText, opt => opt.MapFrom(src => src.CommentText));
    }
}
