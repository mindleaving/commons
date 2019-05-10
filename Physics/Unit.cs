namespace Commons.Physics
{
    public enum Unit
    {
        None = -1,

        Compound = 0,

        // Distances
        Meter = 1,
        Feet = 2,
        StatuteMile = 3,
        NauticalMile = 4,
        Inches = 5,

        // Velocities
        MetersPerSecond = 101,
        FeetPerMinute = 102,
        Knots = 103,
        Mach = 104,

        // Acceleration
        MetersPerSecondSquared = 201,
        KnotsPerSeond = 202,

        // Time
        Second = 301,
        Minute = 302,

        // Temperature
        Kelvin = 401,
        Celsius = 402,
        Fahrenheit = 403,

        // Pressure
        Pascal = 501,
        Bar = 502,
        InchesOfMercury = 503,
        MillimeterOfMercury = 504,
        Torr = 505,

        // Area
        SquareMeter = 601,

        // Volume
        Liter = 701,
        CubicMeters = 702,

        // Mass
        Kilogram = 801,
        Gram = 802,
        KilogramPerMole = 803,

        // Charge,
        Coulombs = 901,
        ElementaryCharge = 902,

        // Energy,
        Joule = 1001,
        ElectronVolts = 1002,

        // Force
        Newton = 1101,

        // Angles
        Radians = 1201,
        Degree = 1202,

        // Unitless
        Mole = 1301,

        // Concentration
        Molar = 1401,
    }
}