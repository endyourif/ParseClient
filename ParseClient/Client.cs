using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ParseClient.Dtos;

namespace ParseClient
{
    public class Client
    {
        private readonly HttpClient _httpClient;

        private readonly string _appId;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        private const string _userDataClassName = "userData";
        private const string _gameConfigurationClassName = "gameConfigurationData";

        public Client()
        {
            _appId = ConfigurationManager.AppSettings["appId"];
            _apiKey = ConfigurationManager.AppSettings["masterKey"];

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-Parse-Application-Id", _appId);
            _httpClient.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", _apiKey);

            _baseUrl = "https://api.parse.com/1/classes/";
        }

        public string PostUserData(string encryptedData, DateTime? lastSyncedDate = null)
        {
            lastSyncedDate = DefaultLastSyncedDate(lastSyncedDate);

            return Post(_userDataClassName, encryptedData, lastSyncedDate);
        }

        public void PutUserData(string objectId, string encryptedData, DateTime? lastSyncedDate = null)
        {
            lastSyncedDate = DefaultLastSyncedDate(lastSyncedDate);

            Put(_userDataClassName, objectId, encryptedData, lastSyncedDate);
        }

        public DeserializableDto GetUserData(string objectId)
        {
            return Get(_userDataClassName, objectId);
        }

        public string PostGameConfigurationData(string encryptedData, DateTime? lastSyncedDate = null)
        {
            lastSyncedDate = DefaultLastSyncedDate(lastSyncedDate);

            return Post(_gameConfigurationClassName, encryptedData, lastSyncedDate);
        }

        public void PutGameConfigurationData(string objectId, string encryptedData, DateTime? lastSyncedDate = null)
        {
            lastSyncedDate = DefaultLastSyncedDate(lastSyncedDate);

            Put(_gameConfigurationClassName, objectId, encryptedData, lastSyncedDate);
        }

        public DeserializableDto GetGameConfigurationData(string objectId)
        {
            return Get(_gameConfigurationClassName, objectId);
        }

        public DeserializableDto Get(string className, string objectId)
        {
            var url = string.Format("{0}{1}/{2}", _baseUrl, className, objectId);

            var result = _httpClient.GetAsync(url).Result;
            result.EnsureSuccessStatusCode();

            var content = result.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<DeserializableDto>(content);
        }

        public string Post(string className, string data, DateTime? lastSyncedDate = null)
        {
            lastSyncedDate = DefaultLastSyncedDate(lastSyncedDate);

            var entity = new SerializableDto { encryptedData = data, lastSyncedDate = lastSyncedDate.Value };
            var url = string.Format("{0}{1}", _baseUrl, className);

            var result = _httpClient.PostAsync(url, SetupJson(entity)).Result;
            result.EnsureSuccessStatusCode();

            return ExtractNewObjectId(result);
        }
  
        private string ExtractNewObjectId(HttpResponseMessage result)
        {
            IEnumerable<string> values;
            var doesHeaderExist = result.Headers.TryGetValues("Location", out values);
            if (!doesHeaderExist)
                throw new InvalidOperationException("Unable to parse out response header with new location");

            var newObjectUrl = values.ToList()[0].ToString();
            var pieces = newObjectUrl.Split('/');

            return pieces.Last();
        }

        public void Put(string className, string objectId, string data, DateTime? lastSyncedDate = null)
        {
            lastSyncedDate = DefaultLastSyncedDate(lastSyncedDate);

            var entity = new SerializableDto { encryptedData = data, lastSyncedDate = lastSyncedDate.Value };
            var url = string.Format("{0}{1}/{2}", _baseUrl, className, objectId);

            var result = _httpClient.PutAsync(url, SetupJson(entity)).Result;
            result.EnsureSuccessStatusCode();
        }

        private HttpContent SetupJson(SerializableDto dto)
        {
            var inputJson = JsonConvert.SerializeObject(dto);
            return new StringContent(inputJson, Encoding.UTF8, "application/json");
        }

        private DateTime? DefaultLastSyncedDate(DateTime? lastSyncedDate)
        {
            if (null == lastSyncedDate)
                lastSyncedDate = DateTime.UtcNow;
            return lastSyncedDate;
        }
    }
}
