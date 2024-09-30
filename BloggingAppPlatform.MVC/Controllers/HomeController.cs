using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggingAppPlatform.MVC.Models;
using Business.Abstract;
using BloggingAppPlatform.MVC.ViewModels;
using Entities.DTOs;
using System.Collections.Generic;

namespace BloggingAppPlatform.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;

    public HomeController(ILogger<HomeController> logger, IPostService postService, ICommentService commentService)
    {
        _logger = logger;
        _postService = postService;
        _commentService = commentService;
    }

    public IActionResult Index(int page = 1)
    {
        int pageSize = 3;
        var postsResult = _postService.GetAllPosts(page, pageSize);
        int totalPosts = _postService.GetPostCount();
        int totalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);
        var posts = _postService.GetAllPosts(page).Data;
        List<GetCommentDto> allComments = new List<GetCommentDto>();

        foreach (var post in posts)
        {
            var commentsResult = _commentService.GetCommentsByPostId(post.PostId);
            if (commentsResult.Success)
            {
                allComments.AddRange(commentsResult.Data);
            }
            else
            {
            }
        }

        var postVM = new PostVM
        {
            Posts = postsResult.Data,
            Comments = allComments,
            CurrentPage = page,
            TotalPages = totalPages
        };

        // Fetch comments for each post
        foreach (var post in postVM.Posts)
        {
            var commentsResult = _commentService.GetCommentsByPostId(post.PostId);
            if (commentsResult.Success)
            {
                postVM.Comments = commentsResult.Data; // Assign the comments for each post
            }
        }

        return View(postVM);
    }
    //public IActionResult Index(int page = 1)
    //{
    //    // Fetch posts
    //    var posts = _postService.GetAllPosts(page).Data;

    //    // Initialize a list to hold all comments
    //    List<GetCommentDto> allComments = new List<GetCommentDto>();

    //    // Loop through each post and get comments
    //    foreach (var post in posts)
    //    {
    //        var commentsResult = _commentService.GetCommentsByPostId(post.PostId);
    //        if (commentsResult.Success)
    //        {
    //            allComments.AddRange(commentsResult.Data);
    //        }
    //        else
    //        {
    //            // Handle the error if needed
    //            // You might want to log it or notify the user
    //        }
    //    }

    //    // Map to ViewModel
    //    int pageSize = 3;
    //    var postsResult = _postService.GetAllPosts(page, pageSize);
    //    int totalPosts = _postService.GetPostCount();
    //    int totalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);
    //    var viewModel = new PostVM
    //    {
    //        Posts = posts,
    //        Comments = allComments, // Pass the collected comments here
    //        CurrentPage = page,
    //        TotalPages = totalPages
    //    };

    //    return View(viewModel);
    //}


    public IActionResult Error(int code)
    {
        switch (code)
        {
            case 404:
                return View("NotFound"); // Create a NotFound view for 404 errors
            // You can handle other status codes (like 500) here as well
            default:
                return View("Error"); // A generic error view for other codes
        }
    }
}
