﻿using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class SendMessageRequest
    {
        [Required(ErrorMessage ="Receiver id is required!")]
        public int ReceiverId { get; set; }

        [Required(ErrorMessage ="Message content is required!")]
        [MinLength(1)]
        [MaxLength(5000)]
        public string Content { get; set; }
    }
}
