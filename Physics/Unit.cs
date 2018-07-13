namespace Commons.Physics
{
    public enum Unit
    {
        Compound = 0,

        // Distances
        Meter = 1,
        Feet = 2,
        StatuteMile = 3,
        NauticalMile = 4,

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

        // Temperature
        Kelvin = 401,
        Celcius = 402,
        Fahrenheit = 403,

        // Pressure
        Pascal = 501,
        Bar = 502,
        InchesOfMercury = 503,

        // Area
        SquareMeter = 601,

        // Volume
        Liter = 701,
        CubicMeters = 702,

        // Mass
        Kilogram = 801,
        Gram = 802,
        GramPerMole = 803,

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
        Degree = 1202
    }
}