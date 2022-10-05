using DocumentFormat.OpenXml.Wordprocessing;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace Parser.ConsoleApp
{
    public class MedicinalProductModel
    {
        public string TradeName { get; set; }
        public List<string> ChemicalName { get; set; }
        public List<string> ReleaseForm { get; set; }

        public string Manufacturer { get; set; }
        public List<string> ManufacturerCountries { get; set; }

        public string Characteristics { get; set; }
        public RegistrationModel RegistrationCertificate { get; set; }

        public MedicinalProductModel(
            string tradeName, string chemicalName, string releaseForm, string manufacturer,
            string characteristics, string registrationCertificate)
        {
            TradeName = tradeName.Trim(new char[] { '@', '®', ' ' });
            TradeName = TradeName.Replace('®', '_');

            ManufacturerCountries = new();

            Characteristics = characteristics.Trim();

            ChemicalName = new();
            var chemicalNameSplit = chemicalName.Split(new char[] { ',', '+' });
            foreach (var chemical in chemicalNameSplit)
                ChemicalName.Add(chemical.Trim());

            var releaseFormSplit = releaseForm.Split(new char[] { '•' });
            ReleaseForm = new();
            foreach (var form in releaseFormSplit)
                if (!string.IsNullOrEmpty(form))
                    ReleaseForm.Add(form.Replace('\r', ' ').Trim().ToLower());

            Manufacturer = manufacturer.Trim();

            // \w*\((\w+)\)\w*

            Regex regex = new Regex(@"\w*\((\w+\s+\w+|\w+)\)\w*", RegexOptions.IgnoreCase);
            var matches = regex.Matches(Manufacturer);
            foreach (Match match in matches)
            {
                var country = match.Value.Trim(new char[] { '(', ')' });
                ManufacturerCountries.Add(country.ToLower());
            }


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
