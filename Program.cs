// See https://aka.ms/new-console-template for more information
using RaysApps.Services.PlugAndPlayFormat;
using System.Net.Http.Json;
using System.Text.Json;
Dictionary<string, RaysIndexModel> _indexKeyModels = new();
List<RaysIndexModel> _indexModels = new();
HttpClient httpClient = new();
var resp = await httpClient.GetFromJsonAsync<JsonElement>("https://devicemodels.azure.com/index.json");
var models = resp.GetProperty("models");
foreach (var model in models.EnumerateObject())
{
    var indexModel = PnPIndexModel.GetPnPIndexModel(model);
    _indexKeyModels.Add(model.Name, indexModel);
    _indexModels.Add(indexModel);
}
int i = 1;
foreach (var model in _indexKeyModels)
{
    Console.WriteLine($"{i++} -->  {model.Key}");
    Console.WriteLine($"DisplayName: {model.Value.DisplayName}");
}
   