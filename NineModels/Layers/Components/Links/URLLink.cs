using System;

namespace Nine.Layers.Components.Links
{
    /// <summary>
    ///     Allow users to create URL-based links.
    /// </summary>
    [Serializable]
    public class URLLink : BaseModel, ILink
    {
        private string _url;

        public URLLink(string link)
        {
            URL = link;
        }

        public string URL
        {
            get { return _url; }
            private set { _url = value; }
        }

        public bool IsDead()
        {
            return false;
        }

        public string GetProtocole()
        {
            return URL.Substring(0, URL.IndexOf(':'));
        }
    }
}