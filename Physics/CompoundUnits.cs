namespace Commons.Physics
{
    public static class CompoundUnits
    {
        public static CompoundUnit None { get; } = new CompoundUnit();
        public static CompoundUnit Meter { get; } = new CompoundUnit(new[] {SIBaseUnit.Meter});
        public static CompoundUnit MetersPerSecond { get; } = new CompoundUnit(new[] {SIBaseUnit.Meter}, new[] {SIBaseUnit.Second});
        public static CompoundUnit MetersPerSecondSquared { get; } = new CompoundUnit(new[] {SIBaseUnit.Meter}, new[] {SIBaseUnit.Second, SIBaseUnit.Second});
        public static CompoundUnit Second { get; } = new CompoundUnit(new[] {SIBaseUnit.Second});
        public static CompoundUnit Kelvin { get; } = new CompoundUnit(new[] {SIBaseUnit.Kelvin});
        public static CompoundUnit Pascal { get; } = new CompoundUnit(new[] {SIBaseUnit.Kilogram}, new[] {SIBaseUnit.Meter, SIBaseUnit.Second, SIBaseUnit.Second});
        public static CompoundUnit SquareMeter { get; } = new CompoundUnit(new[] {SIBaseUnit.Meter, SIBaseUnit.Meter});
        public static CompoundUnit CubicMeters { get; } = new CompoundUnit(new[] {SIBaseUnit.Meter, SIBaseUnit.Meter, SIBaseUnit.Meter});
        public static CompoundUnit Kilogram { get; } = new CompoundUnit(new[] {SIBaseUnit.Kilogram});
        public static CompoundUnit KilogramPerMole { get; } = new CompoundUnit(new[] {SIBaseUnit.Kilogram}, new[] {SIBaseUnit.Mole});
        public static CompoundUnit Coulombs { get; } = new CompoundUnit(new[] {SIBaseUnit.Ampere, SIBaseUnit.Second});
        public static CompoundUnit Joule { get; } = new CompoundUnit(new[] {SIBaseUnit.Kilogram, SIBaseUnit.Meter, SIBaseUnit.Meter}, new[] {SIBaseUnit.Second, SIBaseUnit.Second});
        public static CompoundUnit Newton { get; } = new CompoundUnit(new[] {SIBaseUnit.Kilogram, SIBaseUnit.Meter}, new[] {SIBaseUnit.Second, SIBaseUnit.Second});
        public static CompoundUnit Radians { get; } = new CompoundUnit(new[] {SIBaseUnit.Radians});
        public static CompoundUnit Mole { get; } = new CompoundUnit(new[] {SIBaseUnit.Mole});
        public static CompoundUnit Molar { get; } = new CompoundUnit(new[] {SIBaseUnit.Mole}, new[] {SIBaseUnit.Meter, SIBaseUnit.Meter, SIBaseUnit.Meter});
    }
}