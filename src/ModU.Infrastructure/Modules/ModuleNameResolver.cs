namespace ModU.Infrastructure.Modules;

internal sealed class ModuleNameResolver
{
    public string Resolve(string typeFullName)
    {
        const char dot = '.';
        const int expectedNumberOfDots = 3;
        var fullName = typeFullName.AsSpan();
        int numberOfDots = 0, currentDotIndex = 0, lastDotIndex = 0;

        for (var i = 0; i < fullName.Length; i++)
        {
            if (fullName[i] == dot)
            {
                numberOfDots++;
                lastDotIndex = currentDotIndex;
                currentDotIndex = i;
            }

            if (numberOfDots == expectedNumberOfDots)
            {
                break;
            }
        }

        switch (numberOfDots)
        {
            case expectedNumberOfDots - 1:
                lastDotIndex = currentDotIndex;
                currentDotIndex = fullName.Length;
                break;
            case < expectedNumberOfDots - 1:
                throw new ArgumentException(
                    $"Provided typeFullName: '{typeFullName}' does not match Module Namespace schema.",
                    nameof(typeFullName));
        }


        var startIndex = lastDotIndex + 1;
        var endIndex = currentDotIndex;

        var result = fullName[startIndex..endIndex].ToString();
        return string.IsNullOrEmpty(result)
            ? throw new ArgumentException(
                $"Provided typeFullName: '{typeFullName}' does not match Module Namespace schema.",
                nameof(typeFullName))
            : result;
    }
}