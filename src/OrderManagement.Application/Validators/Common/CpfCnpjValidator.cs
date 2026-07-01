namespace OrderManagement.Application.Validators.Common
{
    using System.Linq;

    public static class CpfCnpjValidator
    {
        public static bool IsValidCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = OnlyNumbers(cpf);

            if (cpf.Length != 11)
                return false;

            if (cpf.Distinct().Count() == 1)
                return false;

            var numbers = cpf.Select(c => int.Parse(c.ToString())).ToArray();

            // primeiro dígito
            int sum1 = 0;
            for (int i = 0; i < 9; i++)
                sum1 += numbers[i] * (10 - i);

            int digit1 = (sum1 * 10) % 11;
            if (digit1 == 10) digit1 = 0;

            if (numbers[9] != digit1)
                return false;

            // segundo dígito
            int sum2 = 0;
            for (int i = 0; i < 10; i++)
                sum2 += numbers[i] * (11 - i);

            int digit2 = (sum2 * 10) % 11;
            if (digit2 == 10) digit2 = 0;

            return numbers[10] == digit2;
        }

        public static bool IsValidCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            cnpj = OnlyNumbers(cnpj);

            if (cnpj.Length != 14)
                return false;

            if (cnpj.Distinct().Count() == 1)
                return false;

            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCnpj = cnpj.Substring(0, 12);
            var numbers = tempCnpj.Select(c => int.Parse(c.ToString())).ToArray();

            int sum = 0;

            for (int i = 0; i < 12; i++)
                sum += numbers[i] * multiplicador1[i];

            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

            tempCnpj += digit1;

            numbers = tempCnpj.Select(c => int.Parse(c.ToString())).ToArray();
            sum = 0;

            for (int i = 0; i < 13; i++)
                sum += numbers[i] * multiplicador2[i];

            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            return cnpj.EndsWith($"{digit1}{digit2}");
        }

        public static bool IsValidCpfOrCnpj(string document)
        {
            if (string.IsNullOrWhiteSpace(document))
                return false;

            document = OnlyNumbers(document);

            return document.Length switch
            {
                11 => IsValidCpf(document),
                14 => IsValidCnpj(document),
                _ => false
            };
        }

        private static string OnlyNumbers(string input)
        {
            return new string(input.Where(char.IsDigit).ToArray());
        }
    }
}
