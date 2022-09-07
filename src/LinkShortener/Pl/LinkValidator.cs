using FluentValidation;
using LinkShortener.Pl.Models;
using System.Text.RegularExpressions;

namespace LinkShortener.Pl
{
    public class LinkValidator : AbstractValidator<CreateShortUrlRequest>
    {
        // todo: ask about possible attributes
        readonly Regex allowedChars = new Regex("^[A-Za-z0-9-._~:/?#]*$");
        public LinkValidator(int maxLength = 200)
        {
            RuleFor(CreateShortUrlRequest => CreateShortUrlRequest.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(maxLength)
                .MinimumLength(4)
                .Matches(allowedChars);
        }
    }
}
