using System.Text.RegularExpressions;
using System.Linq;

namespace AvaliacaoDev.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string CpfCnpj { get; private set; } // CPF/CNPJ
        public string Email { get; private set; }
        public string Telefone { get; private set; }

        public Cliente(string nome, string cpfCnpj, string email, string telefone)
        {
            Id = Guid.NewGuid();

            // Valida tudo antes de criar o cliente
            Validar(nome, email, cpfCnpj, telefone);

            Nome = nome;
            CpfCnpj = cpfCnpj;
            Email = email;
            Telefone = telefone;
        }

        public void Atualizar(string nome, string cpfCnpj, string email, string telefone)
        {
            // Valida tudo antes de atualizar também
            Validar(nome, email, cpfCnpj, telefone);

            Nome = nome;
            CpfCnpj = cpfCnpj;
            Email = email;
            Telefone = telefone;
        }

        /// <summary>
        /// Valida se não existe duplicidade de CPF/CNPJ ou Email
        /// Este método deve ser chamado no service layer antes de criar/atualizar
        /// </summary>
        /// <param name="clientesExistentes">Lista de clientes existentes para comparação</param>
        /// <param name="clienteAtualId">ID do cliente atual (para atualização)</param>
        /// <returns>True se houver duplicidade</returns>
        public bool PossuiDuplicidade(IEnumerable<Cliente> clientesExistentes, Guid? clienteAtualId = null)
        {
            // Remove máscara do CPF/CNPJ para comparar só os números
            var cpfCnpjLimpo = Regex.Replace(CpfCnpj, @"[^\d]", "");
            
            return clientesExistentes.Any(c => 
                c.Id != clienteAtualId && ( // Ignora o próprio cliente na atualização
                    Regex.Replace(c.CpfCnpj, @"[^\d]", "").Equals(cpfCnpjLimpo, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Equals(Email, StringComparison.OrdinalIgnoreCase)
                ));
        }

        private void Validar(string nome, string email, string cpfCnpj, string telefone)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new Exception("Nome é obrigatório");

            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email é obrigatório");

            // Validação de e-mail mais robusta
            if (!IsValidEmail(email))
                throw new Exception("Email inválido");

            if (string.IsNullOrWhiteSpace(cpfCnpj))
                throw new Exception("CPF/CNPJ é obrigatório");

            if (string.IsNullOrWhiteSpace(telefone))
                throw new Exception("Telefone é obrigatório");

            // Validação de telefone brasileiro
            if (!IsValidTelefone(telefone))
                throw new Exception("Telefone inválido. Use o formato (XX) XXXXX-XXXX ou XXXXX-XXXX");

            // Remove caracteres não numéricos para validação
            var documentoLimpo = Regex.Replace(cpfCnpj, @"[^\d]", "");

            if (documentoLimpo.Length == 11)
            {
                // Validar CPF
                if (!ValidarCpf(documentoLimpo))
                    throw new Exception("CPF inválido");
            }
            else if (documentoLimpo.Length == 14)
            {
                // Validar CNPJ
                if (!ValidarCnpj(documentoLimpo))
                    throw new Exception("CNPJ inválido");
            }
            else
            {
                throw new Exception("CPF/CNPJ deve ter 11 dígitos (CPF) ou 14 dígitos (CNPJ)");
            }
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                // Validação mais completa de e-mail
                var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                return emailRegex.IsMatch(email) && email.Length <= 254;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidTelefone(string telefone)
        {
            // Remove caracteres não numéricos
            var telefoneLimpo = Regex.Replace(telefone, @"[^\d]", "");
            
            // Valida formatos brasileiros: (XX) XXXXX-XXXX ou XXXXX-XXXX
            // 10 dígitos (fixo) ou 11 dígitos (celular)
            if (telefoneLimpo.Length == 10 || telefoneLimpo.Length == 11)
            {
                // Verifica se todos os dígitos são iguais (ex: 1111111111)
                return !telefoneLimpo.All(digit => digit == telefoneLimpo[0]);
            }
            
            return false;
        }

        private bool ValidarCpf(string cpf)
        {
            // CPFs inválidos conhecidos - bloqueia sequências fáceis
            string[] cpfsInvalidos = {
                "00000000000", "11111111111", "22222222222", "33333333333",
                "44444444444", "55555555555", "66666666666", "77777777777",
                "88888888888", "99999999999", "12345678909"
            };

            // Verifica se está na lista de CPFs inválidos
            if (Array.Exists(cpfsInvalidos, invalido => invalido == cpf))
                return false;

            // Verifica se todos os dígitos são iguais
            if (cpf.Distinct().Count() < 2)
                return false;

            // Verifica se tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Cálculo dos dígitos verificadores do CPF
            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            // Calcula primeiro dígito verificador
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            // Calcula segundo dígito verificador
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        private bool ValidarCnpj(string cnpj)
        {
            // CNPJs inválidos conhecidos - bloqueia sequências fáceis
            string[] cnpjsInvalidos = {
                "00000000000000", "11111111111111", "22222222222222", "33333333333333",
                "44444444444444", "55555555555555", "66666666666666", "77777777777777",
                "88888888888888", "99999999999999"
            };

            // Verifica se está na lista de CNPJs inválidos
            if (Array.Exists(cnpjsInvalidos, invalido => invalido == cnpj))
                return false;

            // Verifica se todos os dígitos são iguais
            if (cnpj.Distinct().Count() < 2)
                return false;

            // Verifica se tem 14 dígitos
            if (cnpj.Length != 14)
                return false;

            // Cálculo dos dígitos verificadores do CNPJ
            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            // Calcula primeiro dígito verificador
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            // Calcula segundo dígito verificador
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }
    }
}
