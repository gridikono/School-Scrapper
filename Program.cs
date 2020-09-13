using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Net;


namespace SchoolScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = @"C:\\Users\\User\\source\\repos\\SchoolScrapper\\output\\data.csv";
            var stream = File.AppendText(file);
            

            HtmlDocument htmlDoc = new HtmlDocument();
            string url = "http://wiki.gridi.al/uploads/china.html"; ;
            string urlResponse = URLRequest(url);
            //Convert the Raw HTML into an HTML Object
            htmlDoc.LoadHtml(urlResponse);


            //Find all A tags in the document
            var anchorNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='col-md-4 listing']/div[@class='well ']"); //col-md-4 listing

            if (anchorNodes != null)
            {
                string csvRowHeader = string.Format("{0}", "School Name");
                stream.WriteLine(csvRowHeader);

                foreach (var anchorNode in anchorNodes)
                {
                   // var fc = anchorNode.FirstChild[0];
                    var schoolName = anchorNode.SelectSingleNode("h3[2]/a").InnerText;
                    var schoolDesc = anchorNode.SelectSingleNode("p[1]").InnerText;
                    var schoolWebsite = anchorNode.SelectSingleNode("p[2]").InnerText;
                     Console.WriteLine(String.Format("{0} ", schoolName));
                    string csvRow = string.Format("{0}", schoolName);
                    stream.WriteLine(csvRow);
                }
            }

        }

        //General Function to request data from a Server
        static string URLRequest(string url)
        {
            // Prepare the Request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // Set method to GET to retrieve data
            request.Method = "GET";
            request.Timeout = 6000; //60 second timeout
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";

            string responseContent = null;

            // Get the Response
            using (WebResponse response = request.GetResponse())
            {
                // Retrieve a handle to the Stream
                using (Stream stream = response.GetResponseStream())
                {
                    // Begin reading the Stream
                    using (StreamReader streamreader = new StreamReader(stream))
                    {
                        // Read the Response Stream to the end
                        responseContent = streamreader.ReadToEnd();
                    }
                }
            }

            return (responseContent);
        }
    }
}
