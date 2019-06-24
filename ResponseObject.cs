using System;
using System.Net;
using System.IO;

namespace TjWeb
{
    public class ResponseObject
    {
        //The original response object
        public HttpListenerResponse Response;

        //The text to send
        private String ResponseText;

        /*
         * Method -> Constructor [Sets the response object]
         * @Param (HttpListenerResponse) res -> The reponse object
         * Returns - Void
         */
        public ResponseObject(HttpListenerResponse res)
        {
            //Set the reponse object
            Response = res;
        }

        /*
         * Method -> SetStatus [Sets the status code and message of the response]
         * @Param (int) code -> The status code
         * @Param (String) message -> The status description
         * Returns - Void
         */ 
        public void SetStatus(int code, String message)
        {
            //Set the status code and description
            Response.StatusCode = code;
            Response.StatusDescription = message;
        }

        /*
         * Method -> SetHeader [Add a header to the response]
         * @Param (String) header -> The name of the header
         * @Param (String) value -> The value of the header
         * Returns - Void
         */ 
        public void SetHeader(String header, String value)
        {
            //Add the header
            Response.AddHeader(header, value);
        }

        /*
         * Method -> AddFile [Add an HTML file to the response]
         * @Param (String) FilePath -> The file to load into the reponse
         * Returns - Void
         */ 
        public void AddFile(String FilePath)
        {
            //Set the response text to the file's contents
            ResponseText = File.ReadAllText(FilePath);
        }

        /*
         * Method -> AddText [Add text to the response
         * @Param (String) Text -> The text for the response
         * Returns -> Void
         */ 
        public void AddText(String Text)
        {
            //Set the Response
            ResponseText = Text;
        }

        /*
         * Method -> Send [Send The Response]
         * Returns - Void
         */ 
        public void Send()
        {
            //Get the bytes of the response string
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(ResponseText);

            //Set the length of the response
            Response.ContentLength64 = buffer.Length;

            //Create the output stream of the response
            System.IO.Stream Output = Response.OutputStream;

            //Write to the output, then close the stream
            Output.Write(buffer, 0, buffer.Length);
            Output.Close();
        }
    }
}
