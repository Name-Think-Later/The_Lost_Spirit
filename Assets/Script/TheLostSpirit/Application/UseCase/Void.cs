namespace TheLostSpirit.Application.UseCase
{
    public struct Void : IInput, IOutput
    {
        public static Void Default => new Void();
    }
}