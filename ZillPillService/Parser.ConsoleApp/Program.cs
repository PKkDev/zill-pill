using Parser.ConsoleApp;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var result = ParserWorker.Parse();

Console.ReadKey();
