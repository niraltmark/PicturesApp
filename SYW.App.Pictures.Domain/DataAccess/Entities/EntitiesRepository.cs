using System.Collections.Generic;
using System.Linq;
using CommonGround.Utilities;
using Mongo;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using SYW.App.Pictures.Domain.Entities;

namespace SYW.App.Pictures.Domain.DataAccess.Entities
{
	public interface IEntitiesRepository
	{
		void Add(Entity entity);
		void Update(Entity entity);
		IList<Entity> GetEntitiesByIds(long[] ids);
	    IList<Entity> GetEntitiesByOriginalIds(long[] ids);
        IList<Entity> GetEntitiesByIds(ObjectId[] ids);
	    ObjectId[] GetEntitiesIdsByEmail(IList<string> emails);
	    ObjectId? GetEntityIdByOriginalEmail(string originalEmail);
	    void Touch(Entity entity);
	}

	public class EntitiesRepository : IEntitiesRepository
	{
		private readonly IMongoStorage<Entity> _entitiesStorage;

		public EntitiesRepository(IMongoStorage<Entity> entitiesStorage)
		{
			_entitiesStorage = entitiesStorage;
		}

		public void Add(Entity entity)
		{
			if (entity.MemberSince == null)
				entity.MemberSince = SystemTime.Now();

			_entitiesStorage.Save(entity);
		}

		public void Update(Entity entity)
		{
			_entitiesStorage.Update(entity);
		}

		public IList<Entity> GetEntitiesByIds(long[] ids)
		{
			return ids.Select(id => _entitiesStorage.Query().Where(x => x.OriginalId == id).FirstOrDefault())
				.Where(e => e != null)
				.ToList();
		}

	    public IList<Entity> GetEntitiesByOriginalIds(long[] ids)
	    {
            return ids.Select(id => _entitiesStorage.Query().Where(x => x.OriginalId == id).FirstOrDefault())
                  .ToList()
                  .Where(e => e != null)
                  .ToList();
	    }

	    public IList<Entity> GetEntitiesByIds(ObjectId[] ids)
	    {
	    	return _entitiesStorage.Query().Where(x => x.Id.In(ids)).ToList();
		}

        public ObjectId? GetEntityIdByOriginalEmail(string originalEmail)
        {
            var entity = _entitiesStorage.Query().FirstOrDefault(x => x.OriginalEmail.ToLower() == originalEmail.ToLower());

            if (entity == null)
            	return null;

			return entity.Id;
        }
	    public ObjectId[] GetEntitiesIdsByEmail(IList<string> emails)
	    {
	        var matchedEntities = emails.SelectMany(email => _entitiesStorage.Query().Where(x => x.Email.ToLower() == email.ToLower())).ToList();
	        var result = matchedEntities.Select(x => x.Id).ToArray();
	        return result;
	    }

	    public void Touch(Entity entity)
	    {
	        entity.LastActivity = SystemTime.Now();
	        _entitiesStorage.Save(entity);
	    }
	}
}