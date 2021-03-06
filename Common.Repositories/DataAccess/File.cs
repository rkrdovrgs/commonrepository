﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Core.DataAccess.Abstract;

namespace Common.Repositories
{
    public class File: IFile
    {
        public int Id { get; set; }

        public byte[] Content { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public string ContentType { get; set; }
    }
}
