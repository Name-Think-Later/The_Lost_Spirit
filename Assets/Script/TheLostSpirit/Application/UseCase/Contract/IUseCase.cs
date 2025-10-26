namespace TheLostSpirit.Application.UseCase.Contract {
    public interface IUseCase<out T, in U>
        where T : IOutput
        where U : IInput {
        T Execute(U input);
    }

    public interface IInput { }

    public interface IOutput { }
}