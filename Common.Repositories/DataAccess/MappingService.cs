using AutoMapper;
using Common.Core.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repositories.DataAccess
{

    public class MappingService : IMappingService
    {
        public TDest Map<TSrc, TDest>(TSrc source) where TDest : class
        {
            return Mapper.Map<TSrc, TDest>(source);
        }


        public TDest Map<TDest>(object source) where TDest : class
        {
            return Mapper.Map<TDest>(source);
        }


        public TDest Map<TSrc, TDest>(TSrc source, TDest destination)
            where TSrc : class
            where TDest : class
        {
            return Mapper.Map<TSrc, TDest>(source, destination);
        }
    }
}
