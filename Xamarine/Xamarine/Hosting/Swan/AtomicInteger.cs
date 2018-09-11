using System;
using Xamarine.Hosting.Swan.Abstractions;

namespace Xamarine.Hosting.Swan
{
    /// <summary>
    ///     Represents an atomically readabl;e or writable integer.
    /// </summary>
    public class AtomicInteger : AtomicTypeBase<int>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AtomicInteger" /> class.
        /// </summary>
        /// <param name="initialValue">if set to <c>true</c> [initial value].</param>
        public AtomicInteger(int initialValue = default)
            : base(Convert.ToInt64(initialValue))
        {
            // placeholder
        }

        /// <inheritdoc />
        protected override int FromLong(long backingValue)
        {
            return Convert.ToInt32(backingValue);
        }

        /// <inheritdoc />
        protected override long ToLong(int value)
        {
            return Convert.ToInt64(value);
        }
    }
}
