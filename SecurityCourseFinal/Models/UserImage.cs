﻿using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityCourseFinal.Models
{
    [Table("UserImages")]
    public class UserImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Path { get; set; }

    }
}
