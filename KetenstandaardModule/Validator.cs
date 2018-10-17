using KetenstandaardModule.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetenstandaardModule
{
    public class Validator
    {

        public string ClientID { get;private set; }

        public string ClientSecret { get; private set; }

        public string Username { get; private set; }


        public string Password { get; private set; }

        internal DateTime ExpirationDateTime;

        public bool IsAuthenticated { get; set; }


        internal AccessToken AccessToken;

        public Validator(string username, string password, string clientId, string clientSecret)
        {
            ClientID = clientId;
            ClientSecret = clientSecret;
            Username = username;
            Password = password;
        }



        public void Authenticate(string authorizationendpoint= AuthEndpoint.ProductionEndpoint)
        {

            var url = authorizationendpoint;
           

            //create RestSharp client and POST request object
            var client = new RestClient(url.ToString());
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
           AccessToken= JsonConvert.DeserializeObject<AccessToken>(response.Content);

            IsAuthenticated = true;

            ExpirationDateTime = DateTime.Now.AddSeconds(AccessToken.expires_in);
        }


        public ValidationResult ValidateXmlMessage(string messageformat, string messageversion, string messagecontent, string serviceEndpoint= ServiceEndpoints.ProductionEndpoint)
        {

            if (!IsAuthenticated)
                Authenticate();
            //create RestSharp client and POST request object
            var client = new RestClient(serviceEndpoint);
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

            ValidationResult result = JsonConvert.DeserializeObject<ValidationResult>(response.Content);

            return result;
        }

    }
}
