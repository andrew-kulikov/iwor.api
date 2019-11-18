using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iwor.core.Entities;

namespace iwor.core.Services
{
    public interface IBookmarkService
    {
        Task<ICollection<Bookmark>> GetUserBookmarks(string userId);
        Task<Bookmark> GetUserBookmark(string userId, Guid id);
    }
}