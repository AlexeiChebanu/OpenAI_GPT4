using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI
{
    public class APIAuthentication
    {

        public string ApiKey { get; set; }
		public string OpenAIOrganization { get; set; }

        public static implicit operator APIAuthentication(string key)
        {
            return new APIAuthentication(key);
        }

        public APIAuthentication(string apiKey)
        {
            ApiKey = apiKey;
        }

		public APIAuthentication(string apiKey, string openAIOrganization)
        {
            this.ApiKey = apiKey;
            this.OpenAIOrganization = openAIOrganization;
        }

        private static APIAuthentication cachedDefault = null;

        public static APIAuthentication Default
        {
            get
            {
                if (cachedDefault != null)
                    return cachedDefault;

                APIAuthentication auth = LoadFromEnv();
                if (auth == null)
                    auth = LoadFromPath();

                cachedDefault = auth;
                return auth;
            }
            set
            {
                cachedDefault = value;
            }
        }
        public static APIAuthentication LoadFromEnv()
        {
            string key = Environment.GetEnvironmentVariable("OPENAI_KEY");

            if (string.IsNullOrEmpty(key))
            {
                key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

                if (string.IsNullOrEmpty(key))
                    return null;
            }

/*            string org = Environment.GetEnvironmentVariable("OPENAI_ORGANIZATION");*/

            return new APIAuthentication(key);
        }

         public static APIAuthentication LoadFromPath(string directory = null, string filename = ".openai", bool searchUp = true)
        {
            if (directory == null)
                directory = Environment.CurrentDirectory;

            string key = null;
            string org = null;
            var curDirectory = new DirectoryInfo(directory);

            while (key == null && curDirectory.Parent != null)
            {
                if (File.Exists(Path.Combine(curDirectory.FullName, filename)))
                {
                    var lines = File.ReadAllLines(Path.Combine(curDirectory.FullName, filename));
                    foreach (var l in lines)
                    {
                        var parts = l.Split('=', ':');
                        if (parts.Length == 2)
                        {
                            switch (parts[0].ToUpper())
                            {
                                case "OPENAI_KEY":
                                    key = parts[1].Trim();
                                    break;
                                case "OPENAI_API_KEY":
                                    key = parts[1].Trim();
                                    break;
                                /*case "OPENAI_ORGANIZATION":
                                    org = parts[1].Trim();
                                    break;*/
                                default:
                                    break;
                            }
                        }
                    }
                }

                if (searchUp)
                {
                    curDirectory = curDirectory.Parent;
                }
                else
                {
                    break;
                }
            }

            if (string.IsNullOrEmpty(key))
                return null;

            return new APIAuthentication(key, org);
        }

        public async Task<bool> ValidApiAuthentication()
        {
            if (string.IsNullOrEmpty(ApiKey)) return false;

            var apiKey = new OpenAIClient(this);

            List<Models.Model> results;

            try
            {
                results = await apiKey.Models.GetModelsAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
            return (results.Count > 0);

        }
    }
    internal static class AuthHelpers
    {
        public static APIAuthentication? ThisOrDefault(this APIAuthentication auth)
        {
            if (auth == null) auth = APIAuthentication.Default;

            return auth;

        }
    }
}

