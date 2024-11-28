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

    /*  public IActionResult Index(int page = 1)
      {
          // Fetch posts
          var posts = _postService.GetAllPosts(page).Data;

          // Initialize a list to hold all comments
          List<GetCommentDto> allComments = new List<GetCommentDto>();

          // Loop through each post and get comments
          foreach (var post in posts)
          {
              var commentsResult = _commentService.GetCommentsByPostId(post.PostId);
              if (commentsResult.Success)
              {
                  allComments.AddRange(commentsResult.Data);
              }
              else
              {
                  // Handle the error if needed
                  // You might want to log it or notify the user
              }
          }

          // Map to ViewModel
          int pageSize = 3;
          var postsResult = _postService.GetAllPosts(page, pageSize);
          int totalPosts = _postService.GetPostCount();
          int totalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);
          var viewModel = new PostVM
          {
              Posts = posts,
              Comments = allComments, // Pass the collected comments here
              CurrentPage = page,
              TotalPages = totalPages
          };

          return View(viewModel);
      }
  */
    public IActionResult Index(int page = 1)
    {
        // Define the page size
        int pageSize = 3;

        // Fetch posts for the current page
        var postsResult = _postService.GetAllPosts(page, pageSize);

        // If there are no posts, handle it gracefully
        if (!postsResult.Success)
        {
            // Handle the error if needed (e.g., log it or notify the user)
            return View(new PostVM
            {
                Posts = new List<GetPostDto>(), // Empty list if no posts
                Comments = new List<GetCommentDto>(),
                CurrentPage = page,
                TotalPages = 0
            });
        }

        var posts = postsResult.Data;

        // Get the total number of posts for pagination calculation
        int totalPosts = _postService.GetPostCount();
        int totalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);

        // Initialize a list to hold all comments
        List<GetCommentDto> allComments = new List<GetCommentDto>();
        // Fetch comments for each post
        foreach (var post in posts)
        {
            var commentsResult = _commentService.GetCommentsByPostId(post.PostId);
            if (commentsResult.Success)
            {
                allComments.AddRange(commentsResult.Data);
            }
            else
            {
                // Optional: Log the error or notify the user
            }
        }

        // Map data to the ViewModel
        var viewModel = new PostVM
        {
            Posts = posts,
            Comments = allComments,
            CurrentPage = page,
            TotalPages = totalPages
        };

        // Return the View with the populated ViewModel
        return View(viewModel);
    }

    public IActionResult Error(int code)
    {
        switch (code)
        {
            case 404:
                return View("NotFound"); 
            default:
                return View("Error"); 
        }
    }
}
