using System.Net;
using MCA_ReceiptScanner.Models;
using Newtonsoft.Json;

var json = new WebClient().DownloadString("https://interview-task-api.mca.dev/qr-scanner-codes/alpha-qr-gFpwhsQ8fkY1");

var receipt = JsonConvert.DeserializeObject<IEnumerable<Receipt>>(json);

if (receipt != null && receipt.Any())
{
    List<Receipt> domesticItems = new List<Receipt>();
    List<Receipt> importedItems = new List<Receipt>();

    foreach (var item in receipt)
    {
        if (item.Domestic)
        {
            domesticItems.Add(item);
        }
        else
        {
            importedItems.Add(item);
        }
    }

    if (domesticItems.Any())
    {
        domesticItems = domesticItems.OrderBy(x => x.Name).ToList();
        Console.WriteLine(".Domestic");
        foreach (var item in domesticItems)
        {
            Console.WriteLine($"\t...{item.Name}");
            Console.WriteLine($"\t Price: ${item.Price}");
            Console.WriteLine($"\t {item.Description.Substring(0, 10)}...");

            var domesticWeight = item.Weight != null ? $"{item.Weight}g" : item.Weight != null ? $"{item.Weight}g" : "N/A";
            Console.WriteLine($"\t Weight: {domesticWeight}");
        }

    }

    if (importedItems.Any())
    {
        importedItems = importedItems.OrderBy(x => x.Name).ToList();
        Console.WriteLine(".Imported");
        foreach (var item in importedItems)
        {
            Console.WriteLine($"\t...{item.Name}");
            Console.WriteLine($"\t Price: ${item.Price}");
            Console.WriteLine($"\t {item.Description.Substring(0, 10)}...");

            var importedWeight = item.Weight != null ? $"{item.Weight}g" : item.Weight != null ? $"{item.Weight}g" : "N/A";
            Console.WriteLine($"\t Weight: {importedWeight}");
            Console.WriteLine(); // as <br />
        }
    }

    var domesticTotalPrice = domesticItems.Sum(x => Convert.ToDecimal(x.Price));
    var domesticGeneric = domesticItems.Count == 1 ? $"${domesticTotalPrice}.0" : domesticItems.Count > 0 ? $"${domesticTotalPrice}" : $"${domesticTotalPrice}.0";
    Console.WriteLine($"Domestic cost: {domesticGeneric}");

    var importedTotalPrice = importedItems.Sum(x => Convert.ToDecimal(x.Price));
    var importedGeneric = importedItems.Count == 1 ? $"${importedTotalPrice}.0" : importedItems.Count > 0 ? $"${importedTotalPrice}" : $"${importedTotalPrice}.0";
    Console.WriteLine($"Imported cost: {importedGeneric}");

    Console.WriteLine($"Domestic count: {domesticItems.Count()}");
    Console.WriteLine($"Imported count: {importedItems.Count()}");
}