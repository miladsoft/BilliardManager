using System.ComponentModel.DataAnnotations.Schema;
using Billiard.Entities;
using Billiard.Services;


namespace Billiard.Services
{
    public abstract class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}