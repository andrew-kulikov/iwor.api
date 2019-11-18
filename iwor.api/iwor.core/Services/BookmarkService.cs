using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iwor.core.Entities;
using iwor.core.Repositories;
using iwor.core.Specifications;

namespace iwor.core.Services
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IRepository<Bookmark> _repository;

        public BookmarkService(IRepository<Bookmark> repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<Bookmark>> GetUserBookmarks(string userId)
        {
            var spec = new BookmarkSpecification(b => b.UserId == userId);
            var bookmarks = await _repository.ListAsync(spec);

            return bookmarks.ToList();
        }

        public async Task<Bookmark> GetUserBookmark(string userId, Guid id)
        {
            var spec = new BookmarkSpecification(b => b.UserId == userId && b.Id == id);

            var bookmarks = await _repository.ListAsync(spec);
            var bookmark = bookmarks.FirstOrDefault();

            return bookmark;
        }

        public async Task<bool> DeleteUserBookmark(string userId, Guid id)
        {
            var bookmark = await GetUserBookmark(userId, id);

            if (bookmark == null) return false;

            await _repository.DeleteAsync(bookmark);

            return true;
        }
    }
}