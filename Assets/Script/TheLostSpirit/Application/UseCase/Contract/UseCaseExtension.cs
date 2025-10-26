namespace TheLostSpirit.Application.UseCase.Contract
{
    public static class UseCaseExtension
    {
        public static void Execute(this IUseCase<Void, Void> useCase) => useCase.Execute(Void.Default);

        public static T Execute<T>(this IUseCase<T, Void> useCase) where T : IOutput => useCase.Execute(Void.Default);
    }
}