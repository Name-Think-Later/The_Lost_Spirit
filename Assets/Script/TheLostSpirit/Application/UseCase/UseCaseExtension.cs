namespace TheLostSpirit.Application.UseCase {
    public static class UseCaseExtension {
        public static void Execute(this IUseCase<Void, Void> useCase) => useCase.Execute(Void.Default);

        public static IOutput Execute(this IUseCase<IOutput, Void> useCase) => useCase.Execute(Void.Default);
    }
}