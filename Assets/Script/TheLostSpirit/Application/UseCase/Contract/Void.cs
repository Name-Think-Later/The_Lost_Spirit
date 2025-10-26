namespace TheLostSpirit.Application.UseCase.Contract {
    public struct Void : IInput, IOutput {
        public static Void Default => new Void();
    }
}