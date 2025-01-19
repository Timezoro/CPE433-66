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
      sb.AppendFormat("<h1>Client IP: {0}</h1>", endpoint.Address);
      sb.AppendFormat("<h1>Client Port: {0}</h1>", endpoint.Port);
      sb.AppendFormat("<b>Browser Information: {0}</b>", request.getPropertyByKey("user-agent").Trim());
      sb.AppendFormat("<b>Accept Language: {0}</b>", request.getPropertyByKey("accept-language").Trim());
      sb.AppendFormat("<b>Accept Encoding: {0}</b>", request.getPropertyByKey("accept-encoding").Trim());
      sb.Append("</pre></body></html>");

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