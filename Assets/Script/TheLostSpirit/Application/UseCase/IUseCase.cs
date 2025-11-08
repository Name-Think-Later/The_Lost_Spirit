namespace TheLostSpirit.Application.UseCase
{
    public interface IUseCase<out TOutput, in TInput>
        where TOutput : IOutput
        where TInput : IInput
    {
        TOutput Execute(TInput input);
    }

    public interface IInput
    { }

    public interface IOutput
    { }
}