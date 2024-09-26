﻿using AutoMapper;
using BloggingAppPlatform.MVC.Areas.Admin.ViewModels;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public UsersController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;  
        }
        [Area("Admin")]
        public IActionResult Index()
        {
            UserVM vm = new()
            {
                Users = _userService.GetAllUsers().Data,
            };
            return View(vm);
        }
        [HttpPost]
        [Area("Admin")]
        public IActionResult DeleteUser(int UserId)
        {
            var result = _userService.DeleteUser(UserId);

            if (result.Success)
            {
                TempData["Message"] = "User deleted successfully.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction("Index");
        }

    }
}
