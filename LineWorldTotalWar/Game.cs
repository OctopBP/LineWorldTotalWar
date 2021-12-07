using System;
using System.Text;
using LineWorldTotalWar.field;
using LineWorldTotalWar.units;

namespace LineWorldTotalWar {
    public record Game(Field field, Player player1, Player player2, IInput player1Input, IInput player2Input) {
        public Game start() {
            var current = this;

            while (current.field.castle1.hp > 0 && current.field.castle2.hp > 0) {
                Console.ReadKey();
                current = current.next();
                Console.WriteLine(current.show());
            }

            return current;
        }

        Game next() =>
            handleInput(player1Input, PlayerNo._1)
                .handleInput(player2Input, PlayerNo._2)
                .move();

        private Game handleInput(IInput input, PlayerNo playerNo) =>
            this with {
                field = input.indexToBuy() switch {
                    0 => field.addUnit(UnitStat.warrior, playerNo),
                    1 => field.addUnit(UnitStat.archer, playerNo),
                    2 => field.addUnit(UnitStat.cleric, playerNo),
                    _ => field
                }
            };

        Game move() => this with { field = field.turn() };

        string show() {
            var sb = new StringBuilder();

            sb.Append("\n");
            sb.Append(field.show());

            return sb.ToString();
        }
    }
}