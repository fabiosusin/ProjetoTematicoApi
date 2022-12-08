using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DTO.Interface
{
    public interface IBaseData
    {
        int Id { get; set; }
    }
}
