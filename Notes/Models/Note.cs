using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Notes.Models
{

    public class Settings
    {
        public string ConnectionString;
        public string Database;
    }

    [BsonIgnoreExtraElements]
    public class Note
    {
        [BsonId]
        public ObjectId ID { get; set; }
        [BsonElement("title")]
        public string title { get; set; }
        [BsonElement("plainText")]
        public string plainText { get; set; }
        [BsonElement("checklist")]
        public List<CheckList> checklist { get; set; }
        [BsonElement("label")]
        public List<Labels> label { get; set; }
       [BsonElement("IsPinned")]
        public bool IsPinned { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class CheckList
    {
       [BsonId]
        public int ID { get; set; }
        [BsonElement("value")]
        public string value { get; set; }
       [BsonElement("IsChecked")]
        public Boolean IsChecked { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Labels
    {

        [BsonId]
        public int ID { get; set; }
        [BsonElement("value")]
        public string value { get; set; }
    }
}
