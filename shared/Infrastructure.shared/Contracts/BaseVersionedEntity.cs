using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.shared.Contracts;

public abstract class BaseVersionedEntity : BaseEntity
{
    public long Version { get; set; }

    /// <summary>
    ///   Gets a list of properties, which, when changed, produce a new version of the entity.
    ///   E.g. a change to DateUpdated typically is not meaningful, but change to a Cost is.
    /// </summary>
    [NotMapped]
    public abstract IList<string> VersionedFields { get; }
}
