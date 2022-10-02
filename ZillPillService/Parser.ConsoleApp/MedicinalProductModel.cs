using System.Reflection.PortableExecutable;

namespace Parser.ConsoleApp
{
    public class MedicinalProductModel
    {
        public string TradeName { get; set; }
        public List<string> ChemicalName { get; set; }
        public List<string> ReleaseForm { get; set; }
        public string Manufacturer { get; set; }
        public string Characteristics { get; set; }
        public RegistrationModel RegistrationCertificate { get; set; }

        public MedicinalProductModel(
            string tradeName, string chemicalName, string releaseForm, string manufacturer,
            string characteristics, string registrationCertificate)
        {
            TradeName = tradeName.Trim(new char[] { '®', ' ' });


            // Вид:
            // Доп.признаки:
            // Включен в список контролируемых:
            // Присвоен статус орфанного
            // Не присвоен статус орфанного
            //if (characteristics.Contains("Вид:"))
            //{
            //    var index2 = characteristics.IndexOf("Вид: ");
            //    var index23 = characteristics.IndexOf(":", index2 + 5);
            //    var str = characteristics.Substring(index2, index23);
            //}
            //if (characteristics.Contains("Доп.Признаки:"))
            //{
            //    var index2 = characteristics.IndexOf("Доп.Признаки:");
            //    var index23 = characteristics.IndexOf(":", index2 + 13);
            //    var str = characteristics.Substring(index2, index23);
            //}
            //var characteristicsSplit = characteristics.Split(": ");
            //Characteristics = new();
            //var index = 0;
            //for (int i = 0; i < characteristicsSplit.Length / 2; i++)
            //{
            //    Characteristics.Add($"{characteristicsSplit[index].Trim()} : {characteristicsSplit[index + 1].Trim()}");
            //    index += 2;
            //}
            Characteristics = characteristics.Trim();

            ChemicalName = new();
            var chemicalNameSplit = chemicalName.Split(new char[] { ',', '+' });
            foreach (var chemical in chemicalNameSplit)
                ChemicalName.Add(chemical.Trim());

            var releaseFormSplit = releaseForm.Split(new char[] { '•' });
            ReleaseForm = new();
            foreach (var form in releaseFormSplit)
                if (!string.IsNullOrEmpty(form))
                    ReleaseForm.Add(form.Replace('\r', ' ').Trim());

            Manufacturer = manufacturer.Trim();

            if (string.IsNullOrEmpty(registrationCertificate))
                RegistrationCertificate = new RegistrationModel();
            else
            {
                var sp = registrationCertificate.Trim().Split("от");
                var license = sp.First().Trim();

                var sp2 = sp.Last().Trim().Split(' ');
                var registerDate = Convert.ToDateTime(sp2.First());
                var approved = sp2.Last();

                RegistrationCertificate = new RegistrationModel(license, registerDate, approved);
            }
        }
    }

    public class RegistrationModel
    {
        public string License { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Approved { get; set; }

        public RegistrationModel() { }

        public RegistrationModel(
            string license, DateTime registerDate, string approved)
        {
            License = license;
            RegisterDate = registerDate;
            Approved = approved;
        }
    }
}
