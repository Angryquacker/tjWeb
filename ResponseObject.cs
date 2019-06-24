using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TjWeb
{
    public class ResponseObject
    {
        public HttpListenerResponse Response;
        private String ResponseText;


        public ResponseObject(HttpListenerResponse res)
        {
            Response = res;
        }

        public void SetStatus(int code, String message)
        {
            Response.StatusCode = code;
            Response.StatusDescription = message;
        }

        public void SetHeader(String header, String value)
        {
            Response.AddHeader(header, value);
        }

        public void AddFile(String FilePath)
        {
            ResponseText = File.ReadAllText(FilePath);
        }

        public void AddText(String Text)
        {
            ResponseText = Text;
        }

        public void Send()
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(ResponseText);

            Response.ContentLength64 = buffer.Length;

            System.IO.Stream Output = Response.OutputStream;

            Output.Write(buffer, 0, buffer.Length);
            Output.Close();
        }
    }
}
