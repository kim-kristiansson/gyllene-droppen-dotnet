using System.Text.RegularExpressions;
using GylleneDroppen.Application.Dtos.Shared.Auth;

namespace GylleneDroppen.Blazor.Client.Validators;

public partial class RegisterValidator(RegisterRequest model)
{
    private readonly Dictionary<string, List<string>> _errors = new();

    public bool Validate()
    {
        _errors.Clear();

        ValidateFirstName();
        ValidateLastName();
        ValidateEmail();
        ValidatePassword();
        ValidateConfirmPassword();

        return _errors.Count == 0;
    }

    private void ValidateFirstName()
    {
        if (string.IsNullOrWhiteSpace(model.FirstName))
            AddError(nameof(model.FirstName), "Förnamn är obligatoriskt.");
        else if (model.FirstName.Length > 100)
            AddError(nameof(model.FirstName), "Förnamn får inte vara längre än 100 tecken.");
    }

    private void ValidateLastName()
    {
        if (string.IsNullOrWhiteSpace(model.LastName))
            AddError(nameof(model.LastName), "Efternamn är obligatoriskt.");
        else if (model.LastName.Length > 100)
            AddError(nameof(model.LastName), "Efternamn får inte vara längre än 100 tecken.");
    }

    private void ValidateEmail()
    {
        if (string.IsNullOrWhiteSpace(model.Email))
        {
            AddError(nameof(model.Email), "E-postadress är obligatorisk.");
            return;
        }

        // Simple email validation regex
        const string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        if (!MyRegex().IsMatch(model.Email))
            AddError(nameof(model.Email), "E-postadressen har inte ett giltigt format.");
    }

    private void ValidatePassword()
    {
        if (string.IsNullOrWhiteSpace(model.Password))
        {
            AddError(nameof(model.Password), "Lösenord är obligatoriskt.");
            return;
        }

        if (model.Password.Length < 6) AddError(nameof(model.Password), "Lösenordet måste vara minst 6 tecken långt.");
    }

    private void ValidateConfirmPassword()
    {
        if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
        {
            AddError(nameof(model.ConfirmPassword), "Bekräfta lösenord är obligatoriskt.");
            return;
        }

        if (model.Password != model.ConfirmPassword)
            AddError(nameof(model.ConfirmPassword), "Lösenorden matchar inte.");
    }

    private void AddError(string property, string message)
    {
        if (!_errors.TryGetValue(property, out var value))
        {
            value = [];
            _errors[property] = value;
        }

        value.Add(message);
    }

    public List<string> GetErrors(string propertyName)
    {
        return _errors.TryGetValue(propertyName, out var value) ? value : [];
    }

    public bool HasErrors(string propertyName)
    {
        return _errors.ContainsKey(propertyName) && _errors[propertyName].Count != 0;
    }

    public Dictionary<string, List<string>> GetAllErrors()
    {
        return _errors;
    }

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    private static partial Regex MyRegex();
}