using eCaseBinderService.Config;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace eCaseBinderService.Services
{
    public class DraftService : IDraftService
    {
        private readonly DocumentClient client;
        private readonly Cosmos cosmos;

        public DraftService(IOptions<Cosmos> options)
        {
            cosmos = options.Value;
            client = new DocumentClient(new Uri(cosmos.AccountURL), cosmos.AuthKey);

        }

        public async Task<string> CreateDatabase(string dbName)
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(dbName));
                return "Database already exists";
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(
                        new Database { Id = dbName },
                        new RequestOptions
                        {
                            PartitionKey = new PartitionKey(cosmos.PartitionKey),
                            OfferThroughput = 400
                        });
                    return "Database created successfully";
                }
                else
                {
                    return e.StatusCode.ToString();
                }
            }
        }

        public async Task<string> CreateCollection(string collectionName)
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(cosmos.DatabaseId, collectionName), new RequestOptions { PartitionKey = new PartitionKey(cosmos.PartitionKey) });
                return "Collection already exists";
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    DocumentClient ddbClient = new DocumentClient(new Uri(cosmos.AccountURL), cosmos.AuthKey);
                    var docCCollection = await ddbClient.CreateDocumentCollectionAsync("dbs/" + cosmos.DatabaseId,
                        new DocumentCollection
                        {
                            Id = collectionName,
                            PartitionKey = new PartitionKeyDefinition
                            {
                                Paths = new Collection<string> { "/" + cosmos.PartitionKey }
                            }
                        },
                    new RequestOptions { OfferThroughput = 400 }).ConfigureAwait(false);
                    return "Collection created successfully";
                }
                else
                {
                    return e.StatusCode.ToString();
                }
            }
        }

        public async Task<bool> CreateLoan(JObject request)
        {
            try
            {
                await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(cosmos.DatabaseId, cosmos.CollectionId), request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<dynamic> GetLoans(string userName, int start, int limit)
        {
            try
            {
                IDocumentQuery<Object> QueryStr = client.CreateDocumentQuery<Object>(
                UriFactory.CreateDocumentCollectionUri(cosmos.DatabaseId, cosmos.CollectionId),                
                new FeedOptions
                {
                    EnableCrossPartitionQuery = true,
                    MaxItemCount = -1,
                    PartitionKey = new PartitionKey(userName),
                })
                .Skip(start)
                .Take(limit)
                .AsDocumentQuery();

                var result = QueryStr.ExecuteNextAsync<JObject>().Result.ToList();
                var draftLoans = new List<Object>();
                foreach (var item in result)
                {
                    var draftLoan = new
                    {
                        id = item["id"],
                        userName = item["userName"],
                        BorrowerName = item["BorrowerName"],
                        SubjectPropertyAddress = item["SubjectPropertyAddress"],
                        LoanPurpose = item["LoanPurpose"],
                        LoanAmount = item["LoanAmount"],
                        NoteRate = item["NoteRate"],
                        LoanType = item["LoanType"],
                        Milestone = item["Milestone"],
                        ltv = item["ltv"],
                        cltv = item["cltv"],
                        hcltv = item["hcltv"],
                        dti = item["dti"],
                    };
                    draftLoans.Add(draftLoan);
                }
                return draftLoans;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<dynamic> GetLoan(string id)
        {
            try
            {
                var QueryStr = client.CreateDocumentQuery<JObject>(
                    UriFactory.CreateDocumentCollectionUri(cosmos.DatabaseId, cosmos.CollectionId),
                    $"SELECT * FROM c WHERE c.id = '{id}'",
                     new FeedOptions()
                     {
                         EnableCrossPartitionQuery = true,
                         MaxItemCount = 1,
                     })
                    .AsDocumentQuery();

                var result = QueryStr.ExecuteNextAsync<JObject>().Result.ToList();

                //var result = await client.ReadDocumentFeedAsync(UriFactory.CreateDocumentCollectionUri(cosmos.DatabaseId, cosmos.CollectionId),
                //                new FeedOptions { MaxItemCount = 10 });
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateLoan(string id, JObject request)
        {
            try
            {
                await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(cosmos.DatabaseId, cosmos.CollectionId, id), request);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Object> DeleteLoan(string id)
        {
            try
            {
                var QueryInSql = client.CreateDocumentQuery<JObject>(
                    UriFactory.CreateDocumentCollectionUri(cosmos.DatabaseId, cosmos.CollectionId),
                    $"SELECT * FROM Competitions c WHERE c.id = '{id}'",
                     new FeedOptions()
                     {
                         EnableCrossPartitionQuery = true,
                         MaxItemCount = 1,
                     })
                    .AsDocumentQuery();

                var resultPartitionKey = QueryInSql.ExecuteNextAsync<JObject>().Result.ToList();

                var result = await client.DeleteDocumentAsync(
                    UriFactory.CreateDocumentUri(cosmos.DatabaseId, cosmos.CollectionId, id),
                    new RequestOptions()
                    {
                        PartitionKey = new PartitionKey(resultPartitionKey[0]["loanNumber"].ToString())
                    });

                return (dynamic)result.Resource;
            }
            catch (DocumentClientException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return ex.StatusCode.ToString();
            }
        }

        public async Task<string> DeleteCollection(string collectionName)
        {
            try
            {
                await client.DeleteDocumentCollectionAsync(
                    UriFactory.CreateDocumentCollectionUri(cosmos.DatabaseId, collectionName),
                    new RequestOptions
                    {
                        PartitionKey = new PartitionKey(cosmos.PartitionKey)
                    });
                return "Collection deleted successfully";
            }
            catch (DocumentClientException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return ex.StatusCode.ToString();
            }
        }

        public async Task<string> DeleteDatabase(string dbName)
        {
            try
            {
                await client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(dbName));
                return "Database deleted successfully";
            }
            catch (DocumentClientException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return ex.StatusCode.ToString();
            }
        }

    }
}
