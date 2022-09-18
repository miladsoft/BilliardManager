using System;
using System.ComponentModel.DataAnnotations.Schema;
using Billiard.Entities;


namespace Billiard.Entities
{
    public abstract class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }

        public DateTimeOffset CreatedDateTime { get; set; }=DateTimeOffset.Now;

        
    }
}