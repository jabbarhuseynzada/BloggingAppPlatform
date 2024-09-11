using AutoMapper;
using Business.Abstract;
using Business.BusinessAspect.Autofac.Secured;
using Business.Validation.FluentValidation;
using Core.Aspects.Autofac.Validation.FluentValidation;
using Core.Entities.Concrete;
using Core.Extensions;
using Core.Helpers.Results.Abstract;
using Core.Helpers.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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

        [SecuredOperation("User,Admin")]
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

        //[SecuredOperation("User,Admin,Moderator")]
        //public IResult Delete(int id)
        //{
        //    var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) !;
        //    var deletePost = _postDal.Get(p => p.Id == id && p.IsDeleted == false);
        //    if (deletePost != null && deletePost.UserId == int.Parse(userId))
        //    {
        //        deletePost.IsDeleted = true;
        //        deletePost.UpdateTime = DateTime.Now;
        //        _postDal.Delete(deletePost);
        //        return new SuccessResult("Post have been deleted successfully");
        //    }
        //    else
        //    {
        //        return new ErrorResult("You don't have any access for deleting this post");
        //    }
        //}
        [SecuredOperation("User,Admin")]
        public IResult Delete(int id)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role); // Assuming role is stored in ClaimTypes.Role

            var deletePost = _postDal.Get(p => p.Id == id && p.IsDeleted == false);

            // Check if the user is an Admin or if the user is the owner of the post
            if (deletePost != null && (deletePost.UserId == int.Parse(userId) || userRole == "Admin"))
            {
                if (deletePost != null)
                {
                    deletePost.IsDeleted = true;
                    deletePost.UpdateTime = DateTime.Now;
                    _postDal.Delete(deletePost);
                    return new SuccessResult("Post has been deleted successfully");
                }
                else
                {
                    return new ErrorResult("Post not found");
                }
            }
            else
            {
                return new ErrorResult("You don't have access to delete this post");
            }
        }


        [SecuredOperation("User,Admin,Moderator")]
        [ValidationAspect<UpdatePostDto>(typeof(UpdatePostDtoValidator))]
        public IResult Update(UpdatePostDto post)
        {
            var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var userRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role)!;

            Post updatePost = _mapper.Map<Post>(post);
            if (updatePost != null && (updatePost.UserId == int.Parse(userId) || userRole == "Admin"))
            {
                updatePost.UserId = int.Parse(userId);
                updatePost.UpdateTime = DateTime.Now;
                _postDal.Update(updatePost);
                return new SuccessResult("Post is succesfully updated");
            }
            else
            {
                return new ErrorResult("An error occured while updating post");
            }
        }

        public IDataResult<List<AddPostDto>> GetPostsByUserId(int userId)
        {
            var posts = _postDal.GetAll(p => p.UserId == userId && p.IsDeleted==false);
            var postsDto = _mapper.Map<List<AddPostDto>>(posts);
            
            if(postsDto.Count>0)
            {
                return new SuccessDataResult<List<AddPostDto>>(postsDto, "This user's posts successfully fetched");
            }
            else
            {
                return new ErrorDataResult<List<AddPostDto>>(postsDto, "This is not foun or not shared any post");
            }
        }

        public IDataResult<AddPostDto> GetPostById(int id)
        {
            var post = _postDal.Get(p => p.Id == id && p.IsDeleted == false);
            AddPostDto result = new()
            {
                PostTitle = post.Title,
                PostContext = post.Context,
                CoverImageUrl = post.CoverImageUrl,
                UserId = post.UserId
            };
                ;
            if(result != null)
            {
                return new SuccessDataResult<AddPostDto>(result, "Post successfully fetched");
            }
            else
            {
                return new ErrorDataResult<AddPostDto>(result, "An error occured while post was fetching");
            }
        }

        public IDataResult<List<AddPostDto>> GetAllPosts()
        {
            var result = _postDal.GetAll(p => p.IsDeleted == false);
            var resultDto = _mapper.Map<List<AddPostDto>>(result);
            
            if(resultDto.Count > 0)
                return new SuccessDataResult<List<AddPostDto>>(resultDto, "All the posts succesfully fetched");
            else
                return new ErrorDataResult<List<AddPostDto>>(resultDto, "An error occured while posts were fetching");
        }
    }
}
