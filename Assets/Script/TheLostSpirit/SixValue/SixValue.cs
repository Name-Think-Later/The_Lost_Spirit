namespace Script.TheLostSpirit.SixValue {
    public class SixValue {
        public SixValue(
            Effective  effective,
            Flux       flux,
            Complexity complexity,
            Stability  stability,
            Existence  existence,
            Control    control
        ) {
            Effective  = effective;
            Flux       = flux;
            Complexity = complexity;
            Stability  = stability;
            Existence  = existence;
            Control    = control;
        }

        public Effective Effective { get; }
        public Flux Flux { get; }
        public Complexity Complexity { get; }
        public Stability Stability { get; }
        public Existence Existence { get; }
        public Control Control { get; }
    }

    public class Control { }

    public class Existence { }

    public class Stability { }

    public class Complexity { }

    public class Flux { }

    public class Effective { }
}