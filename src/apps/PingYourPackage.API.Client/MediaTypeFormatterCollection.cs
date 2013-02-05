using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Formatting;

namespace PingYourPackage.API.Client {

    internal sealed class MediaTypeFormatterCollection : ReadOnlyCollection<MediaTypeFormatter> {

        private static readonly Lazy<MediaTypeFormatterCollection> lazy =
               new Lazy<MediaTypeFormatterCollection>(() => new MediaTypeFormatterCollection());

        public static MediaTypeFormatterCollection Instance { get { return lazy.Value; } }

        private MediaTypeFormatterCollection() 
            : base(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() }) {
        }
    }
}