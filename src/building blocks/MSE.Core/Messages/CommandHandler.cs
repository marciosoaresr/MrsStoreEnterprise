using System.Threading.Tasks;
using FluentValidation.Results;
using MSE.Core.Data;

namespace MSE.Core.Messages
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AdicionarErro(string message)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
        }

        protected async Task<ValidationResult> PersistirDados(IUnitOfWork uow)
        {
            if(!await uow.commit()) AdicionarErro("Houve um erro ao persistir os dados");

            return ValidationResult;
        }

    }
}