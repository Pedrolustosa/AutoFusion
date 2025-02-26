namespace AutoFusion.Domain.ValueObjects;

public class CPF
{
    public string Value { get; private set; }

    private CPF(string value)
    {
        Value = value;
    }

    public static CPF Create(string cpf)
    {
        if (!IsValid(cpf))
            throw new ArgumentException("Formato de CPF inválido.");

        return new CPF(cpf);
    }

    public static bool IsValid(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = cpf.Replace(".", "").Replace("-", "");

        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            return false;

        return true;
    }

    public bool IsValid() => IsValid(Value); // Agora é um método de instância

    public override bool Equals(object obj)
    {
        return obj is CPF other && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }
}
