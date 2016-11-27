using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Server.Kestrel.Internal.Http;


namespace Web.Models
{
    public partial class Users
    {
        public Users()
        {
            BetsUser1 = new HashSet<Bets>();
            BetsUser2 = new HashSet<Bets>();
            BetsHistoryUser1 = new HashSet<BetsHistory>();
            BetsHistoryUser2 = new HashSet<BetsHistory>();
            BetsRequests = new HashSet<BetsRequests>();
            Competitions = new HashSet<Competitions>();
            UserCompetitionsMembers = new HashSet<UserCompetitionsMembers>();
            UserCompetitionsRequests = new HashSet<UserCompetitionsRequests>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get;  set; }

        [MaxLength(20), Required, MinLength(5)]
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
  

        public virtual ICollection<Bets> BetsUser1 { get; set; }
        public virtual ICollection<Bets> BetsUser2 { get; set; }
        public virtual ICollection<BetsHistory> BetsHistoryUser1 { get; set; }
        public virtual ICollection<BetsHistory> BetsHistoryUser2 { get; set; }
        public virtual ICollection<BetsRequests> BetsRequests { get; set; }
        public virtual ICollection<Competitions> Competitions { get; set; }
        public virtual Squads Squads { get; set; }
        public virtual ICollection<UserCompetitionsMembers> UserCompetitionsMembers { get; set; }
        public virtual ICollection<UserCompetitionsRequests> UserCompetitionsRequests { get; set; }
    }
}
