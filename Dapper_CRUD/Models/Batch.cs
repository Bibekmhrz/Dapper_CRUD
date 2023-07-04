using System.Collections;
using System.Collections.Generic;

namespace Dapper_CRUD.Models
{
    public class Batch
    {
        public int BatchId { get; set; }
        public ICollection<Student> Students { get; set; }


    }
}
