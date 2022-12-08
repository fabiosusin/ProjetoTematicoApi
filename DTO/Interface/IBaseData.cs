using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DTO.Interface
{
    public interface IBaseData
    {
        Guid Id { get; set; }
    }
}
