using System;

namespace LineWorldTotalWar {
    public interface IInput {
        int indexToBuy();
    }

    public class AIInput : IInput {
        public int indexToBuy() {
            var rnd = new Random().Next() % 30;
            return rnd;
        }
    }

    public class ConsoleInput : IInput {
        public int indexToBuy() {
            return 0;
        }
    }
}