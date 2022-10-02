using ClosedXML.Excel;
using System.Text;

namespace Parser.ConsoleApp
{
    public static class ParserWorker
    {
        public static List<MedicinalProductModel> Parse()
        {
            var pathToFile = Path.Combine(AppContext.BaseDirectory, "files", "exported_14-09-2022.xlsx");
            var workbook = new XLWorkbook(pathToFile);
            var ws1 = workbook.Worksheet(1);

            List<MedicinalProductModel> models = new();

            var currentRow = 2;
            var need = true;
            while (need)
            {
                var tradeName = ws1.Cell(currentRow, 1).GetValue<string>();
                var chemicalName = ws1.Cell(currentRow, 2).GetValue<string>();
                var releaseForm = ws1.Cell(currentRow, 3).GetValue<string>();
                var manufacturer = ws1.Cell(currentRow, 4).GetValue<string>();
                var characteristics = ws1.Cell(currentRow, 5).GetValue<string>();
                var registrationCertificate = ws1.Cell(currentRow, 6).GetValue<string>();

                if (string.IsNullOrEmpty(releaseForm))
                {
                    currentRow++;

                    releaseForm = ws1.Cell(currentRow, 2).GetValue<string>();
                    manufacturer = ws1.Cell(currentRow, 3).HasFormula ? ws1.Cell(currentRow, 3).FormulaA1 : ws1.Cell(currentRow, 3).GetValue<string>();
                    characteristics = ws1.Cell(currentRow, 4).GetValue<string>();
                    registrationCertificate = ws1.Cell(currentRow, 5).GetValue<string>();
                }

                if (!string.IsNullOrEmpty(tradeName))
                    models.Add(new MedicinalProductModel(tradeName, chemicalName, releaseForm, manufacturer, characteristics, registrationCertificate));

                currentRow++;

                if (string.IsNullOrEmpty(tradeName) && string.IsNullOrEmpty(chemicalName) && string.IsNullOrEmpty(releaseForm)
                    && string.IsNullOrEmpty(manufacturer) && string.IsNullOrEmpty(characteristics) && string.IsNullOrEmpty(registrationCertificate))
                    need = false;
            }

            StringBuilder sb = new();
            var g = models.Select(x => x.Characteristics);
            foreach (var item in g) sb.AppendLine(item);
            var t = sb.ToString();

            return models;
        }
    }
}
