using System;
using System.IO;
using System.Net;
using OpenQA.Selenium.Firefox;

namespace ConsoleApp17
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console Options
            Console.Title = "CaptchaX";

            // Firefox Driver Service
            FirefoxDriverService firefoxDriverService = FirefoxDriverService.CreateDefaultService();
            firefoxDriverService.HideCommandPromptWindow = true;

            // Firefox Driver
            FirefoxDriver firefoxDriver = new FirefoxDriver(firefoxDriverService);

            // Page URL
            string pageURL = "https://www.google.com/recaptcha/api2/demo";

            // API Key
            string apiKey = "YOUR API KEY";

            // DataSiteKey Captcha
            string dataSiteKey = "6Le-wvkSAAAAAPBMRTvw0Q4Muexq9bi0DJwx_mJ-";

            // Request String
            string firstRequest = $"https://2captcha.com/in.php?key={apiKey}&method=userrecaptcha&googlekey={dataSiteKey}&pageurl={pageURL}&json=1&invisible=1";

            // Server Response
            string responseFromServer;
            string responseFromServer2;

            // Goes To Website
            firefoxDriver.Navigate().GoToUrl(pageURL);

            // Web Request            
            // Create a request for the URL.
            WebRequest request = WebRequest.Create(firstRequest);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.
            WebResponse response = request.GetResponse();

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer);
            }

            string operation = responseFromServer;
            int colonreapeat = 0;

            string jsonNumber = "";

            for (int i = 0; i < operation.Length; i++)
            {
                if (colonreapeat == 2)
                {
                    if (operation[i].ToString() != "}")
                    {
                        jsonNumber = jsonNumber + operation[i].ToString();
                    }
                }

                if (operation[i].ToString() == ":")
                {
                    colonreapeat++;
                }
            }

            string realJSONnumber = "";

            for (int i = 0; i < jsonNumber.Length; i++)
            {
                if (jsonNumber[i] != '"')
                {
                    realJSONnumber = realJSONnumber + jsonNumber[i];
                }
            }

            string secondRequest = $"https://2captcha.com/res.php?key={apiKey}&action=get&id={realJSONnumber}&json=1";
            // Close the response.
            response.Close();

            // Second Request 


            while (true)
            {
                // Create a request for the URL.
                WebRequest request2 = WebRequest.Create(secondRequest);
                // If required by the server, set the credentials.
                request2.Credentials = CredentialCache.DefaultCredentials;

                // Get the response.
                WebResponse response2 = request2.GetResponse();

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (Stream dataStream = response2.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader2 = new StreamReader(dataStream);
                    // Read the content.
                    responseFromServer2 = reader2.ReadToEnd();
                    // Display the content.
                    Console.WriteLine(responseFromServer2);

                    if (responseFromServer2.Length > 100)
                    {
                        break;
                    }
                }

                // Close the response.
                response2.Close();

                System.Threading.Thread.Sleep(5000);
            }

            // Colon Repeater
            int colonRepeater2 = 0;

            // Real JSON FORMAT
            string realJSONFormat = "";

            for (int i = 0; i < responseFromServer2.Length; i++)
            {
                if (colonRepeater2 == 2)
                {
                    if (responseFromServer2[i] != '"' && responseFromServer2[i] != '}')
                    {
                        realJSONFormat = realJSONFormat + responseFromServer2[i].ToString();
                    }

                }

                if (responseFromServer2[i].ToString() == ":")
                {
                    colonRepeater2++;
                }
            }

            string writeToken = $"document.getElementById('g-recaptcha-response').innerHTML='{realJSONFormat}'";
            string submitJavascript = $"document.getElementById('recaptcha-demo-form').submit()";
            firefoxDriver.ExecuteScript(writeToken);
            System.Threading.Thread.Sleep(3000);
            firefoxDriver.ExecuteScript(submitJavascript);
            System.Threading.Thread.Sleep(3000);
        }
    }
}