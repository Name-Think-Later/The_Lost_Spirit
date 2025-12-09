namespace TheLostSpirit.Application.UseCase
{
    public static class UseCaseExtension
    {
        public static void Execute(this IUseCase<Void, Void> useCase) {
            useCase.Execute(Void.Default);
        }

        public static T Execute<T>(this IUseCase<T, Void> useCase) where T : IOutput {
            return useCase.Execute(Void.Default);
        }
    }
}