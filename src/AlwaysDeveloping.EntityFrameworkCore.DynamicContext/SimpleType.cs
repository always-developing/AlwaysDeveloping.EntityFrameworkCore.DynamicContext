namespace AlwaysDeveloping.EntityFrameworkCore.DynamicContext
{
    /// <summary>
    /// A wrapper class used so simple types can be used to query the database
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class SimpleType<TValue>
    {
        /// <summary>
        /// The value type
        /// </summary>
        public TValue Value { get; set; }
    }
}
