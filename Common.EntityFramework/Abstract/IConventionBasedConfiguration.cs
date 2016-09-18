using System.Data.Entity;

namespace Common.EntityFramework.Abstract
{
    public interface IConventionBasedConfiguration
    {
        void PostScripts(DbContext context);
    }
}
