using System;

namespace LineWorldTotalWar.field {
    public record Castle(int hp) {
        public Castle dealDamage(int damage) => this with { hp = Math.Max(hp - damage, 0) };
    }
}