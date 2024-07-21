using System;
using System.Runtime.InteropServices;
using System.Web;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class AuthorizationManager
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string GetURLFromPage();

        [DllImport("__Internal")]
        private static extern string GetQueryParam(string paramId);
		        
		public string ReadURL()
        {
            return GetURLFromPage();
        }

        public string ReadQueryParam(string paramName)
        {
            return GetQueryParam(paramName);
        }
#endif

        string _testURL = "https://mini-app-game.hamstamania.dev.gamemob.tech/?token=askdljhfklasjhgdfljkasghdfjkhasdf";

        public async UniTask<string> Authorization()
        {

#if UNITY_EDITOR
            return "test_only";
#endif
#if UNITY_WEBGL && !UNITY_EDITOR
            return GetQueryParam("token");
#endif
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        private string GetTokenSystemWeb(string url)
        {
            var uri = new Uri(url);
            var query = HttpUtility.ParseQueryString(uri.Query);

            foreach (var k in query.AllKeys)
            {
                Debug.Log($"KEY( {k} ) VALUE( {query.Get(k)} )");
            }


            if (query["token"] != null)
                return query.Get("token");

            return "";
        }
#endif

        private string GetAuthToken(string url)
        {
            var allQueryParams = url.Split('?')[1];
            var paramsSplit = allQueryParams.Split('&');

            foreach (var param in paramsSplit)
            {
                Debug.Log(param);

                if (!param.StartsWith("token"))
                {
                    Debug.Log("Token not found");
                    continue;
                }
                var splits = param.Split('=');
                Debug.Log("splits count:" + splits.Length);

                foreach (var split in splits)
                {
                    Debug.Log(split);
                    if (split.Contains("token")) continue;
                    return split;
                }
                    
                
                var jwtToken = splits[1];
                // sometimes the url has a # at the end of the string
                Debug.Log("Token " + jwtToken);
                return jwtToken;
            }
            return "";
        }
        
    }
}