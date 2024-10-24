using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.shared.Services.Abstractions;

public interface ICurrentDateProvider
{
    DateTimeOffset Now { get; }
    DateTimeOffset NowUtc { get; }
}
