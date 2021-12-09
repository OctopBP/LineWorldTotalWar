using System;
using System.Linq;
using LanguageExt;
using LineWorldTotalWar.units;

namespace LineWorldTotalWar.field.actions {
    public record Heal(UnitInField unit, Field field) {
        public static State<Field, UnitInField> heal(UnitInField unit) => Prelude.State((Field field) => {
            if (unit.moved) return (unit, field);
            if (!unit.canHeal) return (unit, field);

            var allies = from u in field.units
                where u.playerNo == unit.playerNo
                where u.position != unit.position
                where Math.Abs(unit.position - u.position) <= unit.stat.range
                where u.canBeHealTarget
                orderby u.hp
                select u;

            return allies.HeadOrNone().Match(
                Some: targetAlly => {
                    var ally = targetAlly.heal(unit.stat.healPower);
                    var movedUnit = unit.setMoved();
                    return (movedUnit, field.clear(ally.position, unit.position).add(ally, movedUnit));
                },
                None: () => (unit, field)
            );
        });

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

            return allies.HeadOrNone().Match(
                Some: targetAlly => {
                    var ally = targetAlly.heal(unit.stat.healPower);
                    var movedUnit = unit.setMoved();
                    return (movedUnit, field.clear(ally.position, unit.position).add(ally, movedUnit));
                },
                None: () => (unit, field)
            );
        }
    }
}