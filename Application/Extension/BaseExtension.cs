namespace Application.Extension;
public static partial class Extensions {
    public static TDist AutoMap<TDist, TSrc> (this TSrc src)
    where TDist : class, new ()
    where TSrc : class, new () {
        var TDistProperties = typeof (TDist).GetProperties ();
        var TSrcProperties = typeof (TSrc).GetProperties ();
        TDist dist = new TDist ();
        foreach (var distProp in TDistProperties) {
            var srcProp = TSrcProperties.SingleOrDefault (x => string.Equals (x.Name, distProp.Name, StringComparison.OrdinalIgnoreCase) && x.PropertyType == distProp.PropertyType);
            if (srcProp == null) {
                continue;
            } else {
                distProp.SetValue (dist, srcProp.GetValue (src));
            }
        }
        return dist;
    }
}