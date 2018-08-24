using Estrutura.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Estrutura.API.Controller
{
    [RoutePrefix("api/groupkt")]
    public class GroupKTController : ApiController
    {
        private readonly string _baseURL = "http://services.groupkt.com/state";

        public IEnumerable<States> Get()
        {
            IList<States> collection = new List<States>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{_baseURL}/get/USA/all");
                var response = client.GetAsync(client.BaseAddress);
                response.Wait();

                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    JObject content = JObject.Parse(readTask.Result);

                    IEnumerable<JToken> results = content["RestResponse"]["result"].Children().ToList();

                    foreach (var r in results)
                    {
                        var st = r.ToObject<States>();
                        collection.Add(st);
                    }
                }
                else
                {
                    ModelState modelState = new ModelState();
                    modelState.Errors.Add("Erro ao requisitar estados.");
                }
            }

            return collection;
        }
    }
}
