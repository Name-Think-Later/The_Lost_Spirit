namespace Script.TheLostSpirit.SixValue {
    public record SixValue(
        Effective  @Effective,
        Flux       @Flux,
        Complexity @Complexity,
        Stability  @Stability,
        Existence  @Existence,
        Control    @Control
    );

    public class Control { }

    public class Existence { }

    public class Stability { }

    public class Complexity { }

    public class Flux { }

    public class Effective { }
}