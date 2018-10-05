using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;

namespace DiscordBot.Tools
{
    public static class Extensions
    {
        public static bool IsStatusOk(this HttpStatusCode status)
        {
            return (int) status >= 200 && (int) status < 300;
        }
        
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            /* If this is a list, use the Count property for efficiency. 
             * The Count property is O(1) while IEnumerable.Count() is O(N). */
            var collection = enumerable as ICollection<T>;
            if (collection != null)
            {
                return collection.Count < 1;
            }
            return !enumerable.Any(); 
        }
        
        public static ImageCodecInfo GetEncoder(this ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
 
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}