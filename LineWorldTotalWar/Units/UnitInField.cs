using System;

namespace LineWorldTotalWar.units {
    public record UnitInField(UnitStat stat, int position, int hp, PlayerNo playerNo, bool moved = false) {
        public readonly bool canHeal = stat.healPower > 0;
        public readonly bool canBeHealTarget = hp < stat.maxHp;

        public UnitInField move() => this with { position = position + playerNo.direction() };
        public UnitInField dealDamage(int dmg) => this with { hp = Math.Max(hp - dmg, 0) };
        public UnitInField heal(int amount) => this with { hp = Math.Min(hp + amount, stat.maxHp) };
        public UnitInField setPosition(int pos) => this with { position = pos };
        public UnitInField setMoved() => this with { moved = true };

        public string show() => (playerNo == PlayerNo._1 ? stat.p1 : stat.p2).ToString();
    }
}