using System;
using LineWorldTotalWar.units;

namespace LineWorldTotalWar.field.actions {
    public record AttackCastle(UnitInField unit, Field field) {
        public (UnitInField unit, Field field) execute() {
            if (unit.moved) return (unit, field);

            if (Math.Abs(unit.position - unit.playerNo.targetPosition(field.size)) > unit.stat.range)
                return (unit, field);

            var targetCastle = unit.playerNo switch {
                PlayerNo._1 => field.castle2,
                PlayerNo._2 => field.castle1,
                _ => throw new ArgumentOutOfRangeException("No castle for this player")
            };

            var hittedCastle = targetCastle.dealDamage(unit.stat.damage);
            var movedUnit = unit.setMoved();

            var newUnits = field.units
                .Filter(u => u.position != unit.position)
                .Add(movedUnit);

            return unit.playerNo switch {
                PlayerNo._1 => (movedUnit, field with {
                    castle2 = hittedCastle,
                    units = newUnits
                }),
                PlayerNo._2 => (movedUnit, field with {
                    castle1 = hittedCastle,
                    units = newUnits
                }),
                _ => throw new ArgumentOutOfRangeException("No castle for this player")
            };
        }
    }
}