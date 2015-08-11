using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Core.DataAccess.Abstract;

namespace Common.Core.Repositories
{
    public interface IFileRepository
    {
        IFile Get(int id, string name);

        IFile GetPreview(int id, string name);

        IFile Insert(byte[] content, string fileName, string contentType);

        void Delete(int id);

        void Rotate(int id);
    }
}
