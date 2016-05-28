using Core.Common.Contracts;
using Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Data
{
    public abstract class DataRepositoryBase<T, U> : IDataRepository<T>
        where T : class, IIdentifiableEntity, new()
        where U : DbContext, new()
    {
        protected abstract T AddEntity(U entityContex, T entity);
        protected abstract T UpdateEntity(U entityContex, T entity);
        protected abstract T GetEntities(U entityContex, int id);
        protected abstract IEnumerable<T> GetEntities(U entityContex);

        public T Add(T entity)
        {
            using (U entityContext = new U())
            {
                T addedEntity = AddEntity(entityContext, entity);
                entityContext.SaveChanges();
                return addedEntity;
            }
        }

        public IEnumerable<T> Get()
        {
            using (U entityContext = new U())
            {
                IEnumerable<T> entities = GetEntities(entityContext);
                return entities.ToArray().ToList();
            }
        }

        public T Get(int id)
        {
            using (U entityContext = new U())
            {
                T entity = GetEntities(entityContext, id);
                return entity;
            }
        }

        public void Remove(int id)
        {
            using (U entityContext = new U())
            {
                T entity = GetEntities(entityContext, id);
                entityContext.Entry<T>(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public void Remove(T entity)
        {
            using (U entityContext = new U())
            {
                entityContext.Entry<T>(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public T Update(T entity)
        {
            using (U entityContext = new U())
            {
                T updatedEntity = UpdateEntity(entityContext, entity);
                SimpleMapper.PropertyMap(entity, updatedEntity);
                entityContext.SaveChanges();
                return updatedEntity;
            }
        }
    }
}
