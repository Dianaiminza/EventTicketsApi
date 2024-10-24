using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.shared.Contracts;

public abstract class BaseEntity : IBaseEntity
{
    public BaseEntity()
    {
        CreatedOn = DateTime.UtcNow;
        LastModifiedOn = DateTime.UtcNow;
    }

    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
}
