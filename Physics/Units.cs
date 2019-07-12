using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons.Physics
{
    /// <summary>
    /// How to extend this class:
    /// 1) Add your own units in constructor by calling AddUnit
    /// 2) Set static Effective-property to an instance of your derived class when startin up your application
    /// Pro-tip: For coding convenience add your own units as public static properties, like this class does it.
    /// This will allow you to write code like 13.To(MyUnits.XXXXX).
    /// See sample usage in UnitValueParserTest.CanParseCustomUnit()
    /// </summary>
    public class Units
    {
        public Units()
        {
            inverseStringRepresentationLookup = new Dictionary<string, IUnitDefinition>();
            foreach (var unit in AllUnits)
            {
                AddUnitToStringRepresentationLookup(unit);
            }
        }

        private void AddUnitToStringRepresentationLookup(IUnitDefinition unit)
        {
            inverseStringRepresentationLookup.Add(unit.StringRepresentation, unit);
            foreach (var alternativeStringRepresentation in unit.AlternativeStringRepresentations)
            {
                inverseStringRepresentationLookup.Add(alternativeStringRepresentation, unit);
            }
        }

        public IEnumerable<IUnitDefinition> AllUnits => new[]
            {
                Meter,
                Feet,
                StatuteMile,
                NauticalMile,
                Inches,

                MetersPerSecond,
                FeetPerMinute,
                Knots,
                Mach,

                MetersPerSecondSquared,
                KnotsPerSeond,

                Second,
                Minute,

                Kelvin,
                Celsius,
                Fahrenheit,

                Pascal,
                Bar,
                InchesOfMercury,
                MillimeterOfMercury,
                Torr,

                SquareMeter,

                Liter,
                CubicMeters,

                Kilogram,
                Gram,
                KilogramPerMole,

                Coulombs,
                ElementaryCharge,

                Joule,
                ElectronVolts,

                Newton,

                Radians,
                Degree,

                Mole,

                Molar
            }
            .Concat(CustomUnits);

        private readonly Dictionary<string, IUnitDefinition> inverseStringRepresentationLookup;
        public IReadOnlyDictionary<string, IUnitDefinition> InverseStringRepresentationLookup => inverseStringRepresentationLookup;

        private readonly List<IUnitDefinition> customUnits = new List<IUnitDefinition>();
        private IReadOnlyCollection<IUnitDefinition> CustomUnits => customUnits;

        public void AddUnit(IUnitDefinition unit)
        {
            customUnits.Add(unit);
            AddUnitToStringRepresentationLookup(unit);
        }

        // Distances
        public static readonly IUnitDefinition Meter = new UnitDefinition("m", true, CompoundUnits.Meter, x => x, x => x);
        public static readonly IUnitDefinition Feet = new UnitDefinition("ft", false, CompoundUnits.Meter, x => 0.3048 *x, x => x / 0.3048);
        public static readonly IUnitDefinition StatuteMile = new UnitDefinition("mi", false, CompoundUnits.Meter, x => x * 1609.344, x => x / 1609.344);
        public static readonly IUnitDefinition NauticalMile = new UnitDefinition("NM", false, CompoundUnits.Meter, x => x * 1852, x => x / 1852);
        public static readonly IUnitDefinition Inches = new UnitDefinition("in", false, CompoundUnits.Meter, x => x * 0.0254, x => x / 0.0254);

        // Velocities
        public static readonly IUnitDefinition MetersPerSecond = new UnitDefinition("m/s", true, CompoundUnits.MetersPerSecond, x => x, x => x);
        public static readonly IUnitDefinition FeetPerMinute = new UnitDefinition("ft/min", false, CompoundUnits.MetersPerSecond, x => x * 0.00508, x => x / 0.00508);
        public static readonly IUnitDefinition Knots = new UnitDefinition("kn", false, CompoundUnits.MetersPerSecond, x => x * 0.514444444, x => x / 0.514444444);
        public static readonly IUnitDefinition Mach = new UnitDefinition("mach", false, CompoundUnits.MetersPerSecond, x => x * 340.29, x => x / 340.29);

        // Acceleration
        public static readonly IUnitDefinition MetersPerSecondSquared = new UnitDefinition("m/s^2", true, CompoundUnits.MetersPerSecondSquared, x => x, x => x);
        public static readonly IUnitDefinition KnotsPerSeond = new UnitDefinition("kn/s", false, CompoundUnits.MetersPerSecondSquared, x => x * 0.514444444, x => x / 0.514444444);

        // Time
        public static readonly IUnitDefinition Second = new UnitDefinition("s", true, CompoundUnits.Second, x => x, x => x);
        public static readonly IUnitDefinition Minute = new UnitDefinition("min", false, CompoundUnits.Second, x => x * 60, x => x / 60);

        // Temperature
        public static readonly IUnitDefinition Kelvin = new UnitDefinition("°K", true, CompoundUnits.Kelvin, x => x, x => x, new[] { "K" });
        public static readonly IUnitDefinition Celsius = new UnitDefinition("°C", false, CompoundUnits.Kelvin, x => x + 273.15, x => x - 273.15);
        public static readonly IUnitDefinition Fahrenheit = new UnitDefinition("°F", false, CompoundUnits.Kelvin, x => (x + 459.67) * (5.0 / 9.0), x => x * (9.0/5.0) - 459.67);

        // Pressure
        public static readonly IUnitDefinition Pascal = new UnitDefinition("Pa", true, CompoundUnits.Pascal, x => x, x => x);
        public static readonly IUnitDefinition Bar = new UnitDefinition("bar", false, CompoundUnits.Pascal, x => x * 1e5, x => x * 1e-5);
        public static readonly IUnitDefinition InchesOfMercury = new UnitDefinition("inHg", false, CompoundUnits.Pascal, x => x * 3386.38816, x => x / 3386.38816);
        public static readonly IUnitDefinition MillimeterOfMercury = new UnitDefinition("mmHg", false, CompoundUnits.Pascal, x => x * 133.322387415, x => x / 133.322387415);
        public static readonly IUnitDefinition Torr = new UnitDefinition("Torr", false, CompoundUnits.Pascal, x => x * (101325.0/760), x => x / (101325.0/760));

        // Area
        public static readonly IUnitDefinition SquareMeter = new UnitDefinition("m^2", true, CompoundUnits.SquareMeter, x => x, x => x);

        // Volume
        public static readonly IUnitDefinition CubicMeters = new UnitDefinition("m^3", true, CompoundUnits.CubicMeters, x => x, x => x);
        public static readonly IUnitDefinition Liter = new UnitDefinition("L", false, CompoundUnits.CubicMeters, x => x * 0.001, x => x / 0.001, new[] { "l" });

        // Mass
        public static readonly IUnitDefinition Kilogram = new UnitDefinition("kg", true, CompoundUnits.Kilogram, x => x, x => x);
        public static readonly IUnitDefinition Gram = new UnitDefinition("g", false, CompoundUnits.Kilogram, x => x * 0.001, x => x / 0.001);
        public static readonly IUnitDefinition KilogramPerMole = new UnitDefinition("kg/mol", true, CompoundUnits.KilogramPerMole, x => x, x => x);

        // Charge,
        public static readonly IUnitDefinition Coulombs = new UnitDefinition("C", true, CompoundUnits.Coulombs, x => x, x => x);
        public static readonly IUnitDefinition ElementaryCharge = new UnitDefinition("e", false, CompoundUnits.Coulombs, x => x * 1.60217662 * 1e-19, x => x / (1.60217662 * 1e-19));

        // Energy,
        public static readonly IUnitDefinition Joule = new UnitDefinition("J", true, CompoundUnits.Joule, x => x, x => x);
        public static readonly IUnitDefinition ElectronVolts = new UnitDefinition("eV", false, CompoundUnits.Joule, x => x * 1.60217662 * 1e-19, x => x / (1.60217662 * 1e-19));

        // Force
        public static readonly IUnitDefinition Newton = new UnitDefinition("N", true, CompoundUnits.Newton, x => x, x => x);

        // Angles
        public static readonly IUnitDefinition Radians = new UnitDefinition("rad", true, CompoundUnits.Radians, x => x, x => x);
        public static readonly IUnitDefinition Degree = new UnitDefinition("°", false, CompoundUnits.Radians, x => x * Math.PI / 180, x => 180 * x / Math.PI);

        // Unitless
        public static readonly IUnitDefinition Mole = new UnitDefinition("mol", true, CompoundUnits.Mole, x => x, x => x);

        // Concentration
        public static readonly IUnitDefinition Molar = new UnitDefinition("M", true, CompoundUnits.Molar, x => x, x => x);

        // This must be located at the bottom of the class
        // to ensure all the above fields are initialized when calling new Units()
        public static Units Effective { get; set; } = new Units();
    }
}
