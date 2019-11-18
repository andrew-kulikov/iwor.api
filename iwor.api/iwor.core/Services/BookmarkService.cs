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
            var spec = new UserBookmarkSpecification(userId);
            var bookmarks = await _repository.ListAsync(spec);

            return bookmarks.ToList();
        }
    }
}