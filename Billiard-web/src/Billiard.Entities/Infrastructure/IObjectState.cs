
using System.ComponentModel.DataAnnotations.Schema;

namespace Billiard.Entities
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}