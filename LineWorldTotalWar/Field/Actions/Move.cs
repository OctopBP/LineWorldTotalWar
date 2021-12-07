using LineWorldTotalWar.units;

namespace LineWorldTotalWar.field.actions {
    public record Move(UnitInField unit, Field field) {
        public (UnitInField unit, Field field) execute() {
            if (unit.moved) return (unit, field);

            var allys = from u in field.units
                where u.playerNo == unit.playerNo
                where u.position == unit.position + unit.playerNo.direction()
                select unit;

            if (allys.IsEmpty) {
                var newUnit = unit.move().setMoved();
                return (newUnit, field.clear(unit.position).add(newUnit));
            }

            var allyToSwap = allys.First();

            if (allyToSwap.hp >= unit.hp) {
                var movedUnit = unit.setMoved();
                return (movedUnit, field.clear(unit.position).add(movedUnit));
            }

            var swappedAlly = allyToSwap.setPosition(unit.position);
            var swappedUnit = unit.setPosition(allyToSwap.position).setMoved();

            return (swappedUnit, field.clear(unit.position, allyToSwap.position).add(swappedUnit, swappedAlly));
        }
    }
}