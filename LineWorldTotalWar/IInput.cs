using System;
using LanguageExt;

namespace LineWorldTotalWar {
    public interface IInput {
        Eff<Option<Game.ToBuy>> indexToBuy();
    }

    public class AIInput : IInput {
        readonly Random rng;

        public AIInput(Random rng) => this.rng = rng;

        public Eff<Game.ToBuy> indexToBuy() {
            var rnd = rng.Next() % 30;
            return rnd;
        }
    }

    public class ConsoleInput : IInput {
        public Eff<Game.ToBuy> indexToBuy() {
            return 0;
        }
    }
}