using Billiard.Entities.AuditableEntity;
using Billiard.Entities.Identity;

namespace Billiard.Services.Identity.Logger;

public class LoggerItem
{
    public AppShadowProperties Props { set; get; }
    public AppLogItem AppLogItem { set; get; }
}