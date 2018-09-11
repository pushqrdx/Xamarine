using Xamarine.Hosting.Swan.Abstractions;

namespace Xamarine.Hosting.Swan
{
    /// <summary>
    ///     Fast, atomioc long combining interlocked to write value and volatile to read values.
    /// </summary>
    public sealed class AtomicLong : AtomicTypeBase<long>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AtomicLong" /> class.
        /// </summary>
        /// <param name="initialValue">if set to <c>true</c> [initial value].</param>
        public AtomicLong(long initialValue = default)
            : base(initialValue)
        {
            // placeholder
        }

        /// <inheritdoc />
        protected override long FromLong(long backingValue)
        {
            return backingValue;
        }

        /// <inheritdoc />
        protected override long ToLong(long value)
        {
            return value;
        }
    }
}
