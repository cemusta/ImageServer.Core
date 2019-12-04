using System;

namespace ImageServer.Core.Services
{
    [Serializable]
    public class SlugNotFoundException : Exception
    {
        public SlugNotFoundException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class GridFsObjectIdException : Exception
    {
        public GridFsObjectIdException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class RedirectToFallbackException : Exception
    {
        public readonly string FallbackImage;
        public RedirectToFallbackException(string fallback, string message) : base(message)
        {
            FallbackImage = fallback;
        }
    }
}
