using AutoMapper;
using Business.Abstract;
using Business.BusinessAspect.Autofac.Secured;
using Business.Validation.FluentValidation;
using Core.Aspects.Autofac.Validation.FluentValidation;
using Core.Extensions;
using Core.Helpers.Results.Abstract;
using Core.Helpers.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System.Drawing.Printing;
using System.Security.Claims;
using IResult = Core.Helpers.Results.Abstract.IResult;

namespace Business.Concrete
{
    public class PostManager : IPostService
    {
        public PostManager(IPostDal postDal, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUserService userService, ICommentService commentService)
        {
            _postDal = postDal;
            _httpContextAccessor = httpContextAccessor;
            _commentService = commentService;
            _mapper = mapper;
            _userService = userService;
        }
        private readonly IPostDal _postDal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;

        [SecuredOperation("User")]
        [ValidationAspect<AddPostDto>(typeof(PostValidator))]
        public IResult Add(AddPostDto postDto)
        {
            Post addPost = _mapper.Map<Post>(postDto);
            addPost.CreateDate = DateTime.Now;
            addPost.IsDeleted = false;
            addPost.UpdateTime = DateTime.Now;


            _postDal.Add(addPost);
            return new SuccessResult("Post added successfully");
        }

        [SecuredOperation("User,post.delete")]
        public IResult Delete(int postId, int userId)
        {

            bool deletePostClaim = _httpContextAccessor.HttpContext.User.ClaimRoles().Contains("post.delete");

            var deletePost = _postDal.Get(p => p.Id == postId && p.IsDeleted == false);

            if (deletePost != null && (deletePostClaim || deletePost.UserId == userId))
            {
                deletePost.IsDeleted = true;
                deletePost.UpdateTime = DateTime.Now;
                _postDal.Delete(deletePost);
                return new SuccessResult("Post has been deleted successfully");
            }
            else
            {
                return new ErrorResult("Post is not found or You cannot delete");
            }
        }


        [SecuredOperation("User,Admin,Moderator")]
        [ValidationAspect<UpdatePostDto>(typeof(UpdatePostDtoValidator))]
        public IResult Update(UpdatePostDto post, int userId)
        {
            //Post updatePost = _mapper.Map<Post>(post);
            Post uPost = _postDal.Get(p => p.Id == post.PostId);

            if (uPost != null && uPost.UserId == userId)
            {
                 // Ensure the userId is set correctly
                uPost.UpdateTime = DateTime.Now;
                uPost.Title = post.Title;
                uPost.Context = post.Context;
                _postDal.Update(uPost);
                return new SuccessResult("Post is successfully updated");
            }
            else
            {
                return new ErrorResult("An error occurred while updating the post");
            }
        }

        public IDataResult<List<GetPostDto>> GetPostsByUserId(int userId)
        {
            var posts = _postDal.GetAll(p => p.IsDeleted == false && p.UserId == userId)
                               .OrderByDescending(p => p.UpdateTime);

            

            List<GetPostDto> result = new List<GetPostDto>();

            foreach (var post in posts)
            {
                var user = _userService.GetUserById(post.UserId);
                GetPostDto postDto = new()
                {
                    UserId = user.User.Id,
                    Context = post.Context,
                    PostId = post.Id,
                    CommentCount = _commentService.GetCommentsByPostId(post.Id).Data.Count,
                    Title = post.Title,
                    Username = user.User.Username,
                    Date = post.UpdateTime.HasValue ? post.UpdateTime.Value.ToString("dd/MM/yyyy") : ""
                };

                result.Add(postDto);
            }
            if (result.Count > 0)
            {
                return new SuccessDataResult<List<GetPostDto>>(result, "This user's posts successfully fetched");
            }
            else
            {
                return new ErrorDataResult<List<GetPostDto>>(result, "This is not foun or not shared any post");
            }
        }

        public IDataResult<Post> GetPostById(int id)
        {
            var result = _postDal.Get(p => p.Id == id && p.IsDeleted == false);

            if (result != null)
            {
                return new SuccessDataResult<Post>(result, "Post successfully fetched");
            }
            else
            {
                return new ErrorDataResult<Post>(result, "An error occured while post was fetching");
            }
        }

        public IDataResult<List<GetPostDto>> GetAllPosts(int page = 1, int pageSize = 5)
        {
            // Get all posts ordered by UpdateTime descending (newest to oldest)
            var posts = _postDal.GetAll(p => p.IsDeleted == false)
                                .OrderByDescending(p => p.UpdateTime);

            // Apply pagination (Skip and Take)
            var pagedPosts = posts.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            List<GetPostDto> result = new List<GetPostDto>();

            foreach (var post in pagedPosts)
            {
                var user = _userService.GetUserById(post.UserId);
                GetPostDto postDto = new()
                {
                    PostId = post.Id,
                    UserId = user.User.Id,
                    Context = post.Context,
                    CommentCount = _commentService.GetCommentsByPostId(post.Id).Data.Count,
                    Title = post.Title,
                    Username = user.User.Username,
                    Date = post.UpdateTime.HasValue ? post.UpdateTime.Value.ToString("H:mm | dd/MM/yyyy") : ""
                };

                result.Add(postDto);
            }

            if (result.Count > 0)
            {
                return new SuccessDataResult<List<GetPostDto>>(result, "Posts successfully fetched");
            }
            else
            {
                return new ErrorDataResult<List<GetPostDto>>(result, "No posts found");
            }
        }

        public IDataResult<List<Post>> GetPosts()
        {
            var result = _postDal.GetAll(p => p.IsDeleted == false);
            if(result.Count > 0)
            {
                return new SuccessDataResult<List<Post>>(result, "All the posts succesfully fetched");
            }
            else
            {
                return new ErrorDataResult<List<Post>>(result, "An error occured while posts were fetching");
            }
            throw new NotImplementedException();
        }
        public int GetPostCount()
        {
            return _postDal.GetAll(p => p.IsDeleted == false).Count;
        }

    }
}
