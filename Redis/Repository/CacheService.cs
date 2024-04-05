
using Newtonsoft.Json;
using Redis.IRepository;
using StackExchange.Redis;
using System;
using Redis;
namespace Redis
{
    public class CacheService : ICacheService
    {
        private IDatabase _db;  //private variable used to store connection
        public CacheService()//constructor
        {
            ConfigureRedis();// calling the method
        }
        private void ConfigureRedis()  // method with data as void 
        {
            _db = ConnectionHelper.Connection.GetDatabase();   // connection to a Redis database.
        }
        public T GetData<T>(string key)  // method having datatype tuple and takes parameter [key.]
        {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value)) //if the string is not null or not  empty true , and false otherwise
            {
                return JsonConvert.DeserializeObject<T>(value); //JSON string (value) into an object of a specified type (T).
            }
            return default;
        }
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }
        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }
    }
}