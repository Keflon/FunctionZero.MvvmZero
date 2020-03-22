using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.MvvmZero.Commanding
{
    /// <summary>
    /// A Builder for a CommandZeroAsync instance
    /// </summary>
    public class CommandBuilder
    {
        private Func<object, Task> _execute;
        private Func<object, bool> _predicate;
        private IList<IGuard> _guardList;
        private Func<string> _getName;
        private bool _hasBuilt;
        private IDictionary<INotifyPropertyChanged, HashSet<string>> _observedProperties;

        /// <summary>
        /// CommandBuilder ctor
        /// </summary>
        public CommandBuilder()
        {
            _guardList = new List<IGuard>();
            _observedProperties = new Dictionary<INotifyPropertyChanged, HashSet<string>>();
        }
        /// <summary>
        /// Build the Command! :)
        /// </summary>
        /// <returns></returns>
        public CommandZeroAsync Build()
        {
            if (_hasBuilt)
                throw new InvalidOperationException("This CommandBuilder has expired. You cannot call Build more than once.");
            _hasBuilt = true;
            return new CommandZeroAsync(_guardList, _execute, _predicate, _getName, _observedProperties);
        }

        /// <summary>
        /// Set an asynchronous Execute callback that requires a parameter
        /// </summary>
        /// <param name="execute">An asynchronous Execute callback that requires a parameter</param>
        /// <returns></returns>
        public CommandBuilder SetExecute(Func<object, Task> execute)
        {
            if (_execute != null)
                throw new NotSupportedException("SetExecute cannot be called more than once");
            _execute = execute;
            return this;
        }

        /// <summary>
        /// Set an asynchronous Execute callback that does not require a parameter
        /// </summary>
        /// <param name="execute">An asynchronous Execute callback that requires a parameter</param>
        /// <returns></returns>
        public CommandBuilder SetExecute(Func<Task> execute)
        {
            if (_execute != null)
                throw new NotSupportedException("SetExecute cannot be called more than once");
            _execute = (o) => execute();
            return this;
        }

        /// <summary>
        /// Set a synchonous Execute callback that does not require a parameter
        /// </summary>
        /// <param name="execute">A synchonous Execute callback that does not require a parameter</param>
        /// <returns></returns>
        public CommandBuilder SetExecute(Action execute)
        {
            if (_execute != null)
                throw new NotSupportedException("SetExecute cannot be called more than once");
            _execute = (o) => { execute(); return Task.CompletedTask; };
            return this;
        }

        /// <summary>
        /// Set a synchonous Execute callback that requires a parameter
        /// </summary>
        /// <param name="execute">A synchonous Execute callback that requires a parameter</param>
        /// <returns></returns>
        public CommandBuilder SetExecute(Action<object> execute)
        {
            if (_execute != null)
                throw new NotSupportedException("SetExecute cannot be called more than once");
            _execute = (o) => { execute(o); return Task.CompletedTask; };
            return this;
        }

        /// <summary>
        /// Set a CanExecute callback that requires a parameter
        /// </summary>
        /// <param name="canExecute">A CanExecute callback that requires a parameter</param>
        /// <returns></returns>
        public CommandBuilder SetCanExecute(Func<object, bool> canExecute)
        {
            if (_predicate != null)
                throw new NotSupportedException("SetCanExecute cannot be called more than once");
            _predicate = canExecute;
            return this;
        }

        /// <summary>
        /// Set a CanExecute callback that does not require a parameter
        /// </summary>
        /// <param name="canExecute">A CanExecute callback that does not require a parameter</param>
        /// <returns></returns>
        public CommandBuilder SetCanExecute(Func<bool> canExecute)
        {
            if (_predicate != null)
                throw new NotSupportedException("SetCanExecute cannot be called more than once");
            _predicate = (o) => canExecute();
            return this;
        }

        /// <summary>
        /// Applies a global guard object to the resultant ICommand
        /// Async Commands that share this global guard cannot execute concurrently
        /// Commands can be given multiple guard implementations, though individual guard implementations
        /// can only be added once
        /// </summary>
        /// <returns></returns>
        public CommandBuilder AddGlobalGuard()
        {
            if (_guardList.Contains(GlobalGuard))
                throw new ArgumentException("Cannot add the global guard to the same command twice");
            _guardList.Add(GlobalGuard);
            return this;
        }

        /// <summary>
        /// This is a global implementation if IGuard that can optionally be used by commands
        /// </summary>
        public static IGuard GlobalGuard { get; } = new BasicGuard();

        /// <summary>
        /// Adds a guard implementation. Commands that share a guard cannot execute concurrently.
        /// Async Commands that share this guard cannot execute concurrently
        /// Commands can be given multiple guard implementations, though individual guard implementations
        /// can only be added once
        /// *CAUTION* Watch out for deadlock if you use the same Guard across multiple Pages.
        /// </summary>
        /// <param name="guard">A guard implementation to add to the Command being built</param>
        /// <returns></returns>
        public CommandBuilder AddGuard(IGuard guard)
        {
            if (_guardList.Contains(guard))
                throw new ArgumentException("Cannot add the same guard to the same command twice");
            _guardList.Add(guard);
            return this;
        }

        /// <summary>
        /// Sets a delegate that can be used to retrieve the name of the resultant Command. Can be bound to by the UI etc.
        /// Useful for swapping language at runtime
        /// </summary>
        /// <param name="getName">A delegate that returns a friendly name for the Command</param>
        /// <returns></returns>
        public CommandBuilder SetName(Func<string> getName)
        {
            if (_getName != null)
                throw new NotSupportedException("SetName cannot be called more than once");
            _getName = getName;
            return this;
        }

        /// <summary>
        /// Sets the name of the resultant Command. Can be bound to by the UI etc.
        /// </summary>
        /// <param name="name">The friendly name for the Command</param>
        /// <returns></returns>
        public CommandBuilder SetName(string name)
        {
            if (_getName != null)
                throw new NotSupportedException("SetName cannot be called more than once");
            _getName = () => name;
            return this;
        }

        public CommandBuilder AddObservedProperty(INotifyPropertyChanged propertySource, string propertyName)
        {
            return this.AddObservedProperty(propertySource, new string[] { propertyName });
        }

        public CommandBuilder AddObservedProperty(INotifyPropertyChanged propertySource, params string[] propertyNames)
        {
            if (propertySource == null)
                throw new ArgumentException("Cannot be null or empty", nameof(propertySource));

            if (propertyNames == null)
                throw new ArgumentException("Cannot be null or empty", nameof(propertyNames));

            if (_observedProperties.ContainsKey(propertySource) == false)
                _observedProperties.Add(propertySource, new HashSet<string>());

            foreach (string propertyName in propertyNames)
                _observedProperties[propertySource].Add(propertyName);

            return this;
        }
    }
}
