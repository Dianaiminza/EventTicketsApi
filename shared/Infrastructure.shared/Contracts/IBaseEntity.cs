using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.shared.Contracts;

public interface IBaseEntity : IEntity
{
    DateTimeOffset CreatedOn { get; set; }


    DateTimeOffset? LastModifiedOn { get; set; }
}
