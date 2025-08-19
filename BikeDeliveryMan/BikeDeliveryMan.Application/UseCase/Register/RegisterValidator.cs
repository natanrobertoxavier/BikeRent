using BikeDeliveryMan.Communication.Request;
using BikeRent.Exception.ExceptionBase;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BikeDeliveryMan.Application.UseCase.Register;

public class RegisterValidator : AbstractValidator<RegisterDeliveryMan>
{
    public RegisterValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage(ErrorsMessages.BlankName);
        RuleFor(c => c.Email).NotEmpty().WithMessage(ErrorsMessages.BlankEmail);
        RuleFor(c => c.Password).SetValidator(new PasswordValidator());
        When(c => !string.IsNullOrWhiteSpace(c.Email), () =>
        {
            RuleFor(c => c.Email).EmailAddress().WithMessage(ErrorsMessages.InvalidEmail);
        });

        RuleFor(c => c.CNHNumber)
            .NotEmpty().WithMessage(ErrorsMessages.BlankCNHNumber);

        RuleFor(c => c.CNHCategory)
            .NotEmpty().WithMessage("O tipo da CNH não pode ser nulo.")
            .Must(type => type == "A" || type == "B" || type == "AB")
            .WithMessage(ErrorsMessages.CNHCategory);

        RuleFor(c => c.CNPJ)
            .NotEmpty().WithMessage(ErrorsMessages.BlankCNPJ)
            .Must(IsNumberOnly).WithMessage(ErrorsMessages.CNPJOnlyNumbers)
            .Must(IsValidCnpj).WithMessage(ErrorsMessages.InvalidCNPJ);

        RuleFor(c => c.BirthDate)
            .Must(date => date != default)
            .WithMessage(ErrorsMessages.BlankBirthDate);
    }

    private bool IsValidCnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        cnpj = Regex.Replace(cnpj, "[^0-9]", "");

        if (cnpj.Length != 14)
            return false;

        if (new string(cnpj[0], cnpj.Length) == cnpj)
            return false;

        int[] multiplicator1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicator2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCnpj = cnpj.Substring(0, 12);
        int sum = 0;

        for (int i = 0; i < 12; i++)
            sum += int.Parse(tempCnpj[i].ToString()) * multiplicator1[i];

        int remainder = (sum % 11);
        int digit = remainder < 2 ? 0 : 11 - remainder;
        tempCnpj += digit;
        sum = 0;

        for (int i = 0; i < 13; i++)
            sum += int.Parse(tempCnpj[i].ToString()) * multiplicator2[i];

        remainder = (sum % 11);
        digit = remainder < 2 ? 0 : 11 - remainder;

        return cnpj.EndsWith(tempCnpj.Substring(12, 1) + digit.ToString());
    }

    private bool IsNumberOnly(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj) || !Regex.IsMatch(cnpj, @"^\d+$"))
            return false;

        return true;
    }
}