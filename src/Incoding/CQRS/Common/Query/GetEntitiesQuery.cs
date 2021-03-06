namespace Incoding.CQRS
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Data;

    #endregion

    public class GetEntitiesQuery<T> : QueryBase<List<T>> where T : class, IEntity
    {
        #region Override

        protected override List<T> ExecuteResult()
        {
            return Repository.Query<T>().ToList();
        }

        #endregion
    }
}