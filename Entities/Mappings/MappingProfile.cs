using AutoMapper;
using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using Newtonsoft.Json.Serialization;

namespace Entities.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //User
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        
        //Post
        CreateMap<AddPostDto, Post>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.Context));
        CreateMap<Post, AddPostDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.Context));
        CreateMap<UpdatePostDto, Post>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PostId));
        //Comment
        CreateMap<CommentDto, Comment>()
            .ForMember(dest => dest.CreateDate, opt=> opt.MapFrom(src => src.CommentTime));
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.CommentTime, opt => opt.MapFrom(src => src.CreateDate));
        CreateMap<UpdateCommentDto, Comment>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CommentId));
        CreateMap<GetCommentDto, Comment>()
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CommentTime));
        CreateMap<Comment, GetCommentDto>()
            .ForMember(dest => dest.CommentTime, opt => opt.MapFrom(src => src.CreateDate));
   
    }
}
