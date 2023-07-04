using System.ComponentModel.DataAnnotations;

namespace Dapper_CRUD.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        public string[] Hobbies { get; set; }
        public int BatchId { get; set; }


    }
}
