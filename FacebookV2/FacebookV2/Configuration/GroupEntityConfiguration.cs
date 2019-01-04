using FacebookV2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace FacebookV2.Configuration
{
    public class GroupEntityConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupEntityConfiguration()
        {
            this.HasMany<Profile>(s => s.Profiles)
                .WithMany(c => c.Groups)
                .Map(cs =>
                {
                    cs.MapLeftKey("GroupId");
                    cs.MapRightKey("ProfileId");
                    cs.ToTable("GroupProfiles");
                });
        }
    }
}