namespace Application.Extension;
public static partial class Extensions {
    public static TDist AutoMap<TDist, TSrc> (this TSrc src)
    where TDist : class, new ()
    where TSrc : class, new () {
        var TDistProperties = typeof (TDist).GetProperties ();
        var TSrcProperties = typeof (TSrc).GetProperties ();
        TDist dist = new TDist ();
        foreach (var prop in TDistProperties) {
            var prop2 = TSrcProperties.SingleOrDefault (x => string.Equals (x.Name, prop.Name, StringComparison.OrdinalIgnoreCase) && x.PropertyType == prop.PropertyType);
            if (prop2 == null) {
                continue;
            } else {
                prop2.SetValue (dist, prop.GetValue (src));
            }
        }
        return dist;
    }
}