using System;
using System.Linq.Expressions;
using iwor.core.Entities;

namespace iwor.core.Specifications
{
    public class BookmarkSpecification : BaseSpecification<Bookmark>
    {
        public BookmarkSpecification(Expression<Func<Bookmark, bool>> criteria) : base(criteria)
        {
        }
    }
}