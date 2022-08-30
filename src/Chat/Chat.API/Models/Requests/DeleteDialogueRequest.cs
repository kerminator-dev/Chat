﻿using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class DeleteDialogueRequest
    {
        [Required(ErrorMessage = "Dialogue is required!")]
        public int DialogueId { get; set; }
    }
}