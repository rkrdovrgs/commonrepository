using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.DataAccess.Abstract
{
    public interface IMappingService
    {
        TDest Map<TSrc, TDest>(TSrc source) where TDest : class;

        TDest Map<TDest>(object source) where TDest : class;

        TDest Map<TSrc, TDest>(TSrc source, TDest destination) where TDest: class where TSrc: class;
    }
}
