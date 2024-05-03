﻿namespace User.API.Models.Responses
{
    public class GetUserPrivatePageResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Description { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Role { get; set; }
    }
}