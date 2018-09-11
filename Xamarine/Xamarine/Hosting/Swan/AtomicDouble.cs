using System;
using Xamarine.Hosting.Swan.Abstractions;

namespace Xamarine.Hosting.Swan
{
    /// <summary>
    ///     Fast, atomic double combining interlocked to write value and volatile to read values.
    /// </summary>
    public sealed class AtomicDouble : AtomicTypeBase<double>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AtomicDouble" /> class.
        /// </summary>
        /// <param name="initialValue">if set to <c>true</c> [initial value].</param>
        public AtomicDouble(double initialValue = default)
            : base(BitConverter.DoubleToInt64Bits(initialValue))
        {
            // placeholder
        }

        /// <inheritdoc />
        protected override double FromLong(long backingValue)
        {
            return BitConverter.Int64BitsToDouble(backingValue);
        }

        /// <inheritdoc />
        protected override long ToLong(double value)
        {
            return BitConverter.DoubleToInt64Bits(value);
        }
    }
}
