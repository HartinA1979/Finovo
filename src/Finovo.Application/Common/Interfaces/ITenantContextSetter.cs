namespace Finovo.Application.Common.Interfaces;

public interface ITenantContextSetter
{
    void SetTenantId(Guid tenantId);
    void SetAdminContext();
}
