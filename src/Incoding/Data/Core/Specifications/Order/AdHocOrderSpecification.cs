﻿namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Incoding.Extensions;

    #endregion

    public class AdHocOrderSpecification<TEntity>
    {
        #region Fields

        internal readonly List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> applies = new List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>();

        readonly List<Tuple<Expression<Func<TEntity, object>>, OrderType>> expressions = new List<Tuple<Expression<Func<TEntity, object>>, OrderType>>();

        #endregion

        #region Api Methods

        public AdHocOrderSpecification<TEntity> Order(Expression<Func<TEntity, object>> sortExpression, OrderType type)
        {
            this.expressions.Add(new Tuple<Expression<Func<TEntity, object>>, OrderType>(sortExpression, type));
            if (this.expressions.Count == 1)
                this.applies.Add(entities => type == OrderType.Ascending ? entities.OrderBy(sortExpression) : entities.OrderByDescending(sortExpression));
            else
            {
                this.applies.Add(entities =>
                                     {
                                         var orderEntities = entities as IOrderedQueryable<TEntity>;
                                         return type == OrderType.Ascending ? orderEntities.ThenBy(sortExpression) : orderEntities.ThenByDescending(sortExpression);
                                     });
            }

            return this;
        }

        public AdHocOrderSpecification<TEntity> OrderBy(Expression<Func<TEntity, object>> sortExpression)
        {
            return Order(sortExpression, OrderType.Ascending);
        }

        public AdHocOrderSpecification<TEntity> OrderByDescending(Expression<Func<TEntity, object>> sortExpression)
        {
            return Order(sortExpression, OrderType.Descending);
        }

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            return Equals(obj as AdHocOrderSpecification<TEntity>);
        }

        ////ncrunch: no coverage start
        public override int GetHashCode()
        {
            return 0;
        }

        ////ncrunch: no coverage end
        protected bool Equals(AdHocOrderSpecification<TEntity> other)
        {
            if (!this.IsReferenceEquals(other))
                return false;

            if (this.expressions.Count != other.expressions.Count)
            {
                Console.WriteLine(SpecificationMessageRes.AdHocOrderSpecification_Equal_diffrent_count_expressions.F(this.expressions.Count, other.expressions.Count));
                return false;
            }

            for (int i = 0; i < this.expressions.Count; i++)
            {
                if (!this.expressions[i].Item1.IsExpressionEqual(other.expressions[i].Item1))
                {
                    Console.WriteLine(SpecificationMessageRes.AdHocOrderSpecification_Equal_diffrent_expressions.F(this.expressions[i].Item1, other.expressions[i].Item1));
                    return false;
                }

                if (this.expressions[i].Item2 != other.expressions[i].Item2)
                {
                    Console.WriteLine(SpecificationMessageRes.AdHocOrderSpecification_Equal_diffrent_type.F(this.expressions[i].Item2, other.expressions[i].Item2));
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}