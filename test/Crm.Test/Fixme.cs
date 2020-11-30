using Crm.Infrastructure.Data;
using Crm.Domain;
using Crm.Test.Setup;

namespace Crm.Test {
    public static class Fixme {
        public static User ReloadUser<TEntryPoint>(NhipsterWebApplicationFactory<TEntryPoint> factory, User user)
            where TEntryPoint : class
        {
            var applicationDatabaseContext = factory.GetRequiredService<ApplicationDatabaseContext>();
            applicationDatabaseContext.Entry(user).Reload();
            return user;
        }
    }
}
