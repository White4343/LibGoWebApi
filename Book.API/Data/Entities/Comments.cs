﻿namespace Book.API.Data.Entities
{
    public class Comments
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int UserId { get; set; }
        
        public int BookId { get; set; }
        public Books Book { get; set; }
    }
}