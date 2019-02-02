using System;
using System.IO;
using Newtonsoft.Json;

namespace Runpath.Api.Contract.Extensions
{
    public static class StreamExtensions
    {
        public static T ReadAndDeserializeFromJson<T>(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(Stream));
            }

            if (!stream.CanRead)
            {
                throw new NotSupportedException("Can't read from this stream");
            }

            using (var streamReader = new StreamReader(stream))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    var jsonSerializer = new JsonSerializer();
                    return jsonSerializer.Deserialize<T>(jsonTextReader);
                }                
            }
        }
    }
}
