using RestSharp;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FluentAssertions;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;

namespace BenchMark.Example
{
    [SimpleJob(RuntimeMoniker.Net462)]
    [RPlotExporter]
    [CsvMeasurementsExporter]
    public class Program
    {
        private RestClient restClient;
        private RestRequest restRequest;
        private IRestResponse restResponse;
        private readonly string NewestFirstAPIUrl = @"https://www.systemtest.taxtechnical.ird.govt.nz/api/taxtechnicalsearch/getdocuments?page=1";

        [Benchmark]
        public void TestAPI()
        {
            var restResponse = NewestFirstResults();
            //restResponse.StatusCode.Should().Be(200);
        }

        public IRestResponse NewestFirstResults()
        {
            /* DataTable table = new DataTable();
             table.NewRow();*/
            restClient = new RestClient(NewestFirstAPIUrl);
            restRequest = new RestRequest();
            restRequest.AddParameter("sort", "best");
            restRequest.AddParameter("scope", "{EF03CAC2-54CF-4D82-99B4-264D68B4F418}");
            restRequest.AddParameter("search", "income tax");
            /*  foreach (var param in GetRequestHeaderParams(table))
                  restRequest.AddParameter(param.Key, param.Value);*/
            restRequest.Method = Method.GET;
            restRequest.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            restResponse = restClient.Execute(restRequest);
            return restResponse;
        }

       public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Program>();
        }
    }
}
