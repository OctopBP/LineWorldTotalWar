using System;
using System.Linq;
using LineWorldTotalWar.units;

namespace LineWorldTotalWar.field.actions {
    public record Heal(UnitInField unit, Field field) {
        public (UnitInField unit, Field field) execute() {
            if (unit.moved) return (unit, field);
            if (!unit.canHeal) return (unit, field);

            var allies = from u in field.units
                where u.playerNo == unit.playerNo
                where u.position != unit.position
                where Math.Abs(unit.position - u.position) <= unit.stat.range
                where u.canBeHealTarget
                orderby u.hp
                select u;

            if (!allies.Any())
                return (unit, field);

            var movedUnit = unit.setMoved();
            var ally = allies
                .First()
                .heal(unit.stat.healPower);

            return (movedUnit, field.clear(ally.position, unit.position).add(ally, movedUnit));
        }
    }
}