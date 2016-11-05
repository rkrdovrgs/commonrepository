using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Angular.CSharpHelpers
{
    public class JsModelGeneratorExtensions
    {
        public static void CreateModule(string path, string moduleName, string factoryName, params Type[] types)
        {

            string basePath = HttpContext.Current.Server.MapPath(path);

            if (Directory.Exists(basePath))
            {
                string[] files = Directory.GetFiles(basePath, "", SearchOption.AllDirectories);
                foreach (string pathFile in files)
                {
                    File.Delete(pathFile);
                }
            }
            else
                Directory.CreateDirectory(basePath);



            var sb = new StringBuilder();


            sb.Append("(function () { \n");
            sb.Append("'use strict'; \n");
            sb.AppendFormat("var __serviceId = '{0}'; \n", factoryName);
            sb.AppendFormat("angular.module('{0}').factory(__serviceId, __serviceImplementation); \n", moduleName);
            sb.Append("function __serviceImplementation() { \n var __serviceInstance = {}; \n");



            foreach (var m in types)
            {



                var inst = Activator.CreateInstance(m);
                //if(model == undefined || model == null)
                //else
                sb.AppendFormat(" \n __serviceInstance.{0} = function (model, setDefaults) {{  \n var instanceOfT = ", m.Name);
                sb.Append(JsonConvert.SerializeObject(inst, Formatting.Indented));
                sb.Append("; \n");

                foreach (var p in m.GetProperties())
                {
                    if (p.PropertyType.IsGenericType && typeof(ICollection<>).IsAssignableFrom(p.PropertyType.GetGenericTypeDefinition()) ||
                            p.PropertyType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>)))
                        sb.AppendFormat("if(setDefaults) instanceOfT.{0} = []; \n", p.Name);
                    else if (types.Any(x => x == p.PropertyType))
                        sb.AppendFormat("if(setDefaults) instanceOfT.{0} = new __serviceInstance.{0}(); \n", p.Name);
                }

                sb.Append("if(model != undefined && model != null) { \n");
                foreach (var p in m.GetProperties())
                {
                    sb.AppendFormat("instanceOfT.{0} = model.{0}  || instanceOfT.{0}; \n", p.Name);
                }
                sb.Append("}; \n return instanceOfT; \n }; \n");

            }


            sb.Append("return __serviceInstance; \n } \n })();");


            var modPath = basePath + string.Format("\\models.{0}.{1}.js", moduleName, factoryName);
            using (StreamWriter file = new StreamWriter(modPath))
            {
                file.Write(sb.ToString());
            }

        }
    }
}
