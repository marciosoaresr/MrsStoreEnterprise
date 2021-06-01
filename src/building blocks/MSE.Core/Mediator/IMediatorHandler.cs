using System.Threading.Tasks;
using FluentValidation.Results;
using MSE.Core.Messages;

namespace MSE.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;

        Task<ValidationResult> EnviarComando<T>(T comando) where T : Command;
    }
}