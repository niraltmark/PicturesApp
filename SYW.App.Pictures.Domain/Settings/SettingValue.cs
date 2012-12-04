using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SYW.App.Pictures.Domain.Settings
{
    public class SettingValue
    {
        [BsonId]
        public ObjectId SettingId{ get; set; }

        [Required]
        public string Section { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}