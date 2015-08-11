using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.DataAccess.Abstract
{
    public interface IFile: IEntity
    {
        int Id { get; set; }
        string Alias { get; set; }
        string Name { get; set; }
        //int ContentLength { get; set; }
        string ContentType { get; set; }
        byte[] Content { get; set; }

    }
}
