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

    public class RedirectToFallbackException : Exception
    {
        public readonly string FallbackImage;
        public RedirectToFallbackException(string fallback, string message) : base(message)
        {
            FallbackImage = fallback;
        }
    }
}
