using System;
using LanguageExt;
using LineWorldTotalWar.field;
using LineWorldTotalWar.units;

namespace LineWorldTotalWar {
    class Program {
        static readonly Eff<Game> createGame =
            from rng in Eff<Random>.Effect(() => new())
            let castle1 = new Castle(100)
            let castle2 = new Castle(100)
            let field = new Field(30, Seq<UnitInField>.Empty, castle1, castle2)
            let player1 = new Player(20)
            let player2 = new Player(20)
            let input1 = new AIInput()
            let input2 = new AIInput()
            select new Game(field, player1, player2, input1, input2);

        static void Main(string[] args) {
            createGame
                .Map(game => game.start())
                .Run()
                .Match(
                    game => { Console.WriteLine("Succ"); },
                    err => { Console.WriteLine("Fail"); }
                );
        }
    }
}