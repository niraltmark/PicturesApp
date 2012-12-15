using System.Collections.Generic;
using Mongo;
using SYW.App.Pictures.Domain.Entities;
using System.Linq;

namespace SYW.App.Pictures.Domain.DataAccess
{
	public interface IPicuturesRespository
	{
		IList<Picture> Get(int limit, int page = 1);
	}

	public class PicuturesRespository : IPicuturesRespository
	{
		private readonly IMongoStorage<Picture> _storage;

		public PicuturesRespository(IMongoStorage<Picture> storage)
		{
			_storage = storage;
		}

		public IList<Picture> Get(int limit, int page = 1)
		{
			return _storage.Query().Where(x => x.Status == (int) PictureStatus.Valid)
				.OrderByDescending(x => x.Date)
				.Skip((page - 1)*limit)
				.Take(limit)
				.ToList();
		}
	}
}