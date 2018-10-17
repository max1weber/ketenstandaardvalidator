

using nl.ketenstandaard.api.environment;
using nl.ketenstandaard.api.models;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nl.ketenstandaard.api
{
    public class Validator 
    {
        /// <summary>
        /// ClientId of the calling application Provided by the Ketenstandaard
        /// </summary>
        public string ClientID { get;private set; }

        /// <summary>
        /// ClientSecret of the calling application Provided by the Ketenstandaard
        /// </summary>
        public string ClientSecret { get; private set; }

        /// <summary>
        /// Username Provided by the Ketenstandaard
        /// </summary>
        public string Username { get; private set; }


        /// <summary>
        /// Password Provided by the Ketenstandaard
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Target Environment of the Ketenstandaard API 
        /// </summary>
        public Environment TargetEnvironment { get; set; }

        

        /// <summary>
        /// Has the Validator already been Authenticated by the OAuth2-API 
        /// </summary>
        public bool IsAuthenticated { get; set; }

        internal string authEndpoint;
        internal string serviceEndpoint; 
        internal DateTime ExpirationDateTime;
        internal AccessToken AccessToken;
        internal RestClient client;
        /// <summary>
        /// Validator who will validate a given XML string against the Ketenstandaard Validator API. 
        /// </summary>
        /// <param name="username">Username provided by Ketenstandaard</param>
        /// <param name="password">Password provided by Ketenstandaard</param>
        /// <param name="clientId">ClientId of the application provided by Ketenstandaard</param>
        /// <param name="clientSecret">ClientSecret of the application provided by Ketenstandaard</param>
        /// <param name="targetEnvironment">Target Environment of the API (Default: Development) </param>
        public Validator(string username, string password, string clientId, string clientSecret, Environment targetEnvironment = Environment.Development)
        {
            TargetEnvironment = targetEnvironment;
            ClientID = clientId;
            ClientSecret = clientSecret;
            Username = username;
            Password = password;

            switch (TargetEnvironment)
            {
                case Environment.Test:
                    serviceEndpoint = ServiceEndpoint.TestEndpoint;
                    authEndpoint = AuthEndpoint.TestEndpoint;
                    break;
                case Environment.Acceptance:
                    serviceEndpoint = ServiceEndpoint.AcceptanceEndpoint;
                    authEndpoint = AuthEndpoint.AcceptanceEndpoint;
                    break;
                case Environment.Production:
                    serviceEndpoint = ServiceEndpoint.ProductionEndpoint;
                    authEndpoint = AuthEndpoint.ProductionEndpoint;
                    break;
                default:
                    serviceEndpoint = ServiceEndpoint.DevelopEndpoint;
                    authEndpoint = AuthEndpoint.DevelopEndpoint;
                    break;
            }

            client = new RestClient();
        }


        /// <summary>
        /// The Client will be Authenticated against the TargetEnvironment of the API.
        /// </summary>

        public void Authenticate()
        {




            //create RestSharp client and POST request object
            client.BaseUrl = new Uri(authEndpoint);
            var request = new RestRequest(Method.POST);

            //add GetToken() API method parameters
            request.Parameters.Clear();
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", Username);
            request.AddParameter("password", Password);
            request.AddParameter("client_id", ClientID);
            request.AddParameter("client_secret", ClientSecret);
            request.AddParameter("scope", "Api");

            //make the API request and get the response
            IRestResponse response = client.Execute(request);

            //return an AccessToken
           AccessToken= SimpleJson.SimpleJson.DeserializeObject<AccessToken>(response.Content);

            IsAuthenticated = true;

            ExpirationDateTime = DateTime.Now.AddSeconds(AccessToken.expires_in);
        }

        /// <summary>
        /// Validate a XML string against the Ketenstandaard messagestandards
        /// </summary>
        /// <param name="messageformat">Expected Format to check (e.g. INSBOU)</param>
        /// <param name="messageversion">Expected  Version of the Format (e.g. 004)</param>
        /// <param name="messagetype">Expected  Version of the Format (e.g. 004)</param>
        /// <param name="messagecontent">payload in string format</param>
        /// 
        ///
        /// <returns>ValidationResult</returns>
        /// <seealso cref="nl.ketenstandaard.api.models.ValidationResult">
        /// Take a look at the validationResult Class
        /// </seealso>

        public ValidationResult ValidateXmlMessage(string messageformat, string messageversion,string messagetype, string messagecontent)
        {

            if (!IsAuthenticated)
                Authenticate();




            //create RestSharp client and POST request object
            client.BaseUrl = new Uri(serviceEndpoint);
            var request = new RestRequest(Method.POST);
            client.Authenticator = new JwtAuthenticator(AccessToken.access_token);

            //clear paramaters
            request.Parameters.Clear();
            

            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");

            request.Parameters.Add(new Parameter()
            {
                ContentType = "text/plain",
                Name = "body", // not required 
                Type = ParameterType.RequestBody,
                Value = messagecontent
            });

            //make the API request and get the response
            IRestResponse response = client.Execute(request);

            ValidationResult result = SimpleJson.SimpleJson.DeserializeObject<ValidationResult>(response.Content);

            result.ExpectedFormat = messageformat;
            result.ExpectedType = messagetype;
            result.ExpectedVersion = messageversion;
            Trace.WriteLine("Validation Result " + result.IsValid);
            request = null;
            client.Authenticator = null;
            return result;
        }

    }
}
