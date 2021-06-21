using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace DotNet_WebApi_Learning.Models
{
    public class Skill
    {
        
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Damage { get; set; }

        public List<Character> Characters { get; set; }

    }
}
