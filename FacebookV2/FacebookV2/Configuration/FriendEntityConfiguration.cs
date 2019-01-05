using FacebookV2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace FacebookV2.Configuration
{
    internal class FriendEntityConfiguration : EntityTypeConfiguration<Friend>
    {
        public FriendEntityConfiguration()
        {

            this.ToTable("Friends");

            this.HasKey(e => new { e.SenderId, e.ReceiverId });

            this.Property(e => e.CreatedDate).HasColumnType("datetime").IsRequired();
            this.Property(e => e.SenderId).HasColumnName("FirstUserId");
            this.Property(e => e.ReceiverId).HasColumnName("SecondUserId");


            this.HasRequired(d => d.Sender)
                .WithMany(p => p.FriendsSenderProfile)
                .HasForeignKey(d => d.SenderId)
                .WillCascadeOnDelete(false);

            this.HasRequired(d => d.Receiver)
                .WithMany(p => p.FriendsReceiverProfile)
                .HasForeignKey(d => d.ReceiverId)
                .WillCascadeOnDelete(false);
        }
    }
}