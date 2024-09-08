using System.Collections.Generic;

namespace ClearFarmDebris.Framework;
internal class EqualityComparer<TEntity> : IEqualityComparer<TEntity> where TEntity : class {
  public static IEqualityComparer<TEntity> Comparer => new EqualityComparer<TEntity>();

  public bool Equals(TEntity? x, TEntity? y) => (x is null && y is null) || (x is not null && y is not null && x.Equals(y));

  public int GetHashCode(TEntity obj) => obj.GetHashCode();
}
