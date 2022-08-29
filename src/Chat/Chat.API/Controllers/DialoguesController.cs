﻿using Chat.API.Entities;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;
using Chat.API.Services.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DialoguesController : Controllers.ControllerBase
    {
        private readonly AuthenticationProvider _authenticationProvider;
        private readonly DialogueProvider _dialogueProvider;

        public DialoguesController(AuthenticationProvider authenticationProvider, DialogueProvider dialogueProvider)
        {
            _authenticationProvider = authenticationProvider;
            _dialogueProvider = dialogueProvider;
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAllDialogues()
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            User user = await _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (user == null)
            {
                return NotFound(new ErrorResponse("User not found"));
            }

            var userDialoguesResponce = await _dialogueProvider.GetAll(user);

            return Ok(userDialoguesResponce);
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> Create(CreateDialogueRequest createDialogueRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var requester = await _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (requester == null)
            {
                return NotFound(new ErrorResponse("User not found"));
            }

            var targetUser = await _authenticationProvider.GetUser(createDialogueRequest.TargetUserId);
            if (targetUser == null)
            {
                return NotFound(new ErrorResponse("User not found"));
            }

            if (requester.Id == targetUser.Id)
            {
                return BadRequest(new ErrorResponse("Cannot create dialog with yourself! (may be later)"));
            }

            await _dialogueProvider.Create(requester, targetUser);

            return Ok();
        }
    }
}