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
using System.Security.Claims;
using IResult = Core.Helpers.Results.Abstract.IResult;

namespace Business.Concrete
{
    public class PostManager : IPostService
    {
        public PostManager(IPostDal postDal, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _postDal = postDal;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        private readonly IPostDal _postDal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        [SecuredOperation("User,Admin,Moderator")]
        [ValidationAspect<AddPostDto>(typeof(PostValidator))]
        public IResult Add(AddPostDto post)
        {
            var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            Post addPost = _mapper.Map<Post>(post);

            addPost.UserId = int.Parse(userId);
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
            Post updatePost = _mapper.Map<Post>(post);

            if (updatePost != null && updatePost.UserId == userId)
            {
                updatePost.UserId = userId; // Ensure the userId is set correctly
                updatePost.UpdateTime = DateTime.Now;
                _postDal.Update(updatePost);
                return new SuccessResult("Post is successfully updated");
            }
            else
            {
                return new ErrorResult("An error occurred while updating the post");
            }
        }

        public IDataResult<List<Post>> GetPostsByUserId(int userId)
        {
            var posts = _postDal.GetAll(p => p.UserId == userId && p.IsDeleted == false);

            if (posts.Count > 0)
            {
                return new SuccessDataResult<List<Post>>(posts, "This user's posts successfully fetched");
            }
            else
            {
                return new ErrorDataResult<List<Post>>(posts, "This is not foun or not shared any post");
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

        public IDataResult<List<Post>> GetAllPosts()
        {
            var result = _postDal.GetAll(p => p.IsDeleted == false);

            if (result.Count > 0)
                return new SuccessDataResult<List<Post>>(result, "All the posts succesfully fetched");
            else
                return new ErrorDataResult<List<Post>>(result, "An error occured while posts were fetching");
        }
    }
}
