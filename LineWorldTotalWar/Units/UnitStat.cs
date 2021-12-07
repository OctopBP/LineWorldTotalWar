using LanguageExt;

namespace LineWorldTotalWar.units {
    public record UnitStat(int cost, int damage, int maxHp, int range, int healPower, char p1, char p2) {
        public static readonly UnitStat warrior = new(15, 4, 9, 1, 0, ']', '[');
        public static readonly UnitStat archer = new(20, 2, 5, 4, 0, '}', '{');
        public static readonly UnitStat cleric = new(30, 1, 3, 6, 3, ')', '(');

        public static Lst<UnitStat> allUnits = new() { warrior, archer, cleric };
    }
}