using Microsoft.EntityFrameworkCore;
using Moq;
using Scaffolding.Data.Entities.Abstract;
using Scaffolding.Data.Test.Entities;
using Scaffolding.Data.Test.Repo;
using System.Linq.Expressions;

namespace Scaffolding.Data.Repo.Test
{
    
    public static class DbSetExtensions
    {
        public static DbSet<T> MockDbSet<T>(this List<T> data) where T : class, IEntityBase, new()
        {
            var queryable = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IAsyncEnumerable<T>>()
                        .Setup(m => m.GetAsyncEnumerator(default))
                        .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);

            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            mockDbSet.Setup(d => d.Add(It.IsAny<T>())).Callback((T entity) => data.Add(entity));
            mockDbSet.Setup(d => d.AddRange(It.IsAny<IEnumerable<T>>())).Callback((IEnumerable<T> entities) => data.AddRange(entities));
            mockDbSet.Setup(d => d.Remove(It.IsAny<T>())).Callback((T entity) => data.Remove(entity));
            mockDbSet.Setup(d => d.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback((IEnumerable<T> entities) => data.RemoveAll(entities.Contains));
            return mockDbSet.Object;
        }
    }

}
