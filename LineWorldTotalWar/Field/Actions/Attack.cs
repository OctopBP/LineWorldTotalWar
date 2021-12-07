using System;
using System.Linq;
using LineWorldTotalWar.units;

namespace LineWorldTotalWar.field.actions {
    public record Attack(UnitInField unit, Field field) {
        public (UnitInField unit, Field field) execute() {
            if (unit.moved) return (unit, field);

            var enemies = from u in field.units
                where u.playerNo != unit.playerNo
                where Math.Abs(unit.position - u.position) <= unit.stat.range
                orderby u.hp
                select u;

            if (!enemies.Any())
                return (unit, field);

            var movedUnit = unit.setMoved();
            var enemy = enemies
                .First()
                .dealDamage(unit.stat.damage);

            return (movedUnit, field.clear(enemy.position, unit.position).add(enemy, movedUnit));
        }
    }
}