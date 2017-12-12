using System;

namespace ImageServer.Core.Services
{
    public class SlugNotFoundException : Exception
    {
        public SlugNotFoundException(string message) : base(message)
        {
        }
    }

    public class GridFsObjectIdException : Exception
    {
        public GridFsObjectIdException(string message) : base(message)
        {
        }
    }
}
