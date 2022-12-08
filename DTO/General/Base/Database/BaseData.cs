using DTO.Interface;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DTO.General.Base.Database
{
    public class BaseData : IBaseData
    {
        public int Id { get; set; }
    }
}
