using FacebookV2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace FacebookV2.Configuration
{
    public class FriendRequestEntityConfiguration : EntityTypeConfiguration<FriendRequest>
    {
        public FriendRequestEntityConfiguration()
        {
            this.ToTable("FriendRequests");

            this.HasKey(e => new { e.RequesterProfileId, e.RequestedProfileId });

            this.Property(e => e.CreatedOn).HasColumnType("datetime").IsRequired();

            this.Property(e => e.Status).IsRequired();

            this.HasRequired(d => d.RequestedProfile)
                .WithMany(p => p.FriendRequestsRequestedProfile)
                .HasForeignKey(d => d.RequestedProfileId)
                .WillCascadeOnDelete(false);

            this.HasRequired(d => d.RequesterProfile)
                .WithMany(p => p.FriendRequestsRequesterProfile)
                .HasForeignKey(d => d.RequesterProfileId)
                .WillCascadeOnDelete(false);
        }
    }
}