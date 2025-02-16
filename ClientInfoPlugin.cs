using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace DNWS
{
  class ClientInfoPlugin : IPlugin
  {
    protected static Dictionary<String, int> statDictionary = null;
    public ClientInfoPlugin()
    {
      if (statDictionary == null)
      {
        statDictionary = new Dictionary<String, int>();

      }
    }

    public void PreProcessing(HTTPRequest request)
    {
      if (statDictionary.ContainsKey(request.Url))
      {
        statDictionary[request.Url] = (int)statDictionary[request.Url] + 1;
      }
      else
      {
        statDictionary[request.Url] = 1;
      }
    }

    public HTTPResponse GetResponse(HTTPRequest request)
    {
      HTTPResponse response = null;
      StringBuilder sb = new StringBuilder();

      IPEndPoint endpoint = IPEndPoint.Parse(request.getPropertyByKey("remoteendpoint"));
      sb.Append("<html><body><pre style=\"display: flex; flex-direction: column; gap: 15px\">");
      sb.AppendFormat("<div>Client IP: {0}</div>", endpoint.Address);
      sb.AppendFormat("<div>Client Port: {0}</div>", endpoint.Port);
      sb.AppendFormat("<div>Browser Information: {0}</div>", request.getPropertyByKey("user-agent").Trim());
      sb.AppendFormat("<div>Accept Language: {0}</div>", request.getPropertyByKey("accept-language").Trim());
      sb.AppendFormat("<div>Accept Encoding: {0}</div>", request.getPropertyByKey("accept-encoding").Trim());

      sb.Append("</pre></body></html>");

      // simulate heavy processing for testing thread
      // System.Threading.Thread.Sleep(5000);

      response = new HTTPResponse(200);
      response.body = Encoding.UTF8.GetBytes(sb.ToString());
      return response;
    }


    public HTTPResponse PostProcessing(HTTPResponse response)
    {
      throw new NotImplementedException();
    }
  }
}