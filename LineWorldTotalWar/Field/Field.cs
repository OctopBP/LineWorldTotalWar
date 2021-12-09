using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using LineWorldTotalWar.field.actions;
using LineWorldTotalWar.units;

namespace LineWorldTotalWar.field {
    public record Field(int size, Seq<UnitInField> units, Castle castle1, Castle castle2) {
        public Field addUnit(UnitStat unitStat, PlayerNo playerNo) {
            var newUnit = new UnitInField(unitStat, playerNo.spawnPosition(size), unitStat.maxHp, playerNo);
            return add(newUnit);
        }

        public Field turn() => next();

        Option<Field> step(Field current) => current.unitMoves().Last();

        Field unitTurn(UnitInField unit, Field field) {
            var (unit0, field0) = new Heal(unit, field).execute();
            var (unit1, field1) = new Attack(unit0, field0).execute();
            var field2 = field1.removeDeadBodies();
            var (unit3, field3) = new AttackCastle(unit1, field2).execute();
            var (unit4, field4) = new Move(unit3, field3).execute();

            return field4;
        }

        IEnumerable<Field> unitMoves() {
            var current = this with { units = units.Map(u => u with { moved = false }) };

            if (!current.units.Any(u => !u.moved))
                yield return current;

            while (current.units.Any(u => !u.moved)) {
                var unit = current.units
                    .Where(u => !u.moved)
                    .OrderBy(u => MathF.Abs(u.position - u.playerNo.spawnPosition(size)))
                    .First();

                current = unitTurn(unit, current);

                yield return current;
            }
        }

        Field next() {
            var next = step(this);
            return next.IsSome ? next.ValueUnsafe() : this;
        }

        Field removeDeadBodies() {
            return this with { units = units.Filter(u => u.hp > 0) };
        }

        public Field clear(params int[] posToClear) {
            return this with { units = units.Filter(u => !posToClear.Contains(u.position)) };
        }

        public Field add(params UnitInField[] unitsToAdd) {
            return this with { units = units.Concat(unitsToAdd) };
        }

        public string show() {
            var line1 = new StringBuilder();
            var line2 = new StringBuilder();

            line1.Append("###");
            line2.Append($"{castle1.hp:D3}");

            for (int i = 0; i < size; i++) {
                var unit = units.FirstOrDefault(u => u.position == i);

                if (unit != null) {
                    line1.Append(unit.show());
                    line2.Append(unit.hp);
                }
                else {
                    line1.Append('.');
                    line2.Append(' ');
                }
            }

            line1.Append("###");
            line2.Append($"{castle2.hp:D3}");

            line1.Append('\n');
            line1.Append(line2);

            return line1.ToString();
        }
    }
}