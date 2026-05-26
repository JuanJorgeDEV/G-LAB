using System.Security.Cryptography;
using System.Text;

namespace ProjetoOS.Models;

public static class PasswordHelper
{
    private const int Iterations = 100_000;
    private const int SaltSize = 16;
    private const int KeySize = 32;

    public static string CriarHash(string senha)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(senha),
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize);

        return $"PBKDF2${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public static bool Verificar(string senha, string senhaSalva)
    {
        if (!senhaSalva.StartsWith("PBKDF2$", StringComparison.Ordinal))
        {
            return senha == senhaSalva;
        }

        var partes = senhaSalva.Split('$');
        if (partes.Length != 4 || !int.TryParse(partes[1], out var iteracoes))
        {
            return false;
        }

        var salt = Convert.FromBase64String(partes[2]);
        var hashSalvo = Convert.FromBase64String(partes[3]);
        var hashDigitado = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(senha),
            salt,
            iteracoes,
            HashAlgorithmName.SHA256,
            hashSalvo.Length);

        return CryptographicOperations.FixedTimeEquals(hashDigitado, hashSalvo);
    }

    public static string GerarSenhaInicial(string nome, string? cpf)
    {
        var primeiroNome = nome.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "usuario";
        var cpfNumeros = ApenasNumeros(cpf);
        return $"{RemoverAcentos(primeiroNome).ToLowerInvariant()}{cpfNumeros}";
    }

    public static string ApenasNumeros(string? texto)
    {
        return new string((texto ?? string.Empty).Where(char.IsDigit).ToArray());
    }

    private static string RemoverAcentos(string texto)
    {
        var normalizado = texto.Normalize(NormalizationForm.FormD);
        var chars = normalizado.Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark);
        return new string(chars.ToArray()).Normalize(NormalizationForm.FormC);
    }
}
