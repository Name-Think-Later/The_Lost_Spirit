using Cysharp.Threading.Tasks;

namespace TheLostSpirit.Application.UseCase
{
    public interface IUseCaseAsync<TOutput, in TInput>
        where TOutput : IOutput
        where TInput : IInput
    {
        UniTask<TOutput> Execute(TInput input);
    }
}