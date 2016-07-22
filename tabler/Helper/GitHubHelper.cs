using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace tabler.Helper
{
    public class GitHubHelper
    {
        private const string RELEASE_URL = "https://api.github.com/repos/bux578/tabler/releases";

        public async Task<object> GetUrlContentsAsync(string url)
        {
            // The downloaded resource ends up in the variable named content. 
            var content = new MemoryStream();

            // Initialize an HttpWebRequest for the current URL. 
            var webReq = (HttpWebRequest)WebRequest.Create(url);
            webReq.Method = "GET";
            webReq.Accept = "application/vnd.github.v3+json";
            webReq.UserAgent = "tabler";

            var result = "";
            var serializer = new JsonSerializer();

            // Send the request to the Internet resource and wait for 
            // the response.                 
            using (WebResponse response = await webReq.GetResponseAsync())
            {

                using (var sr = new StreamReader(response.GetResponseStream()))
                {

                    using (var jsonTextReader = new JsonTextReader(sr))
                    {
                        try
                        {
                            return serializer.Deserialize(jsonTextReader);
                        }
                        catch (System.Exception)
                        {
                            
                            throw;
                        }
                       
                    }




                }

                //using (Stream responseStream = response.GetResponseStream()) {
                //    await responseStream.CopyToAsync(content);
                //}
            }

            //return content.ToArray();
            //return result;
        }

        public object GetGitRepo(string url)
        {

            // The downloaded resource ends up in the variable named content. 
            //var content = new MemoryStream();

            // Initialize an HttpWebRequest for the current URL. 
            var webReq = (HttpWebRequest)WebRequest.Create(url);
            webReq.Method = "GET";
            webReq.Accept = "application/vnd.github.v3+json";
            webReq.UserAgent = "tabler";
            webReq.MediaType = "UTF-8";

            var result = "";
          

            // Send the request to the Internet resource and wait for 
            // the response.                 
            using (WebResponse response =  webReq.GetResponse())
            {

                using (var sr = new StreamReader(response.GetResponseStream()))
                {

                    using (var jsonTextReader = new JsonTextReader(sr))
                    {
                        try
                        {





                            var serializer = new JsonSerializer();
                            return serializer.Deserialize(jsonTextReader);
                        }
                        catch (System.Exception)
                        {

                            throw;
                        }

                    }




                }

                //using (Stream responseStream = response.GetResponseStream()) {
                //    await responseStream.CopyToAsync(content);
                //}
            }

            //return content.ToArray();
            //return result;


        }


        public async Task<object> GetJsonAsync(string url)
        {
            var content = new MemoryStream();

            // Initialize an HttpWebRequest for the current URL. 
            var webReq = (HttpWebRequest)WebRequest.Create(url);
            webReq.Method = "GET";
            webReq.Accept = "application/vnd.github.v3+json";
            webReq.UserAgent = "tabler";

            // Send the request to the Internet resource and wait for 
            // the response. 
            // Note: you can't use HttpWebRequest.GetResponse in a Windows Store app. 
            using (WebResponse response = await webReq.GetResponseAsync())
            {
                // Get the data stream that is associated with the specified URL. 
                using (Stream responseStream = response.GetResponseStream())
                {
                    // Read the bytes in responseStream and copy them to content.  
                    await responseStream.CopyToAsync(content);


                    var serializer = new JsonSerializer();

                    using (var sr = new StreamReader(content))
                    using (var jsonTextReader = new JsonTextReader(sr))
                    {
                        return serializer.Deserialize(jsonTextReader);
                    }

                }
            }


        }
    }
}