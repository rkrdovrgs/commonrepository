using Common.EntityFramework.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.EntityFramework.Extensions
{
    public abstract class ConventionBasedDbContext : DbContext
    {
        private string configurationNamespace;
        private string entitiesNamespace;
        private Type baseConfiguration;
        private Assembly assembly;

        public ConventionBasedDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            var config = GetConfiguration();
            configurationNamespace = config.ConfigurationNamespace;
            entitiesNamespace = config.EntitiesNamespace;
            baseConfiguration = config.BaseConfiguration;
            assembly = config.Assembly;
        }

        public abstract DbContextConfiguration GetConfiguration();

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //Dynamic DbSet generator
            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

            //Get entities from Models project
            //TODO: Find another way to get assembly types. Currently, if currency type is deleted, model builder will break.
            var props = assembly.GetTypes().Where(x => !string.IsNullOrEmpty(x.Namespace)
                            && x.Namespace.Equals(entitiesNamespace)
                            && !x.IsInterface
                            && !x.Name.EndsWith("Dto")
                            && !x.Name.StartsWith("<>")
                            && !x.IsAbstract);

            //Custom configurations
            var confFiles = assembly.GetTypes()
                                .Where(x =>
                                    string.Equals(x.Namespace,
                                        configurationNamespace,
                                        StringComparison.Ordinal) && !x.IsAbstract);

            //TODO: Add where statement to avoid entities with attr [NotDBMapped]
            foreach (var p in props)
            {
                var genTypeName = p.Name;

                //Check if there is custom configuration file
                var conf = confFiles.FirstOrDefault(x => x.BaseType.GetGenericArguments().Any(g => g.Name == genTypeName));
                if (conf != null)
                {
                    var type = assembly.GetType(conf.FullName);
                    dynamic inst = Activator.CreateInstance(type);
                    modelBuilder.Configurations.Add(inst);
                }
                //If not, then use base configuration to follow conventions
                else if (baseConfiguration != null)
                {
                    Type[] typeArgs = { p };
                    var genType = baseConfiguration.MakeGenericType(typeArgs);
                    dynamic inst = Activator.CreateInstance(genType);
                    modelBuilder.Configurations.Add(inst);
                }

                entityMethod.MakeGenericMethod(p)
                    .Invoke(modelBuilder, new object[] { });

            }
        }

        public static void ExecutePostScripts(DbContext context)
        {

            //Custom configurations
            var confFiles = context.GetType().Assembly
                                .GetTypes()
                                .Where(x =>
                                    string.Equals(x.Namespace,
                                        context.GetType().Namespace,
                                        StringComparison.Ordinal) && !x.IsAbstract);

            foreach (var conf in confFiles.Where(x => x.GetInterfaces().Contains(typeof(IConventionBasedConfiguration))))
            {
                var type = context.GetType().Assembly.GetType(conf.FullName);
                IConventionBasedConfiguration inst = (IConventionBasedConfiguration)Activator.CreateInstance(type);
                inst.PostScripts(context);
            }


        }
    }

    public class DbContextConfiguration
    {
        public Type BaseConfiguration { get; set; }
        public string ConfigurationNamespace { get; set; }
        public Assembly Assembly { get; set; }
        public string EntitiesNamespace { get; set; }
    }

}
