using System;
using System.Linq.Expressions;
using iwor.core.Entities;

namespace iwor.core.Specifications
{
    public class UserBookmarkSpecification : BaseSpecification<Bookmark>
    {
        public UserBookmarkSpecification(string userId) : base(b => b.UserId == userId)
        {
        }
    }
}