using Newtonsoft.Json;
using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI;

public abstract class EndpointBase
{
    
    protected readonly OpenAIClient _Api;

    
    internal EndpointBase(OpenAIClient api)
    {
        this._Api = api;
    }

    
    protected abstract string Endpoint { get; }

    
    protected string Url
    {
        get
        {
            return string.Format(_Api.ApiUrlFormat, _Api.ApiVersion, Endpoint);
        }
    }

    protected HttpClient GetClient()
    {
        if (_Api.Auth?.ApiKey is null)
        {
            throw new AuthenticationException("You must provide API authentication.  Please refer to  for details.");
        }

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _Api.Auth.ApiKey);

        client.DefaultRequestHeaders.Add("API-KEY", _Api.Auth.ApiKey);

        return client;
    }

    protected string GetErrorMessage(string resultAsString, HttpResponseMessage response, string name, string description = "")
    {
        return $"Error at {name} ({description}) with HTTP status code: {response.StatusCode}. Content: {resultAsString ?? "<no content>"}";
    }

     private async Task<HttpResponseMessage> HttpRequestRaw(string url = null, HttpMethod verb = null, object postData = null, bool streaming = false)
    {
        if (string.IsNullOrEmpty(url))
            url = this.Url;

        if (verb == null)
            verb = HttpMethod.Get;

        using var client = GetClient();

        HttpResponseMessage response = null;
        string resultAsString = null;
        HttpRequestMessage req = new HttpRequestMessage(verb, url);

        if (postData != null)
        {
            if (postData is HttpContent)
            {
                req.Content = postData as HttpContent;
            }
            else
            {
                string jsonContent = JsonConvert.SerializeObject(postData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                var stringContent = new StringContent(jsonContent, UnicodeEncoding.UTF8, "application/json");
                req.Content = stringContent;
            }
        }
        response = await client.SendAsync(req, streaming ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }
        else
        {
            try
            {
                resultAsString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                resultAsString = "Additionally, the following error was thrown when attemping to read the response content: " + e.ToString();
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new AuthenticationException("OpenAI rejected your authorization, most likely due to an invalid API Key.  Try checking your API Key and see *** for guidance.  Full API response follows: " + resultAsString); //should be a link there
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException("OpenAI had an internal server error, which can happen occasionally.  Please retry your request.  " + GetErrorMessage(resultAsString, response, Endpoint, url));
            }
            else
            {
                throw new HttpRequestException(GetErrorMessage(resultAsString, response, Endpoint, url));
            }
        }
    }

    internal async Task<string> HttpGetContent<T>(string url = null)
    {
        var response = await HttpRequestRaw(url);
        return await response.Content.ReadAsStringAsync();
    }

    private async Task<T> HttpRequest<T>(string url = null, HttpMethod verb = null, object postData = null) where T : ApiResultBase
    {
        var response = await HttpRequestRaw(url, verb, postData);
        string resultAsString = await response.Content.ReadAsStringAsync();

        var res = JsonConvert.DeserializeObject<T>(resultAsString);
        try
        {
            res.RequestId = response.Headers.GetValues("X-Request-ID").FirstOrDefault();
            res.ProcessingTime = TimeSpan.FromMilliseconds(int.Parse(response.Headers.GetValues("Openai-Processing-Ms").First()));
            res.OpenaiVersion = response.Headers.GetValues("Openai-Version").FirstOrDefault();
            if (string.IsNullOrEmpty(res.Model))
                res.Model = response.Headers.GetValues("Openai-Model").FirstOrDefault();
        }
        catch (Exception e)
        {
            Debug.Print($"Issue parsing metadata of OpenAi Response.  Url: {url}, Error: {e.ToString()}, Response: {resultAsString}.  This is probably ignorable.");
        }

        return res;
    }

   internal async Task<T> HttpGet<T>(string url = null) where T : ApiResultBase
    {
        return await HttpRequest<T>(url, HttpMethod.Get);
    }

    internal async Task<T> HttpPost<T>(string url = null, object postData = null) where T : ApiResultBase
    {
        return await HttpRequest<T>(url, HttpMethod.Post, postData);
    }

    internal async Task<T> HttpDelete<T>(string url = null, object postData = null) where T : ApiResultBase
    {
        return await HttpRequest<T>(url, HttpMethod.Delete, postData);
    }


    internal async Task<T> HttpPut<T>(string url = null, object postData = null) where T : ApiResultBase
    {
        return await HttpRequest<T>(url, HttpMethod.Put, postData);
    }

    protected async IAsyncEnumerable<T> HttpStreamingRequest<T>(string url = null, HttpMethod verb = null, object postData = null) where T : ApiResultBase
    {
        var response = await HttpRequestRaw(url, verb, postData, true);

        string organization = null;
        string requestId = null;
        TimeSpan processingTime = TimeSpan.Zero;
        string openaiVersion = null;
        string modelFromHeaders = null;

        try
        {
            requestId = response.Headers.GetValues("X-Request-ID").FirstOrDefault();
            processingTime = TimeSpan.FromMilliseconds(int.Parse(response.Headers.GetValues("Openai-Processing-Ms").First()));
            openaiVersion = response.Headers.GetValues("Openai-Version").FirstOrDefault();
            modelFromHeaders = response.Headers.GetValues("Openai-Model").FirstOrDefault();
        }
        catch (Exception e)
        {
            Debug.Print($"Issue parsing metadata of OpenAi Response.  Url: {url}, Error: {e.ToString()}.  This is probably ignorable.");
        }

        string resultAsString = "";

        using (var stream = await response.Content.ReadAsStreamAsync())
        using (StreamReader reader = new StreamReader(stream))
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                resultAsString += line + Environment.NewLine;

                if (line.StartsWith("data:"))
                    line = line.Substring("data:".Length);

                line = line.TrimStart();

                if (line == "[DONE]")
                {
                    yield break;
                }
                else if (line.StartsWith(":"))
                { }
                else if (!string.IsNullOrWhiteSpace(line))
                {
                    var res = JsonConvert.DeserializeObject<T>(line);

                   /* res.Organization = organization;*/
                    res.RequestId = requestId;
                    res.ProcessingTime = processingTime;
                    res.OpenaiVersion = openaiVersion;
                    if (string.IsNullOrEmpty(res.Model))
                        res.Model = modelFromHeaders;

                    yield return res;
                }
            }
        }
    }
}