using System;
using System.Text;
using LanguageExt;
using LineWorldTotalWar.field;
using LineWorldTotalWar.units;
using static LanguageExt.Prelude;

namespace LineWorldTotalWar {
    public record Game(Field field, Player player1, Player player2) {
        public bool canPlay => field.castle1.hp > 0 && field.castle2.hp > 0;
        
        public Eff<Game> start(IInput player1Input, IInput player2Input) {
            return 
                canPlay ?
                iteration(this).Bind(newGame => newGame.start(player1Input, player2Input))
                : LanguageExt.Eff<Game>.Success(this);
            
            Eff<Game> iteration(Game current) =>
                from key in Eff(Console.ReadKey)
                from newGame in current.next(player1Input, player2Input)
                let newGameStr = newGame.show()
                from _ in Eff(() => {
                    Console.WriteLine(newGameStr);
                    return unit;
                })
                select newGame;
        }

        public Eff<Game> next(IInput player1Input, IInput player2Input) =>
            handleInput(player1Input, PlayerNo._1)
                .Bind(_ => _.handleInput(player2Input, PlayerNo._2))
                .Map(_ => _.move());

        public Eff<Game> handleInput(IInput input, PlayerNo playerNo) =>
            input.indexToBuy().Map(maybeIndex => handleInput(maybeIndex, playerNo));

        public enum ToBuy {
            Warrior, Archer, Cleric
        }

        public Game handleInput(Option<ToBuy> maybeIndex, PlayerNo playerNo) =>
            maybeIndex.Match(
                Some: index => handleInput(index, playerNo),
                None: () => this
            );
        
        public Game handleInput(ToBuy index, PlayerNo playerNo) =>
            this with {
                field = index switch {
                    ToBuy.Warrior => field.addUnit(UnitStat.warrior, playerNo),
                    ToBuy.Archer => field.addUnit(UnitStat.archer, playerNo),
                    ToBuy.Cleric => field.addUnit(UnitStat.cleric, playerNo),
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