using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace PSLovers2.Data
{
    public class FavoriteForUser
    {
        public int Id { get; set; }
        public int FavoriteId { get; set; }
        [ForeignKey("FavoriteId")] public virtual Favorite Favorite{ get; set; }        
        public bool Owned { get; set; }        
        public IdentityUser User { get; set; }
        [ForeignKey("UserId")] public string UserId { get; set; }
        public DateTime? LastUpdateTime { get; set; }

        public FavoriteForUser()
        {

        }
    }


}
